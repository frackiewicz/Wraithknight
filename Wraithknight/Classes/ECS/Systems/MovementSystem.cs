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
                movement.Moving = !movement.Speed.Equals(Constants.NullVector);
                movement.Position += movement.Speed * (float) gameTime.ElapsedGameTime.TotalSeconds;
                ApplyInertia(movement, gameTime);
                AccelerateUntilMaxSpeed(movement, gameTime);


                Console.WriteLine(movement.Speed);
                Console.WriteLine(movement.Acceleration);
            }
        }

        public override void ResetSystem()
        {
            throw new NotImplementedException();
        }

        private void ApplyInertia(MovementComponent movement, GameTime gameTime)
        {
            if (movement.Speed.X > 0)
            {
                if (movement.Speed.X - movement.Friction * (float)gameTime.ElapsedGameTime.TotalSeconds <= 0) //step too small
                {
                    movement.Speed.X = 0; //snap
                }
                else
                {
                    movement.Speed.X -= movement.Friction * (float) gameTime.ElapsedGameTime.TotalSeconds;
                }
            }
            if (movement.Speed.X < 0)
            {
                if (movement.Speed.X + movement.Friction * (float)gameTime.ElapsedGameTime.TotalSeconds >= 0)
                {
                    movement.Speed.X = 0;
                }
                else
                {
                    movement.Speed.X += movement.Friction * (float) gameTime.ElapsedGameTime.TotalSeconds;
                }
            }
            if (movement.Speed.Y > 0)
            {
                if (movement.Speed.Y - movement.Friction * (float)gameTime.ElapsedGameTime.TotalSeconds <= 0)
                {
                    movement.Speed.Y = 0;
                }
                else
                {
                    movement.Speed.Y -= movement.Friction * (float) gameTime.ElapsedGameTime.TotalSeconds;
                }
            }
            if (movement.Speed.Y < 0)
            {
                if (movement.Speed.Y + movement.Friction * (float)gameTime.ElapsedGameTime.TotalSeconds >= 0)
                {
                    movement.Speed.Y = 0;
                }
                else
                {
                    movement.Speed.Y += movement.Friction * (float) gameTime.ElapsedGameTime.TotalSeconds;
                }
            }
        }

        private void AccelerateUntilMaxSpeed(MovementComponent movement, GameTime gameTime)
        {
            movement.Speed += (movement.Acceleration) * (float) gameTime.ElapsedGameTime.TotalSeconds;
            if (movement.MaxSpeed != 0.0f) //default
            {
                if (movement.Speed.X < -movement.MaxSpeed) { movement.Speed.X = -movement.MaxSpeed; }
                if (movement.Speed.X > movement.MaxSpeed) { movement.Speed.X = movement.MaxSpeed; }
                if (movement.Speed.Y < -movement.MaxSpeed) { movement.Speed.Y = -movement.MaxSpeed; }
                if (movement.Speed.Y > movement.MaxSpeed) { movement.Speed.Y = movement.MaxSpeed; }
            }
            
        }

        private bool DiagonalTooLong(MovementComponent movement) // TODO Breunig
        {
            return false;
        }
    }
}
