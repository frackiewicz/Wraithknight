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
        private Camera2D _camera; // Maybe insert variables into the camera instance TODO cleanup
        private Rectangle _cameraRectangle;
        private Rectangle _cullRectangle;
        private Point _cullBound;


        public GameScreen(ScreenManager screenManager) : base(screenManager)
        {

        }

        public override Screen LoadContent()
        {
            throw new NotImplementedException();
        }

        public override Screen UnloadContent()
        {
            throw new NotImplementedException();
        }

        public override Screen HandleInput(GameTime gameTime)
        {
            throw new NotImplementedException();
        }

        public override Screen Update(GameTime gameTime)
        {
            throw new NotImplementedException();
        }

        public override Screen Draw(GameTime gameTime)
        {

            return this;
        }


        //TODO Debug mode



    }
}
