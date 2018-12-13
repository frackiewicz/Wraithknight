﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Wraithknight;

namespace Wraithknight
{
    class MovementSystem : System
    {
        private HashSet<MovementComponent> _components = new HashSet<MovementComponent>();

        public MovementSystem(ECS ecs) : base(ecs)
        {
            _ecs = ecs;
        }

        public override void RegisterComponents(ICollection<Entity> entities)
        {
            CoupleComponent(_components, entities);
        }

        public override void Update(GameTime gameTime)
        {
            if (_components.Count <= 0) return;

            foreach (MovementComponent movement in _components)
            {
                if (movement.Inactive) continue;
                movement.IsMoving = !movement.Speed.Cartesian.Equals(Constants.NullVector);

                if (movement.HasCollided) movement.HasCollided = false;
                else ApplySpeed(movement, gameTime);

                ApplyInertia(movement, gameTime);
                AccelerateUntilMaxSpeed(movement, gameTime);
            }
        }

        public override void Reset()
        {
            _components.Clear();
        }

        public HashSet<MovementComponent> GetMovementComponents()
        {
            return _components;
        }

        private void ApplyInertia(MovementComponent movement, GameTime gameTime)
        {
            if (movement.Speed.Polar.Length > 0 && (movement.FrictionWhileMoving || movement.Acceleration.Equals(Vector2.Zero)))
            {
                if (movement.Speed.Polar.Length - movement.Friction * (float)gameTime.ElapsedGameTime.TotalSeconds < 0 )
                {
                    movement.Speed.ChangePolarLength(0);
                }
                else
                {
                    movement.Speed.ChangePolarLength(movement.Speed.Polar.Length - (movement.Friction * ((float) gameTime.ElapsedGameTime.TotalSeconds)));
                }
            }
        }

        private void AccelerateUntilMaxSpeed(MovementComponent movement, GameTime gameTime)
        {
            movement.Speed.AddVector2(movement.Acceleration * (float) gameTime.ElapsedGameTime.TotalSeconds);
            if (movement.MaxSpeed != 0.0f) //default
            {
                if (movement.Speed.Polar.Length >= movement.MaxSpeed)
                {
                    movement.Speed.ChangePolarLength(movement.MaxSpeed);
                }
            }
            
        }

        private void ApplySpeed(MovementComponent movement, GameTime gameTime)
        {
            movement.OldPosition.X = movement.Position.X;
            movement.OldPosition.Y = movement.Position.Y;
            movement.Position += movement.Speed.Cartesian * (float)gameTime.ElapsedGameTime.TotalSeconds; //TODO Breunig there has to be a better way
        }
    }
}
