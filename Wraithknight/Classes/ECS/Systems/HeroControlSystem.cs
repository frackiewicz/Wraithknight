﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Wraithknight
{
    class HeroControlSystem : System
    {
        public Entity Hero;
        public ECS _ecs;

        private MovementComponent _movement;
        private InputComponent _input;

        public HeroControlSystem(ECS ecs)
        {
            _ecs = ecs;
        }

        public override void RegisterComponents(ICollection<Entity> entities)
        {
            foreach (Entity entity in entities)
            {
                if (entity.Type == EntityType.Hero)
                {
                    RegisterHero(entity);
                }
            }
        }

        public void RegisterHero(Entity entity)
        {
            if (Hero != null)
            {
                Functions_ConsoleDebugging.DumpEntityInfo(entity);
                Console.WriteLine("Multiple Heroes in ControlSystem. Overwriting");
            }

            Hero = entity;

            _movement = Functions_Operators.CastComponent<MovementComponent>(Hero.GetComponent<MovementComponent>());
            _input = Functions_Operators.CastComponent<InputComponent>(Hero.GetComponent<InputComponent>());
        }

        public override void Update(GameTime gameTime)
        {
            _movement.Acceleration.X = _input.MovementDirection.X * _movement.AccelerationBase;
            _movement.Acceleration.Y = _input.MovementDirection.Y * _movement.AccelerationBase;
            if (_input.PrimaryAttack)
            {
                _ecs.RegisterEntity(_ecs.CreateEntity(EntityType.KnightSlash, position: _movement.Position, speed: new Coord2(new Vector2(_input.CursorPoint.X - _movement.Position.X, _input.CursorPoint.Y - _movement.Position.Y))));
            }
        }

        public override void ResetSystem()
        {
            throw new NotImplementedException();
        }
    }
}
    
