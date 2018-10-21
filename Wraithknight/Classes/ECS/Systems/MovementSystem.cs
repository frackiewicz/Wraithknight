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
                movement.Moving = movement.Speed.Equals(Constants.NullVector);
                movement.Position += movement.Speed * gameTime.ElapsedGameTime.Seconds;
                ApplyInertia(movement, gameTime);
                movement.Speed += movement.Acceleration * gameTime.ElapsedGameTime.Seconds;
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
                if (movement.Speed.X - movement.Friction <= 0) //step too small
                {
                    movement.Speed.X = 0; //snap
                }
                else
                {
                    movement.Speed.X -= movement.Friction * gameTime.ElapsedGameTime.Seconds;
                }
            }

            if (movement.Speed.X < 0)
            {
                if (movement.Speed.X + movement.Friction >= 0)
                {
                    movement.Speed.X = 0;
                }
                else
                {
                    movement.Speed.X += movement.Friction * gameTime.ElapsedGameTime.Seconds;
                }
            }

            if (movement.Speed.Y > 0)
            {
                if (movement.Speed.Y - movement.Friction <= 0)
                {
                    movement.Speed.Y = 0;
                }
                else
                {
                    movement.Speed.Y -= movement.Friction * gameTime.ElapsedGameTime.Seconds;
                }
            }

            if (movement.Speed.Y < 0)
            {
                if (movement.Speed.Y + movement.Friction >= 0)
                {
                    movement.Speed.Y = 0;
                }
                else
                {
                    movement.Speed.Y += movement.Friction * gameTime.ElapsedGameTime.Seconds;
                }
            }
        }
    }
}
