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
        private readonly SoundManager _soundManager;

        public readonly Color ColorBackground = Color.LightGray;
        private readonly FpsCalculator _fpsCalculator;


        public GameBase()
        {
            #region Initialize Variables
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            _fpsCalculator = new FpsCalculator(0.5);

            _screenManager = new ScreenManager(this, _graphics);
            _soundManager = new SoundManager(this);
            #endregion

            #region SetSettings
            IsFixedTimeStep = false; //uncap fps
            TargetElapsedTime = TimeSpan.FromMilliseconds(1000.0f / 30);
            _graphics.SynchronizeWithVerticalRetrace = false;
            IsMouseVisible = true;
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
            InputReader.Update();
            _screenManager.Update(gameTime);
            base.Update(gameTime);
        }
        protected override void Draw(GameTime gameTime)
        {
            _screenManager.Draw(gameTime);
            UpdateFps(gameTime);
            base.Draw(gameTime);
            Functions_DebugWriter.WriteLine(InputReader.IsMouseButtonPressed(MouseButtons.LMB).ToString());
            Functions_DebugWriter.WriteLine(InputReader.IsMouseButtonPressed(MouseButtons.RMB).ToString());

        }

        private void CheckForEmergencyExit()
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape)) Exit(); //emergency exit
        }
        private void UpdateFps(GameTime gameTime)
        {
            _fpsCalculator.Update(gameTime);
            Window.Title = _fpsCalculator.Getfps() + " fps";
        }

        
    }
}
