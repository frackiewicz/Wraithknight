﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Wraithknight
{
    class CollisionSystem : System //handle damage and other specific behavior in other systems, this is specifically for collisions
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

        public CollisionSystem(ECS ecs) : base(ecs)
        {
            _ecs = ecs;
        }


        public override void RegisterComponents(ICollection<Entity> entities) //modified version of CoupleComponent to allow pairing //Ugly as fuck
        {
            CoupleComponent(_collisionComponents, entities);
            BindMovementComponents();
        }

        public override void Update(GameTime gameTime)
        {
            AlignAllPairs();

            foreach (var actor in _moveableCollisionComponents)
            {
                if (!actor.Collision.Active) continue;
                foreach (var target in _collisionComponents) 
                {
                    if (!target.Active) continue;
                    //you got some traps here, check other comments for info
                    //Fuck collisions, giving up
                    //ALSO IT ROYALLY FUCKS YOUR PERFORMANCE
                    //Current bandaid doesnt work on lower speeds!
                    if (actor.Movement.IsMoving && actor.Collision != target) //TODO breunig talk about performance of collision
                    {
                        if (actor.Collision.Behavior == CollisionBehavior.Block || actor.Collision.Behavior == CollisionBehavior.Bounce) HandlePhysicalCollision(actor, target, gameTime);   
                        else if (actor.Collision.Behavior == CollisionBehavior.Disappear || actor.Collision.Behavior == CollisionBehavior.Pass) HandleLogicalCollision(actor.Collision, target);
                    }
                }
            }
        }

        private void BindMovementComponents()
        {
            Component bind;
            foreach (var component in _collisionComponents)
            {
                if (component.Bindings.TryGetValue(typeof(MovementComponent), out bind))
                {
                    _moveableCollisionComponents.Add(new Pair(component, (MovementComponent)bind));
                }
            }
        }

        public override void Reset()
        {
            _moveableCollisionComponents.Clear();
            _collisionComponents.Clear();
        }

        #region General CD stuff

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

        #endregion

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

        #region ComplexCollisionInteractions

        private void HandlePhysicalCollision(Pair actor, CollisionComponent target, GameTime gameTime)
        {
            if (target.Behavior == CollisionBehavior.Block)
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
        }

        private void HandleLogicalCollision(CollisionComponent actor, CollisionComponent target) //fuckfuckfuck
        {
            
            if (actor.Behavior == CollisionBehavior.Disappear) //currently the source kills the projectile, and the projectiles kill each other as well
            {
                if (actor.CollisionRectangle.Intersects(target.CollisionRectangle))
                {
                    _ecs.KillGameObject(actor.RootID);
                }
                return;
            }
            /*
            if (actor.Behavior == CollisionBehavior.Pass)
            {
                if (actor.IsProjectile)
                {
                    (_ecs.GetEntity(actor.RootID).GetComponent<ProjectileComponent>() as ProjectileComponent).CurrentTargets.Add(_ecs.GetEntity(target.RootID));
                }
                //boy thisll be hard
            }
            */
            
        }

        #endregion
    }
   
}
