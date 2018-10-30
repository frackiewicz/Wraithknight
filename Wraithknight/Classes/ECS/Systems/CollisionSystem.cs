using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace Wraithknight
{
    class CollisionSystem : System //handle damage and other specific behavior in other systems, this is specifically for collisions
    //maybe link to other collision related systems here to avoid repeated calculations?
    //How will you handle complex collision behavior? you will need to access everything... :(
    {
        private struct Pair
        {
            public CollisionComponent Collision;
            public MovementComponent Movement;

            public Pair(CollisionComponent collision, MovementComponent movement)
            {
                Collision = collision;
                Movement = movement;
            }
        }
        private List<CollisionComponent> _collisionComponents = new List<CollisionComponent>();
        private List<Pair> _moveableCollisionComponents = new List<Pair>();

        public override void RegisterComponents(ICollection<Entity> entities) //modified version of CoupleComponents to allow pairing //Ugly as fuck
        {
            foreach (var entity in entities)
            {
                IEnumerable<Component> collisionComponents = entity.GetComponents<CollisionComponent>();
                if (collisionComponents != null)
                {
                    IEnumerable<Component> movementEnumerable = entity.GetComponents<MovementComponent>();
                    List<Component> movementComponents = null;
                    if (movementEnumerable != null)
                    {
                        movementComponents = movementEnumerable.ToList();
                    }
                    foreach (var collisionComponent in collisionComponents)
                    {
                        _collisionComponents.Add(Functions_Operators.CastComponent<CollisionComponent>(collisionComponent));
                        collisionComponent.Activate(); //do you want this?
                        if (movementComponents != null)
                        {
                            foreach (var movementComponent in movementComponents)
                            {
                                if(movementComponent.RootID.Equals(collisionComponent.RootID))
                                _moveableCollisionComponents.Add(new Pair(Functions_Operators.CastComponent<CollisionComponent>(collisionComponent), Functions_Operators.CastComponent<MovementComponent>(movementComponent)));
                            }
                        }
                    }
                }
                else { Console.WriteLine("Entity-" + entity.ID + " lacks " + typeof(CollisionComponent)); } // Output: Entity-0 lacks DrawComponent
            }
        }

        public override void Update(GameTime gameTime)
        {
            AlignAllPairs();

            foreach (var actor in _moveableCollisionComponents)
            {
                foreach (var target in _collisionComponents) 
                {
                    //you got some traps here, check other comments for info
                    //Fuck collisions, giving up
                    if (actor.Movement.IsMoving && actor.Collision != target)
                    {
                        if ((actor.Movement.Speed.Cartesian.X > 0 && IsTouchingLeft(actor, target, gameTime)) ||
                            (actor.Movement.Speed.Cartesian.X < 0 & IsTouchingRight(actor, target, gameTime)))
                        {
                            if (actor.Collision.Behavior == CollisionBehavior.Block)
                            {
                                actor.Movement.Position.X = actor.Movement.OldPosition.X;
                                actor.Movement.StopX();
                            }
                            else if (actor.Collision.Behavior == CollisionBehavior.Bounce)
                            {
                                actor.Movement.Speed.ChangeX(-actor.Movement.Speed.Cartesian.X);
                            }
                        }

                        if ((actor.Movement.Speed.Cartesian.Y > 0 && IsTouchingTop(actor, target, gameTime)) ||
                            (actor.Movement.Speed.Cartesian.Y < 0 & IsTouchingBottom(actor, target, gameTime)))
                        {
                            if (actor.Collision.Behavior == CollisionBehavior.Block)
                            {
                                actor.Movement.Position.Y = actor.Movement.OldPosition.Y;
                                actor.Movement.StopY();
                            }
                            else if (actor.Collision.Behavior == CollisionBehavior.Bounce)
                            {
                                actor.Movement.Speed.ChangeY(-actor.Movement.Speed.Cartesian.Y);
                            }
                        }

                        AlignPair(actor); 
                    }

                    if (actor.Collision.CollisionRectangle.Intersects(target.CollisionRectangle))
                    {

                    }
                }
            }
        }

        private void AlignAllPairs() //TODO talk with breunig how to handle multiple rectangle alignment
        {
            foreach (var pair in _moveableCollisionComponents)
            {
                AlignPair(pair);
            }
        }

        private static void AlignPair(Pair pair)
        {
            pair.Collision.CollisionRectangle.X = (int)pair.Movement.Position.X;
            pair.Collision.CollisionRectangle.Y = (int)pair.Movement.Position.Y;
        }

        private static void ApplyCollision(Pair pair)
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

        public override void ResetSystem()
        {
            throw new NotImplementedException();
        }

        #region AABB checking
        private bool IsTouchingLeft(Pair actor, CollisionComponent target, GameTime gameTime)
        {
            return actor.Collision.CollisionRectangle.Right + actor.Movement.Speed.Cartesian.X * gameTime.ElapsedGameTime.TotalSeconds > target.CollisionRectangle.Left &&
                   actor.Collision.CollisionRectangle.Left < target.CollisionRectangle.Left &&
                   actor.Collision.CollisionRectangle.Bottom > target.CollisionRectangle.Top &&
                   actor.Collision.CollisionRectangle.Top < target.CollisionRectangle.Bottom;
        }

        private bool IsTouchingRight(Pair actor, CollisionComponent target, GameTime gameTime)
        {
            return actor.Collision.CollisionRectangle.Left + actor.Movement.Speed.Cartesian.X * gameTime.ElapsedGameTime.TotalSeconds < target.CollisionRectangle.Right &&
                   actor.Collision.CollisionRectangle.Right > target.CollisionRectangle.Right &&
                   actor.Collision.CollisionRectangle.Bottom > target.CollisionRectangle.Top &&
                   actor.Collision.CollisionRectangle.Top < target.CollisionRectangle.Bottom;
        }

        private bool IsTouchingTop(Pair actor, CollisionComponent target, GameTime gameTime)
        {
            return actor.Collision.CollisionRectangle.Bottom + actor.Movement.Speed.Cartesian.Y * gameTime.ElapsedGameTime.TotalSeconds > target.CollisionRectangle.Top &&
                   actor.Collision.CollisionRectangle.Top < target.CollisionRectangle.Top &&
                   actor.Collision.CollisionRectangle.Right > target.CollisionRectangle.Left &&
                   actor.Collision.CollisionRectangle.Left < target.CollisionRectangle.Right;
        }

        private bool IsTouchingBottom(Pair actor, CollisionComponent target, GameTime gameTime)
        {
            return actor.Collision.CollisionRectangle.Top + actor.Movement.Speed.Cartesian.Y * gameTime.ElapsedGameTime.TotalSeconds < target.CollisionRectangle.Bottom &&
                   actor.Collision.CollisionRectangle.Bottom > target.CollisionRectangle.Bottom &&
                   actor.Collision.CollisionRectangle.Right > target.CollisionRectangle.Left &&
                   actor.Collision.CollisionRectangle.Left < target.CollisionRectangle.Right;
        }
        #endregion //doesnt take friction into account
    }
   
}
