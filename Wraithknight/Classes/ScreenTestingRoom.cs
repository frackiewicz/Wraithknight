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

        private Entity _hero; //To make things easier, like cameramovement

        private LevelGenerator _levelGenerator = new LevelGenerator();
        private Level _currentLevel;

        public ScreenTestingRoom(ScreenManager screenManager) : base(screenManager)
        {
            _camera = new Camera2D(screenManager.Graphics.GraphicsDevice);
            _ecs = new ECS(_camera);
            _internalGameTime = new GameTime();
            _ecs.StartupRoutine(ecsBootRoutine.Testing);
            _hero = _ecs.GetHero();
            _levelGenerator.ApplyPreset(LevelPreset.Test);
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
            if (Flags.Debug) DrawDebug();
            _screenManager.SpriteBatch.End();
            if(Flags.Debug) DrawDebugText();
            return this;
        }

        public override Screen LoadContent()
        {
            _currentLevel = _levelGenerator.GenerateLevel();
            _ecs.ProcessLevel(_currentLevel);
            return this;
        }

        public override Screen UnloadContent()
        {
            throw new NotImplementedException();
        }

        #region Debug

        public void ToggleDebug()
        {
            Flags.Debug = !Flags.Debug;

            Flags.ShowDrawRecs = !Flags.ShowDrawRecs;
            Flags.ShowCollisionRecs = !Flags.ShowCollisionRecs;
            Flags.ShowSpriteRecs = !Flags.ShowSpriteRecs;
        }

        public void DrawDebug()
        {
            _ecs.DrawDebug();
        }

        public void DrawDebugText()
        {
            _screenManager.SpriteBatch.Begin();
            Functions_Debugging.Draw();
            Functions_Debugging.Reset();
            _screenManager.SpriteBatch.End();
        }

        #endregion

        #region Input

        public override Screen HandleInput(GameTime gameTime)
        {
            if (InputReader.IsKeyTriggered(Keys.Escape)) OpenMenuScreen();
            if (InputReader.IsKeyTriggered(Keys.F1)) ToggleDebug();

            if (InputReader.IsKeyTriggered(Keys.M))
            {
                _currentLevel = _levelGenerator.GenerateLevel();
                _ecs.PurgeForNextLevel();
                _ecs.ProcessLevel(_currentLevel);
            }

            if (InputReader.IsKeyTriggered(Keys.N))
            {
                _levelGenerator.ApplySimpleCellularAutomata(_currentLevel, _levelGenerator.StarvationNumber, _levelGenerator.BirthNumber);
                _ecs.PurgeForNextLevel();
                _ecs.ProcessLevel(_currentLevel);
            }
            SimpleCameraMovement(gameTime);

            return this;
        }

        private void SimpleCameraMovement(GameTime gameTime)
        {
            if (InputReader.IsKeyPressed(Keys.Space)) _camera.FollowingHero = !_camera.FollowingHero; //TODO Cameramovement
            if (!_camera.FollowingHero)
            {
                if (InputReader.IsKeyPressed(Keys.Up)) _camera.TargetPosition.Y += 50 * (float)gameTime.ElapsedGameTime.TotalSeconds * 1 / _camera.CurrentZoom;
                if (InputReader.IsKeyPressed(Keys.Down)) _camera.TargetPosition.Y -= 50 * (float)gameTime.ElapsedGameTime.TotalSeconds * 1 / _camera.CurrentZoom;
                if (InputReader.IsKeyPressed(Keys.Left)) _camera.TargetPosition.X -= 50 * (float)gameTime.ElapsedGameTime.TotalSeconds * 1 / _camera.CurrentZoom;
                if (InputReader.IsKeyPressed(Keys.Right)) _camera.TargetPosition.X += 50 * (float)gameTime.ElapsedGameTime.TotalSeconds * 1 / _camera.CurrentZoom;
            }



            if (InputReader.IsScrollingUp()) _camera.TargetZoom += 1;
            if (InputReader.IsScrollingDown()) _camera.TargetZoom -= 1;

            if (_camera.TargetZoom < 0.1) _camera.TargetZoom = 0.3f;
            else if (_camera.TargetZoom > 1) _camera.TargetZoom = (int)_camera.TargetZoom;
        }

        #endregion



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
