using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace Wraithknight.Classes.ECS.Systems
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
                    if (actor.Movement.IsMoving && actor.Collision.CollisionRectangle.Intersects(target.CollisionRectangle) && actor.Collision != target)
                    {
                        //do shit
                        //Lets use only AABB for now, more complex shit in last iterations
                        //TODO lots of redundance, clean up later
                        if (actor.Movement.Speed.Cartesian.X > 0)
                        {
                            actor.Movement.Position.X = target.CollisionRectangle.X - actor.Collision.CollisionRectangle.Width;
                            if (actor.Collision.Behavior == CollisionBehavior.Block)
                            {
                                actor.Movement.Speed.ChangeX(0);
                            }

                            if (actor.Collision.Behavior == CollisionBehavior.Bounce)
                            {
                                actor.Movement.Speed.ChangeX(-actor.Movement.Speed.Cartesian.X); //Does not lose energy: slightly unrealistic
                            }
                        }
                        else
                        {
                            actor.Movement.Position.X = target.CollisionRectangle.X + target.CollisionRectangle.Width;
                            if (actor.Collision.Behavior == CollisionBehavior.Block)
                            {
                                actor.Movement.Speed.ChangeX(0);
                            }

                            if (actor.Collision.Behavior == CollisionBehavior.Bounce)
                            {
                                actor.Movement.Speed.ChangeX(-actor.Movement.Speed.Cartesian.X); //Does not lose energy: slightly unrealistic
                            }
                        }

                        if (actor.Movement.Speed.Cartesian.Y > 0)
                        {
                            actor.Movement.Position.Y = target.CollisionRectangle.Y - actor.Collision.CollisionRectangle.Height;
                            if (actor.Collision.Behavior == CollisionBehavior.Block)
                            {
                                actor.Movement.Speed.ChangeY(0);
                            }

                            if (actor.Collision.Behavior == CollisionBehavior.Bounce)
                            {
                                actor.Movement.Speed.ChangeY(-actor.Movement.Speed.Cartesian.Y); //Does not lose energy: slightly unrealistic
                            }
                        }
                        else
                        {
                            actor.Movement.Position.Y = target.CollisionRectangle.Y + target.CollisionRectangle.Height;
                            if (actor.Collision.Behavior == CollisionBehavior.Block)
                            {
                                actor.Movement.Speed.ChangeY(0);
                            }

                            if (actor.Collision.Behavior == CollisionBehavior.Bounce)
                            {
                                actor.Movement.Speed.ChangeY(-actor.Movement.Speed.Cartesian.Y); //Does not lose energy: slightly unrealistic
                            }
                        }

                        AlignPair(actor);

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
    }
}
