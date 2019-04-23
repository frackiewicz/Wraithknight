using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Wraithknight
{
    class ScreenGameMenu : Screen
    {
        private List<MenuObject> _objects = new List<MenuObject>();
        
        private List<Button> _buttons = new List<Button>(); //how do you put logic behind buttons? how to differentiate
        private Viewport _viewport;
        private ScreenGame _screenGame;
        private ScreenGenerationTester _screenGenerationTester;

        public ScreenGameMenu(ScreenManager screenManager, ScreenGame screenGame = null, ScreenGenerationTester screenGenerationTester = null) : base(screenManager)
        {
            _viewport = screenManager.Graphics.GraphicsDevice.Viewport;
            _screenGame = screenGame;
            _screenGenerationTester = screenGenerationTester;
        }

        public override Screen LoadContent()
        {
            AABB aabb = new AABB(50, 50, 451, 75);
            Rectangle rectangle = new Rectangle(50, 50, 451, 75);

            _buttons.Add(new Button(new Vector2(_viewport.Width / 2, _viewport.Height / 2 - 100), new DrawComponent(Assets.GetTexture("return"), aabb), rectangle, "return_button"));
            _buttons.Add(new Button(new Vector2(_viewport.Width / 2, _viewport.Height / 2), new DrawComponent(Assets.GetTexture("restart"), aabb), rectangle, "restart_button"));
            _buttons.Add(new Button(new Vector2(_viewport.Width / 2, _viewport.Height / 2 + 100), new DrawComponent(Assets.GetTexture("options"), aabb), rectangle, "options_button"));
            _buttons.Add(new Button(new Vector2(_viewport.Width / 2, _viewport.Height / 2 + 200), new DrawComponent(Assets.GetTexture("exit"), aabb), rectangle, "exit_button"));
           

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
                        if (button.ButtonHandle.Equals("exit_button")) Functions_GameControl.ExitGame();
                        if (button.ButtonHandle.Equals("options_button")) _screenManager.AddScreen(new ScreenOptionsMenu(_screenManager));
                        if (button.ButtonHandle.Equals("restart_button"))
                        {
                            _screenGame?.Restart();
                            _screenGenerationTester?.LoadContent();
                            _screenManager.RemoveScreen(this);
                        }

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
