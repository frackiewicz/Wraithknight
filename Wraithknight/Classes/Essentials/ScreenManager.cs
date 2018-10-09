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
        private GameTime _gameTime;

        private readonly List<Screen> _screens = new List<Screen>();

        public SpriteBatch SpriteBatch { get; private set; }
        public readonly InputReader Input; //pass input to each screen
        internal readonly ECS ecsEnvironment; //Maybe move further down? TODO Ask Breunig for possible improvements

        public ScreenManager(GameBase game)
        {
            _game = game;
            Input = new InputReader();
            ecsEnvironment = new ECS();
        }

        public void Initialize()
        {
            SpriteBatch = new SpriteBatch(_game.GraphicsDevice);
            Functions_Draw.setSpriteBatch(SpriteBatch);

            #region Bootroutine
            if (Flags.BootRoutine == BootRoutine.Game)
            {
                //run game title
            }
            else if (Flags.BootRoutine == BootRoutine.TestingRoom)
            {
                FlushAndLoad(new ScreenTestingRoom(this));
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
        public Screen[] GetScreens() { return _screens.ToArray(); }

        public void FlushAndLoad(Screen screenToLoad)
        {
            foreach (Screen screen in _screens)
            {
                RemoveScreen(screen);
            }
            
            AddScreen(screenToLoad);
        }
        #endregion


        public void Update(GameTime gameTime)
        {
            _gameTime = gameTime;
            Input.Update();
            if (_screens.Count > 0)
            {
                Screen activeScreen = _screens[_screens.Count - 1]; //TODO remove temp var? change return type to Screen + chaining
                activeScreen.HandleInput(_gameTime);
                activeScreen.Update(_gameTime);
            }
        }

        public void Draw(GameTime gameTime)
        {
            _game.GraphicsDevice.Clear(_game.ColorBackground);
            foreach (Screen screen in _screens) { screen.Draw(gameTime); }
        }
    }
}