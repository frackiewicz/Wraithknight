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
    class ScreenGenerationTester : Screen
    {
        private readonly ECS _ecs;
        private readonly DebugDrawer _debugDrawer;
        private readonly Camera2D _camera;
        private GameTime _internalGameTime = new GameTime();

        private LevelGenerator _levelGenerator = new LevelGenerator();
        private Level _currentLevel;

        public ScreenGenerationTester(ScreenManager screenManager) : base(screenManager)
        {
            _camera = new Camera2D(screenManager.Graphics.GraphicsDevice) { FollowingHero = true };

            _ecs = new ECS(_camera);
            _ecs.StartupRoutine(ecsBootRoutine.Presenting);
            _debugDrawer = new DebugDrawer(_ecs);

            _levelGenerator.ApplyPreset(LevelPreset.Forest);
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
            _screenManager.SpriteBatch.Begin(SpriteSortMode.BackToFront, transformMatrix: _camera.View, samplerState: SamplerState.PointClamp);

            _ecs.Draw();
            if (Flags.Debug) _debugDrawer.Draw();

            _screenManager.SpriteBatch.End();
            if (Flags.Debug) DrawDebugText();
            return this;
        }

        public override Screen LoadContent()
        {
            _currentLevel = new Level(100, 100);
            _levelGenerator.StepFillWithNoise(_currentLevel);
            _ecs.PurgeForNextLevel();
            _ecs.ProcessLevel(_currentLevel);
            return this;
        }

        public override Screen UnloadContent()
        {
            throw new NotImplementedException();
        }

        #region Debug

        public void ToggleAllDebug()
        {
            if (Flags.Debug)
            {
                Flags.ShowDrawRecs = false;
                Flags.ShowCollisionRecs = false;
                Flags.ShowDebuggingText = false;
                Flags.ShowMovementCenters = false;
            }
            else
            {
                Flags.ShowDrawRecs = true;
                Flags.ShowCollisionRecs = true;
                Flags.ShowDebuggingText = true;
                Flags.ShowMovementCenters = true;
            }

        }

        public void DrawDebugText() //TODO move into debugdrawer?
        {
            if (Flags.ShowDebuggingText)
            {
                _screenManager.SpriteBatch.Begin();
                Functions_DebugWriter.Draw();
                _screenManager.SpriteBatch.End();
            }
        }

        #endregion

        #region Input

        public override Screen HandleInput(GameTime gameTime)
        {
            if (InputReader.IsKeyTriggered(Keys.Escape)) OpenMenuScreen();

            if (InputReader.IsKeyTriggered(Keys.D1))
            {
                _levelGenerator.StepFillWithNoise(_currentLevel);
                ApplyLevel(_currentLevel);
            }
            if (InputReader.IsKeyTriggered(Keys.D2))
            {
                _levelGenerator.StepFillBoundsWithNoise(_currentLevel);
                ApplyLevel(_currentLevel);
            }
            if (InputReader.IsKeyTriggered(Keys.D3))
            {
                _levelGenerator.StepApplyCellularAutomata(_currentLevel);
                ApplyLevel(_currentLevel);
            }
            if (InputReader.IsKeyTriggered(Keys.D4))
            {
                _levelGenerator.StepMapCleanUp(_currentLevel);
                ApplyLevel(_currentLevel);
            }
            if (InputReader.IsKeyTriggered(Keys.D5))
            {
                _levelGenerator.StepResizeLevelToEdges(_currentLevel);
                ApplyLevel(_currentLevel);
            }
            if (InputReader.IsKeyTriggered(Keys.D6))
            {
                _levelGenerator.StepSpawnEntities(_currentLevel);
                ApplyLevel(_currentLevel);
            }

            if (InputReader.IsKeyTriggered(Keys.M))
            {
                _currentLevel = _levelGenerator.GenerateLevel();
                _ecs.PurgeForNextLevel();
                _ecs.ProcessLevel(_currentLevel);
            }

            SimpleCameraMovement(gameTime);

            return this;
        }

        private void ApplyLevel(Level level)
        {
            _ecs.PurgeForNextLevel();
            _ecs.ProcessLevel(level);
        }

        private void SimpleCameraMovement(GameTime gameTime)
        {
            if (InputReader.IsKeyTriggered(Keys.Space)) _camera.FollowingHero = !_camera.FollowingHero;

            if (InputReader.IsKeyPressed(Keys.Up)) _camera.TargetPosition.Y -= 500 * (float)gameTime.ElapsedGameTime.TotalSeconds * 1 / _camera.CurrentZoom;
            if (InputReader.IsKeyPressed(Keys.Down)) _camera.TargetPosition.Y += 500 * (float)gameTime.ElapsedGameTime.TotalSeconds * 1 / _camera.CurrentZoom;
            if (InputReader.IsKeyPressed(Keys.Left)) _camera.TargetPosition.X -= 500 * (float)gameTime.ElapsedGameTime.TotalSeconds * 1 / _camera.CurrentZoom;
            if (InputReader.IsKeyPressed(Keys.Right)) _camera.TargetPosition.X += 500 * (float)gameTime.ElapsedGameTime.TotalSeconds * 1 / _camera.CurrentZoom;



            if (InputReader.IsScrollingUp()) _camera.TargetZoom += 1;
            if (InputReader.IsScrollingDown()) _camera.TargetZoom -= 1;

            if (_camera.TargetZoom < 0.1) _camera.TargetZoom = 0.3f;
            else if (_camera.TargetZoom > 1) _camera.TargetZoom = (int)_camera.TargetZoom;
        }

        #endregion

        private readonly TimeSpan _gameTimeCull = TimeSpan.FromMilliseconds(1000.0f / 30);

        private void UpdateGameTime(GameTime gameTime)
        {
            if (Flags.FpsBelowThreshold)
            {
                _internalGameTime.ElapsedGameTime = _gameTimeCull;
                _internalGameTime.TotalGameTime += _gameTimeCull;
            }
            else
            {
                _internalGameTime.ElapsedGameTime = gameTime.ElapsedGameTime;
                _internalGameTime.TotalGameTime += gameTime.ElapsedGameTime;
            }
        }

        private void OpenMenuScreen()
        {
            _screenManager.AddScreen(new ScreenGameMenu(_screenManager, screenGenerationTester: this));
        }
    }
}
