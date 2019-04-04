using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;

namespace Wraithknight
{
    public class ScreenManager
    {   //manages screens - loads, updates, passes input, draws, and removes screens
        private readonly GameBase _game;
        public readonly GraphicsDeviceManager Graphics;

        private readonly List<Screen> _screens = new List<Screen>();

        public SpriteBatch SpriteBatch { get; private set; }

        public ScreenManager(GameBase game, GraphicsDeviceManager graphics) 
        {
            _game = game;
            Graphics = graphics;
        }

        public void Initialize()
        {
            SpriteBatch = new SpriteBatch(_game.GraphicsDevice);
            Functions_Draw.setSpriteBatch(SpriteBatch);

            #region Bootroutine
            if (Flags.BootRoutine == BootRoutine.Game)
            {
                FlushAndLoad(new ScreenMainMenu(this));
            }
            else if (Flags.BootRoutine == BootRoutine.TestingRoom)
            {
                FlushAndLoad(new ScreenGame(this));
            }
            else if (Flags.BootRoutine == BootRoutine.Generation)
            {
                FlushAndLoad(new ScreenGenerationTester(this));
            }
            #endregion
        }

        public void UnloadContent() { foreach (Screen screen in _screens) { screen.UnloadContent(); } } // use?


        #region ScreenControl
        public void AddScreen(Screen screenToAdd)
        {
            screenToAdd.LoadContent();
            _screens.Add(screenToAdd);
        }

        public void RemoveScreen(Screen screenToRemove)
        {
            screenToRemove.UnloadContent();
            _screens.Remove(screenToRemove);
        }

        public void UnloadScreen(Screen screenToUnload)
        {
            screenToUnload.UnloadContent();
        }
        public Screen[] GetScreens() { return _screens.ToArray(); }

        public void FlushAndLoad(Screen screenToLoad)
        {
            foreach (Screen screen in _screens)
            {
                UnloadScreen(screen);
            }
            _screens.Clear();

            AddScreen(screenToLoad);
        }
        #endregion


        public void Update(GameTime gameTime)
        {
            UpdateHighestScreen(gameTime);
        }

        private void UpdateHighestScreen(GameTime gameTime)
        {
            if (_screens.Count > 0)
            {
                _screens[_screens.Count - 1].HandleInput(gameTime).Update(gameTime);
            }
        }

        public void Draw(GameTime gameTime)
        {
            _game.GraphicsDevice.Clear(_game.ColorBackground);
            foreach (Screen screen in _screens) { if(screen.IsVisible) screen.Draw(gameTime); }
        }
    }
}