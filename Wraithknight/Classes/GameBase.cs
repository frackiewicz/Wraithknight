﻿using System;
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
            /*
            Coord2 test = new Coord2(new Vector2(-1, 100));

            Console.WriteLine(test.Polar.Angle + "°");
            Console.WriteLine(test.Polar.Length);
            Console.WriteLine(test.Cartesian.X + " X");
            Console.WriteLine(test.Cartesian.Y + " Y");

            Console.WriteLine("----------------");
            Coord2 newVector = new Coord2(test.Polar);
            Console.WriteLine(newVector.Polar.Angle + "°");
            Console.WriteLine(newVector.Polar.Length);
            Console.WriteLine(newVector.Cartesian.X + " X");
            Console.WriteLine(newVector.Cartesian.Y + " Y");*/
            Coord2 test2 = new Coord2(new Vector2(2,2));
            Coord2 test3 = new Coord2(new Vector2(8,8));
            test2.AddPolar2(test3.Polar);
            Console.WriteLine(test2.Cartesian);

            InputReader.Initialize();
            base.Initialize();
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

        private void CheckForEmergencyExit()
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape)) Exit(); //emergency exit
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