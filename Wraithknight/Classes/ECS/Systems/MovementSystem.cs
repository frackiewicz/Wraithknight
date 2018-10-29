using System;
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
        private List<MovementComponent> _components = new List<MovementComponent>();

        public override void RegisterComponents(ICollection<Entity> entities)
        {
            CoupleComponents(_components, entities);
        }

        public override void Update(GameTime gameTime)
        {
            foreach (MovementComponent movement in _components)
            {
                movement.IsMoving = !movement.Speed.Cartesian.Equals(Constants.NullVector);

                ApplyInertia(movement, gameTime);
                AccelerateUntilMaxSpeed(movement, gameTime);
                movement.Position += movement.Speed.Cartesian * (float)gameTime.ElapsedGameTime.TotalSeconds;

            }
        }

        public override void ResetSystem()
        {
            throw new NotImplementedException();
        }

        private void ApplyInertia(MovementComponent movement, GameTime gameTime)
        {
            if (movement.Speed.Polar.Length > 0 && movement.Acceleration.Equals(Vector2.Zero))
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

        private bool DiagonalTooLong(MovementComponent movement) // TODO polar
        {
            return false;
        }
    }
}
