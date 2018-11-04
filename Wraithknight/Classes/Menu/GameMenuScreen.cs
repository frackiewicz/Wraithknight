﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace Wraithknight
{
    class GameMenuScreen : Screen
    {
        private List<MenuObject> _objects = new List<MenuObject>();
        
        private List<Button> _buttons = new List<Button>(); //how do you put logic behind buttons? how to differentiate

        public GameMenuScreen(ScreenManager screenManager) : base(screenManager)
        {

        }

        public override Screen LoadContent()
        {
            _buttons.Add(new ButtonWithText(new DrawComponent(drawRec: new Rectangle(50, 50, 100, 20)), new Rectangle(50, 50, 100, 20), "exit", "Exit", Assets.GetFont("Test"), new Vector2(0,0)));
            _objects.AddRange(_buttons);
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
                        if(button.ButtonHandle.Equals("exit")) _screenManager.RemoveScreen(this);
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
                menuObject.Draw(); //maybe move this down into the objects as a Draw() method to allow more specific behavior
            }
            _screenManager.SpriteBatch.End();
            return this;
        }
    }
}
