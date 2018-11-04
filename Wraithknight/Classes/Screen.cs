using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace Wraithknight
{
    public abstract class Screen
    {
        public enum DisplayState
        {
            Opening,
            Open,
            Closing,
            Closed
        }

        protected ScreenManager _screenManager;
        public bool IsVisible = true; //idea: main menu remains hidden in the background to prevent unnecessary loading

        public Screen(ScreenManager screenManager)
        {
            _screenManager = screenManager;
        }

        public abstract Screen LoadContent();
        public abstract Screen UnloadContent();
        public abstract Screen HandleInput(GameTime gameTime);
        public abstract Screen Update(GameTime gameTime);
        public abstract Screen Draw(GameTime gameTime);
    }
}
