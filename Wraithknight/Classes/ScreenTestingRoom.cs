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
        private readonly Camera2D _camera;

        public ScreenTestingRoom(ScreenManager screenManager, GraphicsDevice graphics)
        {
            _screenManager = screenManager;
            _ecs = new ECS();
            _ecs.StartupRoutine(ecsBootRoutine.Testing);
            _camera = new Camera2D(graphics);
        }

        public override Screen Update(GameTime gameTime)
        {
            _ecs.UpdateSystems(gameTime);
            _camera.Update(gameTime);
            return this;
        }

        public override Screen Draw(GameTime gameTime)
        {
            _screenManager.SpriteBatch.Begin(transformMatrix: _camera.View);
            _ecs.Draw();
            _screenManager.SpriteBatch.End();
            return this;
        }

        public override Screen HandleInput(GameTime gameTime)
        {
            if (InputReader.IsKeyPressed(Keys.F))
            {
                GC.Collect();
            }

            if (InputReader.IsKeyTriggered(Keys.F1))
            {
                Flags.ShowDrawRecs = !Flags.ShowDrawRecs;
                Flags.ShowCollisionRecs = !Flags.ShowCollisionRecs;
                Flags.ShowSpriteRecs = !Flags.ShowSpriteRecs;
            }

            SimpleCameraMovement(gameTime);

            base.HandleInput(gameTime);
            return this;
        }

        private void SimpleCameraMovement(GameTime gameTime)
        {
            if (InputReader.IsKeyPressed(Keys.Up))
            {
                _camera.TargetPosition.Y += 50 * (float) gameTime.ElapsedGameTime.TotalSeconds;
            }
            if (InputReader.IsKeyPressed(Keys.Down))
            {
                _camera.TargetPosition.Y -= 50 * (float)gameTime.ElapsedGameTime.TotalSeconds;
            }
            if (InputReader.IsKeyPressed(Keys.Left))
            {
                _camera.TargetPosition.X -= 50 * (float)gameTime.ElapsedGameTime.TotalSeconds;
            }
            if (InputReader.IsKeyPressed(Keys.Right))
            {
                _camera.TargetPosition.X += 50 * (float)gameTime.ElapsedGameTime.TotalSeconds;
            }

            if (InputReader.IsScrollingUp())
            {
                _camera.TargetZoom += 300 * (float) gameTime.ElapsedGameTime.TotalSeconds;
            }

            if (InputReader.IsScrollingDown())
            {
               _camera.TargetZoom -= 300 * (float) gameTime.ElapsedGameTime.TotalSeconds;
               if(_camera.TargetZoom < 0.1) _camera.TargetZoom = 0.1f;
            }
        }
    }
}
