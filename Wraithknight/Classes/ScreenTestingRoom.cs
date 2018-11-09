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
        private readonly ECS _ecs;
        private readonly Camera2D _camera;
        private GameTime _internalGameTime; //TODO refactor this to gameTime, and higher level gameTime to deltaTime

        public ScreenTestingRoom(ScreenManager screenManager) : base(screenManager)
        {
            _camera = new Camera2D(screenManager.Graphics.GraphicsDevice);
            _ecs = new ECS(_camera);
            _internalGameTime = new GameTime();
            _ecs.StartupRoutine(ecsBootRoutine.Testing);
        }

        public override Screen Update(GameTime gameTime)
        {
            UpdateGameTime(gameTime);
            _ecs.UpdateSystems(_internalGameTime);
            _camera.Update(_internalGameTime);
            return this;
        }

        public override Screen Draw(GameTime gameTime)
        {
            _screenManager.SpriteBatch.Begin(transformMatrix: _camera.View);
            _ecs.Draw();
            _screenManager.SpriteBatch.End();
            return this;
        }

        public override Screen LoadContent()
        {
            return this;
        }

        public override Screen UnloadContent()
        {
            throw new NotImplementedException();
        }

        public override Screen HandleInput(GameTime gameTime)
        {
            if (InputReader.IsKeyTriggered(Keys.Escape)) OpenMenuScreen();
            if (InputReader.IsKeyTriggered(Keys.F1)) ToggleDebug();
            SimpleCameraMovement(gameTime);


            return this;
        }

        public void ToggleDebug()
        {
            Flags.ShowDrawRecs = !Flags.ShowDrawRecs;
            Flags.ShowCollisionRecs = !Flags.ShowCollisionRecs;
            Flags.ShowSpriteRecs = !Flags.ShowSpriteRecs;
        }

        private void SimpleCameraMovement(GameTime gameTime)
        {
            if (InputReader.IsKeyPressed(Keys.Up))
            {
                _camera.TargetPosition.Y += 50 * (float) gameTime.ElapsedGameTime.TotalSeconds * 1/_camera.CurrentZoom;
            }
            if (InputReader.IsKeyPressed(Keys.Down))
            {
                _camera.TargetPosition.Y -= 50 * (float)gameTime.ElapsedGameTime.TotalSeconds * 1 / _camera.CurrentZoom;
            }
            if (InputReader.IsKeyPressed(Keys.Left))
            {
                _camera.TargetPosition.X -= 50 * (float)gameTime.ElapsedGameTime.TotalSeconds * 1 / _camera.CurrentZoom;
            }
            if (InputReader.IsKeyPressed(Keys.Right))
            {
                _camera.TargetPosition.X += 50 * (float)gameTime.ElapsedGameTime.TotalSeconds * 1 / _camera.CurrentZoom;
            }


            if (InputReader.IsScrollingUp()) _camera.TargetZoom += 1;
            if (InputReader.IsScrollingDown()) _camera.TargetZoom -= 1;

            if (_camera.TargetZoom < 0.1) _camera.TargetZoom = 0.1f;
            else if (_camera.TargetZoom > 1) _camera.TargetZoom = (int) _camera.TargetZoom;
        }

        private void UpdateGameTime(GameTime gameTime)
        {
            _internalGameTime.ElapsedGameTime = gameTime.ElapsedGameTime;
            _internalGameTime.TotalGameTime += gameTime.ElapsedGameTime;
        }

        private void OpenMenuScreen()
        {
            _screenManager.AddScreen(new GameMenuScreen(_screenManager));
        }
    }
}
