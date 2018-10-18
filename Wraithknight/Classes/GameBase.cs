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
        //Assetmanager?

        public readonly Color ColorBackground = Color.LightGray;

        private Texture2D _dummyTexture;
        private Vector2 _testpos = new Vector2(0,0);

        private readonly FpsCalculator _fpsCalculator;


        public GameBase()
        {
            #region Initialize Variables
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            _fpsCalculator = new FpsCalculator(0.5);

            _screenManager = new ScreenManager(this);
            _soundManager = new SoundManager(this);
            #endregion

            #region SetSettings
            IsFixedTimeStep = false; //uncap fps
            _graphics.SynchronizeWithVerticalRetrace = false;
            IsMouseVisible = true;
            #endregion
        }


        protected override void Initialize()
        {
            
            base.Initialize();
        }

        protected override void LoadContent()
        {
            Assets.Initialize(GraphicsDevice, Content);
            _screenManager.Initialize();
            _dummyTexture = new Texture2D(GraphicsDevice, 1, 1);
           
        }

        protected override void UnloadContent()
        {
            
          
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape)) Exit(); //emergency exit
            
            _screenManager.Update(gameTime);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            _screenManager.Draw(gameTime);

            UpdateFps(gameTime);
            
            base.Draw(gameTime);
        }

        private void UpdateFps(GameTime gameTime)
        {
            _fpsCalculator.Update(gameTime);
            Window.Title = _fpsCalculator.Getfps() + " fps";
        }

    }
}
