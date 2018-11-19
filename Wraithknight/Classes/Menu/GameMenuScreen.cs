using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Wraithknight
{
    class GameMenuScreen : Screen
    {
        private List<MenuObject> _objects = new List<MenuObject>();
        
        private List<Button> _buttons = new List<Button>(); //how do you put logic behind buttons? how to differentiate
        private Viewport _viewport;

        public GameMenuScreen(ScreenManager screenManager) : base(screenManager)
        {
            _viewport = screenManager.Graphics.GraphicsDevice.Viewport;
        }

        public override Screen LoadContent()
        {
            _buttons.Add(new ButtonWithText(new Vector2(_viewport.Width / 2, _viewport.Height / 2), new DrawComponent(drawRec: new Rectangle(50, 50, 100, 20)), new Rectangle(50, 50, 100, 20), "return_button", "Return", Assets.GetFont("Test"), new Vector2(0,0)));
            _buttons.Add(new ButtonWithText(new Vector2(_viewport.Width / 2, _viewport.Height / 2 - 100), new DrawComponent(drawRec: new Rectangle(50, 50, 100, 20)), new Rectangle(50, 50, 100, 20), "exit_button", "Exit", Assets.GetFont("Test"), new Vector2(0, 0)));

            _objects.AddRange(_buttons);
            AlignObjects();
            return this;
        }

        public override Screen UnloadContent()
        {
            return this;
        }

        public override Screen HandleInput(GameTime gameTime)
        {
            if (InputReader.IsMouseButtonTriggered(MouseButtons.LMB))
            {
                foreach (var button in _buttons)
                {
                    if (button.HandleMouseClick())
                    {
                        if (button.ButtonHandle.Equals("return_button")) _screenManager.RemoveScreen(this);
                        if (button.ButtonHandle.Equals("exit_button")) _screenManager.RemoveScreen(this);
                    }
                }
            }

            return this;
        }

        public override Screen Update(GameTime gameTime)
        {
            return this;
        }

        public override Screen Draw(GameTime gameTime)
        {
            _screenManager.SpriteBatch.Begin();
            foreach (var menuObject in _objects)
            {
                menuObject.Align(_viewport); //TODO optimize when this gets updated, maybe only on res change
                menuObject.Draw(); //maybe move this down into the objects as a Draw() method to allow more specific behavior
            }
            _screenManager.SpriteBatch.End();
            return this;
        }

        private void AlignObjects()
        {
            foreach (var menuObject in _objects)
            {
                menuObject.Align(_viewport);
            }
        }
    }
}
