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
    class
        CollisionSystem : System //handle damage and other specific behavior in other systems, this is specifically for collisions
        //maybe link to other collision related systems here to avoid repeated calculations?
        //How will you handle complex collision behavior? you will need to access everything... :(
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

        private HashSet<CollisionComponent> _collisionComponents = new HashSet<CollisionComponent>();
        private HashSet<Pair> _moveableCollisionComponents = new HashSet<Pair>();

        private CollisionLogicSubsystem _logicSubsystem;

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
                        if (actor.Collision.IsPhysical && target.IsPhysical) HandlePhysicalCollision(actor, target, gameTime);
                        else HandleLogicalCollision(actor.Collision, target);
                    }
                }
            }
        }

        private void BindMovementComponents()
        {
            foreach (var component in _collisionComponents)
            {
                if (component.Bindings.TryGetValue(typeof(MovementComponent), out var bind))
                {
                    _moveableCollisionComponents.Add(new Pair(component, (MovementComponent) bind));
                }
            }
        }

        public override void Reset()
        {
            _moveableCollisionComponents.Clear();
            _collisionComponents.Clear();
            _logicSubsystem.ResetSystem();
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
            pair.Collision.CollisionRectangle.X = (int) pair.Movement.Position.X;
            pair.Collision.CollisionRectangle.Y = (int) pair.Movement.Position.Y;
        }

        private static void AlignMovement(Pair pair)
        {
            pair.Movement.Position.X = pair.Collision.CollisionRectangle.X;
            pair.Movement.Position.Y = pair.Collision.CollisionRectangle.Y;
        }

        private void MoveEntity(Pair entity, float newX, float newY)
        {
            entity.Movement.Position.X = newX;
            entity.Movement.Position.Y = newY;
            AlignPair(entity);
        }

        #endregion

        #region ComplexCollisionInteractions

        private void HandlePhysicalCollision(Pair actor, CollisionComponent target, GameTime gameTime) //TODO Implement Swept CC sometime
        {
            float entryTime = SweptAABB(actor, target, gameTime, out var normalX, out var normalY);
            if (entryTime <= 0 || entryTime >= 1) return;
            MoveToEntry(actor, entryTime, gameTime);
            if (actor.Collision.Behavior == CollisionBehavior.Block)
            {
                BlockActor(actor, normalX);
                ApplyRemainingSpeed(actor, 1 - entryTime, gameTime);
            }
            else if (actor.Collision.Behavior == CollisionBehavior.Bounce)
            {
                BounceActor(actor, normalX, normalY);
                ApplyRemainingSpeed(actor, 1 - entryTime, gameTime);
            }
            AlignMovement(actor);
        }

        private void HandleLogicalCollision(CollisionComponent actor, CollisionComponent target)
        {
            if (actor.Allegiance == target.Allegiance) return;
            _logicSubsystem.HandleCollision(actor, target);
            //boy thisll be hard //Update: it was.
        }

        #endregion

        #region AABB

        private static float SweptAABB(Pair actor, CollisionComponent target, GameTime gameTime, out int normalX, out int normalY)
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
                xEntryTime = xEntryDistance / (actor.Movement.Speed.Cartesian.X);
                xExitTime = xExitDistance / (actor.Movement.Speed.Cartesian.Y);
            }

            if (actor.Movement.Speed.Cartesian.Y == 0.0f)
            {
                yEntryTime = float.NegativeInfinity;
                yExitTime = float.PositiveInfinity;
            }
            else
            {
                yEntryTime = yEntryDistance / (actor.Movement.Speed.Cartesian.Y);
                yExitTime = yExitDistance / (actor.Movement.Speed.Cartesian.Y);
            }
            #endregion
            float entryTime = Math.Max(xEntryTime, yEntryTime);
            float exitTime = Math.Min(xExitTime, yExitTime);
            Console.WriteLine("Distance: " + xEntryDistance);
            Console.WriteLine("Time: " + xEntryTime);

            #region no collision happened
            if (entryTime > exitTime || (xEntryTime < 0.0f && yEntryTime < 0.0f) || (xEntryTime > 1.0f && yEntryTime > 1.0f))
            {
                Console.WriteLine(entryTime > exitTime);
                normalX = 0;
                normalY = 0;
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

        private static void BlockActor(Pair actor, int normalX)
        {
            if (normalX <= 0) actor.Movement.Speed.ChangeX(0);
            else actor.Movement.Speed.ChangeY(0);
        }

        private static void BounceActor(Pair actor, int normalX, int normalY)
        {
            actor.Movement.Speed.ChangeX(actor.Movement.Speed.Cartesian.X * normalX);
            actor.Movement.Speed.ChangeY(actor.Movement.Speed.Cartesian.Y * normalY);
        }
        #endregion
    }
}