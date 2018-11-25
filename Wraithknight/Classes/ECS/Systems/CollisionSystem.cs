using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Wraithknight
{
    class CollisionSystem : System //TODO Breunig Collision System definetly needs a performance boost
    {
        private struct Pair
        {
            public readonly CollisionComponent Collision;
            public readonly MovementComponent Movement;

            public Pair(CollisionComponent collision, MovementComponent movement)
            {
                Collision = collision;
                Movement = movement;
            }
        }

        private readonly HashSet<CollisionComponent> _collisionComponents = new HashSet<CollisionComponent>();
        private readonly HashSet<Pair> _moveableCollisionComponents = new HashSet<Pair>();

        private CollisionComponent[,] _walls;

        private readonly CollisionLogicSubsystem _logicSubsystem;

        #region General System Stuff

        public CollisionSystem(ECS ecs) : base(ecs)
        {
            _ecs = ecs;
            _logicSubsystem = new CollisionLogicSubsystem(ecs);
        }


        public override void RegisterComponents(ICollection<Entity> entities)
        {
            CoupleComponent(_collisionComponents, entities);
            BindMovementComponents();
            _logicSubsystem.RegisterComponents(entities);
        }

        private void BindMovementComponents()
        {
            foreach (var component in _collisionComponents)
            {
                if (component.Bindings.TryGetValue(typeof(MovementComponent), out var bind))
                {
                    _moveableCollisionComponents.Add(new Pair(component, (MovementComponent)bind));
                }
            }
        }

        public override void Reset()
        {
            _moveableCollisionComponents.Clear();
            _collisionComponents.Clear();
            _logicSubsystem.ResetSystem();
        }

        public HashSet<CollisionComponent> GetCollisionComponents()
        {
            return _collisionComponents;
        }

        #endregion

        public override void Update(GameTime gameTime)
        {
            AlignAllPairs();

            foreach (var actor in _moveableCollisionComponents)
            {
                if (actor.Collision.Inactive) continue;
                foreach (var target in _collisionComponents)
                {
                    if (target.Inactive) continue;
                    if (actor.Movement.IsMoving && !actor.Collision.Equals(target))
                    {
                        if (actor.Collision.IsPhysical && target.IsPhysical) HandlePhysicalCollision(actor, target, gameTime, false);
                        else HandleLogicalCollision(actor.Collision, target);
                    }
                }
            }
        }

        

        #region General CD stuff

        private void AlignAllPairs()
        {
            foreach (var pair in _moveableCollisionComponents)
            {
                AlignPair(pair);
            }
        }

        private static void AlignPair(Pair pair)
        {
            pair.Collision.CollisionRectangle.SetCenter(pair.Movement.Position);
        }

        private static void AlignMovement(Pair pair)
        {
            pair.Movement.Position.X = pair.Collision.CollisionRectangle.Center.X;
            pair.Movement.Position.Y = pair.Collision.CollisionRectangle.Center.Y;
        }

        private void MoveEntity(Pair entity, float newX, float newY)
        {
            entity.Movement.Position.X = newX;
            entity.Movement.Position.Y = newY;
            AlignPair(entity);
        }

        #endregion

        #region ComplexCollisionInteractions

        private void HandlePhysicalCollision(Pair actor, CollisionComponent target, GameTime gameTime, bool swept) //TODO Implement Swept CC sometime
        {
            if (swept)
            {
                float entryTime = SweptAABB(actor, target, gameTime, out var normalX, out var normalY);
                if (entryTime <= 0 || entryTime >= 1) return;
                MoveToEntry(actor, entryTime, gameTime);
                HandleCollisionBehavior(actor, normalX, normalY);
                ApplyRemainingSpeed(actor, 1 - entryTime, gameTime);

            }
            else //TODO Blocking works fine now, figure out the problem with normals next, also Speedblocking doesnt work yet
            {
                if (actor.Collision.CollisionRectangle.Intersects(target.CollisionRectangle))
                {
                    Vector2 penetrationVector = ClosestPointOnBoundsToPoint(CalculateMinkowskiDifference(actor.Collision.CollisionRectangle, target.CollisionRectangle), Vector2.Zero);
                    actor.Collision.CollisionRectangle -= penetrationVector;
                    Point normals = PenetrationToNormals(penetrationVector);
                    HandleCollisionBehavior(actor, normals.X, normals.Y);
                }
            }
            
            AlignMovement(actor);
        }

        public void DrawMinkowski()
        {
            foreach (var actor in _moveableCollisionComponents)
            {
                if (actor.Collision.Inactive) continue;
                foreach (var target in _collisionComponents)
                {
                    if (target.Inactive) continue;
                    if (actor.Movement.IsMoving && !actor.Collision.Equals(target))
                    {
                        Functions_Draw.DrawDebug(CalculateMinkowskiDifference(actor.Collision.CollisionRectangle, target.CollisionRectangle));
                    }
                }
            }
        }

        private void HandleCollisionBehavior(Pair actor, int normalX, int normalY)
        {
            if (actor.Collision.Behavior == CollisionBehavior.Block)
            {
                BlockActor(actor, normalX, normalY);
            }
            else if (actor.Collision.Behavior == CollisionBehavior.Bounce)
            {
                BounceActor(actor, normalX, normalY);
            }
        }

        private Point PenetrationToNormals(Vector2 vector)
        {
            Point result = new Point();
            
            if (vector.X < 0) result.X = -1;
            else if (vector.X > 0) result.X = 1;
            if (vector.Y < 0) result.Y = -1;
            else if (vector.Y > 0) result.Y = 1;
            
            return result;
        } //TODO This has some Issues

        private void HandleLogicalCollision(CollisionComponent actor, CollisionComponent target)
        {
            if (actor.Allegiance == target.Allegiance) return;
            _logicSubsystem.HandleCollision(actor, target);
        }

        #endregion

        #region AABB

        private static float SweptAABB(Pair actor, CollisionComponent target, GameTime gameTime, out int normalX, out int normalY) //TODO THIS ALGORITHM IS NOT FUNCTIONAL, TOO LAZY TO FIX IT NOW
        {
            float xEntryDistance, yEntryDistance;
            float xExitDistance, yExitDistance;

            #region calculate distances
            if (actor.Movement.Speed.Cartesian.X > 0.0f)
            {
                xEntryDistance = target.CollisionRectangle.X - (actor.Collision.CollisionRectangle.X + actor.Collision.CollisionRectangle.Width);
                xExitDistance = (target.CollisionRectangle.X + target.CollisionRectangle.Width) - actor.Collision.CollisionRectangle.X;
            }
            else
            {
                xEntryDistance = (target.CollisionRectangle.X + target.CollisionRectangle.Width) - actor.Collision.CollisionRectangle.X;
                xExitDistance = target.CollisionRectangle.X - (actor.Collision.CollisionRectangle.X + actor.Collision.CollisionRectangle.Width);
            }

            if (actor.Movement.Speed.Cartesian.Y > 0.0f)
            {
                yEntryDistance = target.CollisionRectangle.Y - (actor.Collision.CollisionRectangle.Y + actor.Collision.CollisionRectangle.Height);
                yExitDistance = (target.CollisionRectangle.Y + target.CollisionRectangle.Height) - actor.Collision.CollisionRectangle.Y;
            }
            else
            {
                yEntryDistance = (target.CollisionRectangle.Y + target.CollisionRectangle.Height) - actor.Collision.CollisionRectangle.Y;
                yExitDistance = target.CollisionRectangle.Y - (actor.Collision.CollisionRectangle.Y + actor.Collision.CollisionRectangle.Height);
            }
            #endregion

            float xEntryTime, yEntryTime;
            float xExitTime, yExitTime;

            #region calculate entry and exit times
            if (actor.Movement.Speed.Cartesian.X == 0.0f)
            {
                xEntryTime = float.NegativeInfinity;
                xExitTime = float.PositiveInfinity;
            }
            else
            {
                xEntryTime = xEntryDistance / (actor.Movement.Speed.Cartesian.X * (float)gameTime.ElapsedGameTime.TotalSeconds);
                xExitTime = xExitDistance / (actor.Movement.Speed.Cartesian.Y * (float)gameTime.ElapsedGameTime.TotalSeconds);
            }

            if (actor.Movement.Speed.Cartesian.Y == 0.0f)
            {
                yEntryTime = float.NegativeInfinity;
                yExitTime = float.PositiveInfinity;
            }
            else
            {
                yEntryTime = yEntryDistance / (actor.Movement.Speed.Cartesian.Y * (float)gameTime.ElapsedGameTime.TotalSeconds);
                yExitTime = yExitDistance / (actor.Movement.Speed.Cartesian.Y * (float)gameTime.ElapsedGameTime.TotalSeconds);
            }
            #endregion
            if (xEntryTime > 1.0f) xEntryTime = -float.MaxValue;
            if (yEntryTime > 1.0f) yEntryTime = -float.MaxValue;
            float entryTime = Math.Max(xEntryTime, yEntryTime);
            float exitTime = Math.Min(xExitTime, yExitTime);
            Functions_DebugWriter.WriteLine("Distance: " + xEntryDistance);
            Functions_DebugWriter.WriteLine("Time: " + xEntryTime);

            #region no collision happened
            if ((entryTime > exitTime) || (xEntryTime < 0.0f && yEntryTime < 0.0f))
            {
                normalX = 0;
                normalY = 0;
                Functions_DebugWriter.WriteLine("No Collision");
                return 1.0f;
            }
            
            #endregion

            #region set normals
            if (xEntryTime > yEntryTime)
            {
                normalX = -1;
                normalY = 1;
            }
            else
            {
                normalX = 1;
                normalY = -1;
            }
            #endregion
            return entryTime;
        }



        private static AABB CalculateMinkowskiDifference(AABB actor, AABB target)
        {
            return new AABB(actor.X - target.Right, actor.Y - target.Bottom, actor.Width + target.Width, actor.Height + target.Height);
        }

        private static Vector2 CalculatePenetrationVector(AABB actor, Vector2 point) //primarily used for minkowski
        {
            throw new NotImplementedException();
        }

        public static Vector2 ClosestPointOnBoundsToPoint(AABB actor, Vector2 point) //TODO you can do better...
        {
            float distance = Math.Abs(actor.Left - point.X);
            Vector2 pointOnBounds = new Vector2(actor.Left, point.Y);
            if (Math.Abs(actor.Right - point.X) < distance)
            {
                distance = Math.Abs(actor.Right - point.X);
                pointOnBounds = new Vector2(actor.Right, point.Y);
            }
            if (Math.Abs(actor.Bottom - point.Y) < distance)
            {
                distance = Math.Abs(actor.Bottom - point.Y);
                pointOnBounds = new Vector2(point.X, actor.Bottom);
            }
            if (Math.Abs(actor.Top - point.Y) < distance)
            {
                distance = Math.Abs(actor.Top - point.Y);
                pointOnBounds = new Vector2(point.X, actor.Top);
            }

            return pointOnBounds;
        }

        private static void MoveToEntry(Pair actor, float entryTime, GameTime gameTime)
        {
            actor.Collision.CollisionRectangle.X += (int)(actor.Movement.Speed.Cartesian.X * (entryTime * (float)gameTime.ElapsedGameTime.TotalSeconds));
            actor.Collision.CollisionRectangle.Y += (int)(actor.Movement.Speed.Cartesian.Y * (entryTime * (float)gameTime.ElapsedGameTime.TotalSeconds));
        }

        private static void ApplyRemainingSpeed(Pair actor, float remainingTime, GameTime gameTime)
        {
            actor.Collision.CollisionRectangle.X += (int)(actor.Movement.Speed.Cartesian.X * (remainingTime * (float)gameTime.ElapsedGameTime.TotalSeconds));
            actor.Collision.CollisionRectangle.Y += (int)(actor.Movement.Speed.Cartesian.Y * (remainingTime * (float)gameTime.ElapsedGameTime.TotalSeconds));
        }

        private static void BlockActor(Pair actor, Point normals)
        {
            BlockActor(actor, normals.X, normals.Y);
        }

        private static void BlockActor(Pair actor, int normalX, int normalY)
        {
            if (normalX != 0) actor.Movement.Speed.ChangeX(0);
            if (normalY != 0) actor.Movement.Speed.ChangeY(0);
        }

        private static void BounceActor(Pair actor, int normalX, int normalY)
        {
            actor.Movement.Speed.ChangeX(actor.Movement.Speed.Cartesian.X * normalX);
            actor.Movement.Speed.ChangeY(actor.Movement.Speed.Cartesian.Y * normalY);
        }
        #endregion
    }
}