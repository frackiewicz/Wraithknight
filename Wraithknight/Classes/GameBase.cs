using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
#pragma warning disable 618

namespace Wraithknight
{
    public class GameBase : Game
    {
        private readonly GraphicsDeviceManager _graphics;
        private readonly ScreenManager _screenManager;

        public readonly Color ColorBackground = Color.LightGray;
        public GameTime GameTime;
        private readonly FpsCalculator _fpsCalculator;


        public GameBase()
        {
            #region Initialize Variables
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            _fpsCalculator = new FpsCalculator(0.5);

            _screenManager = new ScreenManager(this, _graphics);
            #endregion

            #region SetSettings
            IsFixedTimeStep = false; //uncap fps
            TargetElapsedTime = TimeSpan.FromMilliseconds(1000.0f / 30);
            //_graphics.SynchronizeWithVerticalRetrace = false;
            IsMouseVisible = true;
            //_graphics.IsFullScreen = true;
            _graphics.PreferredBackBufferHeight = 1080;
            _graphics.PreferredBackBufferWidth = 1920;

            #endregion
        }

        protected override void Initialize()
        {
            ConsoleTesting();
            Window.AllowUserResizing = true;
            InputReader.Initialize();
            Functions_GameControl.ConnectGameBase(this);
            base.Initialize();
        }
        private void ConsoleTesting()
        {

        }
        protected override void LoadContent()
        {
            Assets.Initialize(GraphicsDevice, Content);
            _screenManager.Initialize();
           
        }
        protected override void UnloadContent()
        {
            
          
        }

        protected override void Update(GameTime gameTime)
        {
            Flags.FpsBelowThreshold = gameTime.ElapsedGameTime.Milliseconds > 1000.0f / 30;

            InputReader.Update();
            _screenManager.Update(gameTime);
            base.Update(gameTime);
        }
        protected override void Draw(GameTime gameTime)
        {
            _screenManager.Draw(gameTime);
            UpdateFps(gameTime);
            base.Draw(gameTime);
        }

        private void UpdateFps(GameTime gameTime) //TODO Create Char-Array and create a string from that to avoid concat
        {
            _fpsCalculator.Update(gameTime);
            Window.Title = _fpsCalculator.Getfps() + " fps";
        }

        
    }
}
