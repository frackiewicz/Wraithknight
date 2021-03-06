﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Wraithknight
{
    class ScreenGame : Screen
    {
        private readonly ECS _ecs;
        private readonly DebugDrawer _debugDrawer;
        private readonly Camera2D _camera;
        private GameTime _internalGameTime = new GameTime();

        private Entity _hero;

        private LevelGenerator _levelGenerator = new LevelGenerator();
        private Level _currentLevel; //TODO Make Level into a super class, current level becomes LevelSpawnData
        private LevelTracker _currentLevelTracker;
        private GameTracker _gameTracker = new GameTracker();

        public ScreenGame(ScreenManager screenManager) : base(screenManager)
        {
            _camera = new Camera2D(screenManager.Graphics.GraphicsDevice) {FollowingHero = true};

            _ecs = new ECS(_camera);
            _ecs.StartupRoutine(ecsBootRoutine.Presenting);
            _debugDrawer = new DebugDrawer(_ecs);
            _hero = _ecs.GetHero();

            _levelGenerator.ApplyPreset(LevelPreset.Forest);
        }

        public override Screen Update(GameTime gameTime)
        {
            UpdateGameTime(gameTime);

            _ecs.UpdateSystems(_internalGameTime);
            _camera.Update(_internalGameTime);

            if (_currentLevelTracker.ReadyForRestart) LoadContent();
            return this;
        }

        #region UpdateFunctions

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

        #endregion

        public override Screen Draw(GameTime gameTime)
        {
            _screenManager.SpriteBatch.Begin(SpriteSortMode.BackToFront, transformMatrix: _camera.View, samplerState: SamplerState.PointClamp);

            _ecs.Draw();
            if (Flags.Debug) _debugDrawer.Draw();

            _screenManager.SpriteBatch.End();
            if(Flags.Debug) DrawDebugText();
            return this;
        }

        public override Screen LoadContent()
        {
            LoadLevel();
            SoundEffect soundTrack = Assets.GetSound("ThemeSong");
            var instance = soundTrack.CreateInstance();
            instance.IsLooped = true;
            instance.Play();
            return this;
        }

        private void LoadLevel()
        {
            _gameTracker.Runs++;
            _levelGenerator.TotalEnemySpawnBudget = _levelGenerator.TotalEnemySpawnBudget + 50 * _gameTracker.Runs;
            _currentLevel = _levelGenerator.GenerateLevel();
            _currentLevelTracker = new LevelTracker();
            _ecs.PurgeForNextLevel();
            _ecs.ProcessLevel(_currentLevel, _currentLevelTracker);
            _hero = _ecs.GetHero();
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
            if (InputReader.IsKeyTriggered(Keys.F1)) ToggleAllDebug();
            if (InputReader.IsKeyTriggered(Keys.F2)) Flags.ShowDrawRecs = !Flags.ShowDrawRecs;
            if (InputReader.IsKeyTriggered(Keys.F3)) Flags.ShowCollisionRecs = !Flags.ShowCollisionRecs;
            if (InputReader.IsKeyTriggered(Keys.F4)) Flags.ShowDebuggingText = !Flags.ShowDebuggingText;
            if (InputReader.IsKeyTriggered(Keys.F5)) Flags.ShowMovementCenters = !Flags.ShowMovementCenters;
            if (InputReader.IsKeyPressed(Keys.F10)) Functions_Draw.Draw("Error", Assets.GetFont("Test"), new Vector2(100, 100));

            /* if (InputReader.IsKeyTriggered(Keys.N)) //TODO Create GenerationTesterScreen to test generation lmao
            {
                _levelGenerator.ApplySimpleCellularAutomata(_currentLevel);
                _ecs.PurgeForNextLevel();
                _ecs.ProcessLevel(_currentLevel, _currentLevelTracker);
            }
            */
            SimpleCameraMovement(gameTime);

            return this;
        }

        private void SimpleCameraMovement(GameTime gameTime)
        {
            if (InputReader.IsKeyTriggered(Keys.Space)) _camera.FollowingHero = !_camera.FollowingHero;
            if (!_camera.FollowingHero)
            {
                if (InputReader.IsKeyPressed(Keys.Up)) _camera.TargetPosition.Y -= 500 * (float)gameTime.ElapsedGameTime.TotalSeconds * 1 / _camera.CurrentZoom;
                if (InputReader.IsKeyPressed(Keys.Down)) _camera.TargetPosition.Y += 500 * (float)gameTime.ElapsedGameTime.TotalSeconds * 1 / _camera.CurrentZoom;
                if (InputReader.IsKeyPressed(Keys.Left)) _camera.TargetPosition.X -= 500 * (float)gameTime.ElapsedGameTime.TotalSeconds * 1 / _camera.CurrentZoom;
                if (InputReader.IsKeyPressed(Keys.Right)) _camera.TargetPosition.X += 500 * (float)gameTime.ElapsedGameTime.TotalSeconds * 1 / _camera.CurrentZoom;
            }
            else
            {
                if(_hero != null && _hero.Components.TryGetValue(typeof(DrawComponent), out var component))
                {
                    DrawComponent heroMovement = component as DrawComponent;
                    _camera.TargetPosition.X = heroMovement.DrawRec.Center.X;
                    _camera.TargetPosition.Y = heroMovement.DrawRec.Center.Y;
                }
            }



            if (InputReader.IsScrollingUp()) _camera.TargetZoom += 1;
            if (InputReader.IsScrollingDown()) _camera.TargetZoom -= 1;

            if (_camera.TargetZoom < 0.1) _camera.TargetZoom = 0.3f;
            else if (_camera.TargetZoom > 1) _camera.TargetZoom = (int)_camera.TargetZoom;
        }

        #endregion



        private void OpenMenuScreen()
        {
            _screenManager.AddScreen(new ScreenGameMenu(_screenManager, this));
        }

        public void Restart()
        {
            _gameTracker.Runs = 0;
            _ecs.PurgeForNextLevel();
            _levelGenerator.ApplyPreset(LevelPreset.Forest);
            LoadContent();
        }
    }
}
