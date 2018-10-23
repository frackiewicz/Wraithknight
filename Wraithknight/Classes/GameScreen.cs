using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Activation;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace Wraithknight
{
    public class GameScreen : Screen
    {
        private readonly ScreenManager _screenManager;

        private Camera2D _camera; // Maybe insert variables into the camera instance TODO cleanup
        private Rectangle _cameraRectangle;
        private Rectangle _cullRectangle;
        private Point _cullBound;


        public GameScreen(ScreenManager screenManager)
        {

        }

        public override Screen Draw(GameTime gameTime)
        {

            return this;
        }


        //TODO Debug mode



    }
}
