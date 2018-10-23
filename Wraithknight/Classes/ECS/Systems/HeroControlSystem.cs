using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Wraithknight.Classes.Functions;

namespace Wraithknight.Classes.ECS.Systems
{
    class HeroControlSystem : System
    {
        public Entity Hero;

        private MovementComponent _movement;
        private InputComponent _input;

        public override void RegisterComponents(ICollection<Entity> entities)
        {
            foreach (Entity entity in entities)
            {
                if (entity.Type == EntityType.Hero)
                {
                    if (Hero != null)
                    {
                        Functions_ConsoleDebugging.DumpEntityInfo(entity);
                        Console.WriteLine("Multiple Heroes in Controller. Overwriting");
                    }

                    Hero = entity;

                    _movement = Functions_Operators.CastComponent<MovementComponent>(Hero.GetComponent<MovementComponent>());
                    _input = Functions_Operators.CastComponent<InputComponent>(Hero.GetComponent<InputComponent>());
                }
            }
        }

        public override void Update(GameTime gameTime)
        {
            _movement.Acceleration.X = 0;
            _movement.Acceleration.Y = 0;
            if (_input.CurrentKeyboardState.IsKeyDown(Keys.W))
            {
                _movement.Acceleration.Y = -_movement.AccelerationBase;
            }
            if (_input.CurrentKeyboardState.IsKeyDown(Keys.A))
            {
                _movement.Acceleration.X = -_movement.AccelerationBase;
            }
            if (_input.CurrentKeyboardState.IsKeyDown(Keys.S))
            {
                _movement.Acceleration.Y = _movement.AccelerationBase;
            }
            if (_input.CurrentKeyboardState.IsKeyDown(Keys.D))
            {
                _movement.Acceleration.X = _movement.AccelerationBase;
            }
        }

        public override void ResetSystem()
        {
            throw new NotImplementedException();
        }
    }
}