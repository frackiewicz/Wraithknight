using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Wraithknight
{
    class ScreenTestingRoom : Screen
    {
        private readonly ScreenManager _screenManager;
        private readonly ECS _ecs;

        public ScreenTestingRoom(ScreenManager screenManager)
        {
            _screenManager = screenManager;
            _ecs = new ECS();
            _ecs.StartupRoutine(ecsBootRoutine.Testing);
        }

        public override Screen Update(GameTime gameTime)
        {
            _ecs.UpdateSystems(gameTime);
            return this;
        }

        public override Screen Draw(GameTime gameTime)
        {
            _screenManager.SpriteBatch.Begin();
            _ecs.Draw();
            _screenManager.SpriteBatch.End();
            return this;
        }

        private int _i = 0;
        public override Screen HandleInput(GameTime gameTime)
        {
            float deltaSeconds = (float)gameTime.ElapsedGameTime.TotalSeconds;
            int speed = 100;
            /*
            if (_input.IsKeyPressed(Keys.A))
            {
                _hero.MoveActor(new Vector2(-speed * deltaSeconds, 0));
                Console.Write("pressed A");
            }
            if (_input.IsKeyPressed(Keys.W))
            {
                _hero.MoveActor(new Vector2(0, -speed * deltaSeconds));
            }
            if (_input.IsKeyPressed(Keys.S))
            {
                _hero.MoveActor(new Vector2(0, speed * deltaSeconds));
            }
            if (_input.IsKeyPressed(Keys.D))
            {
                _hero.MoveActor(new Vector2(speed * deltaSeconds, 0));
            }
            */
            if (InputReader.IsKeyPressed(Keys.Space))
            {

            }

            if (InputReader.IsKeyPressed(Keys.F))
            {
                GC.Collect();
            }

            if (InputReader.IsKeyTriggered(Keys.F1))
            {
                Flags.ShowDrawRecs = !Flags.ShowDrawRecs;
                Flags.ShowCollisionRecs = !Flags.ShowCollisionRecs;
                Flags.ShowSpriteRecs = !Flags.ShowSpriteRecs;
                Console.WriteLine("F1 : " + _i);
                _i++;
            }
            base.HandleInput(gameTime);
            return this;
        }
    }
}
