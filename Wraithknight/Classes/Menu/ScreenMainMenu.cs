﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Wraithknight
{
    class ScreenMainMenu : Screen
    {
        private List<MenuObject> _objects = new List<MenuObject>();

        private List<Button> _buttons = new List<Button>();
        private Viewport _viewport;

        public ScreenMainMenu(ScreenManager screenManager) : base(screenManager)
        {
            _viewport = screenManager.Graphics.GraphicsDevice.Viewport;
        }

        public override Screen LoadContent()
        {
<<<<<<< HEAD
            AABB aabb = new AABB(50, 50, 451, 75);
            Rectangle rectangle = new Rectangle(50, 50, 451, 75);

            _buttons.Add(new Button(new Vector2(_viewport.Width / 2, _viewport.Height / 2 - 100), new DrawComponent(Assets.GetTexture("start"), aabb), rectangle, "start_game_button"));
            _buttons.Add(new Button(new Vector2(_viewport.Width / 2, _viewport.Height / 2), new DrawComponent(Assets.GetTexture("options"), aabb), rectangle, "options_button"));
            _buttons.Add(new Button(new Vector2(_viewport.Width / 2, _viewport.Height / 2 + 100), new DrawComponent(Assets.GetTexture("exit"), aabb), rectangle, "exit_button"));
=======
            _buttons.Add(new Button(new Vector2(_viewport.Width / 2, _viewport.Height / 2), new DrawComponent(Assets.GetTexture("start"), new AABB(50, 50, 451, 75)), new Rectangle(50, 50, 451, 75), "start_game_button"));
            _buttons.Add(new ButtonWithText(new Vector2(_viewport.Width / 2, _viewport.Height / 2 + 25), new DrawComponent(drawRec: new AABB(50, 50, 100, 20)), new Rectangle(50, 50, 100, 20), "options_button", "Options", Assets.GetFont("Test"), new Vector2(0, 0)));
            _buttons.Add(new ButtonWithText(new Vector2(_viewport.Width / 2, _viewport.Height / 2 + 50), new DrawComponent(drawRec: new AABB(50, 50, 100, 20)), new Rectangle(50, 50, 100, 20), "exit_button", "Exit", Assets.GetFont("Test"), new Vector2(0, 0)));
>>>>>>> 8645d874ee37e774e3b8a6cea3ac3e8ef156e2e0


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

                        if (button.ButtonHandle.Equals("start_game_button")) _screenManager.FlushAndLoad(new ScreenGame(_screenManager));
                        if (button.ButtonHandle.Equals("options_button")) _screenManager.AddScreen(new ScreenOptionsMenu(_screenManager));
                        if (button.ButtonHandle.Equals("exit_button")) Functions_GameControl.ExitGame();

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
            _screenManager.SpriteBatch.Draw(Assets.GetTexture("MenuBackground"), new Rectangle(0, 0, 1920, 1080), Color.White);
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
