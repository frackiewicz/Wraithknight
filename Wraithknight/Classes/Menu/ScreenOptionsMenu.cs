﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
<<<<<<< HEAD
using Microsoft.Xna.Framework.Audio;
=======
>>>>>>> 8645d874ee37e774e3b8a6cea3ac3e8ef156e2e0
using Microsoft.Xna.Framework.Graphics;

namespace Wraithknight
{
    class ScreenOptionsMenu : Screen
    {
        private List<MenuObject> _objects = new List<MenuObject>();

        private List<Button> _buttons = new List<Button>();
        private Viewport _viewport;

        public ScreenOptionsMenu(ScreenManager screenManager) : base(screenManager)
        {
            _viewport = screenManager.Graphics.GraphicsDevice.Viewport;
        }

        public override Screen LoadContent()
        {
<<<<<<< HEAD
            AABB aabb = new AABB(50, 50, 451, 75);
            Rectangle rectangle = new Rectangle(50, 50, 451, 75);

            _buttons.Add(new Button(new Vector2(_viewport.Width / 2, _viewport.Height / 2 - 100), new DrawComponent(Assets.GetTexture("vsync"), aabb), rectangle, "sync_button"));
            _buttons.Add(new Button(new Vector2(_viewport.Width / 2, _viewport.Height / 2), new DrawComponent(Assets.GetTexture("sound"), aabb), rectangle, "audio_button"));
            _buttons.Add(new Button(new Vector2(_viewport.Width / 2, _viewport.Height / 2 + 100), new DrawComponent(Assets.GetTexture("return"), aabb), rectangle, "return_button"));
=======
            _buttons.Add(new ButtonWithText(new Vector2(_viewport.Width / 2, _viewport.Height / 2), new DrawComponent(drawRec: new AABB(50, 50, 100, 20)), new Rectangle(50, 50, 100, 20), "sync_button", "Toggle V-Sync", Assets.GetFont("Test"), new Vector2(0, 0))); //TODO Replace Handles with enums?
            _buttons.Add(new ButtonWithText(new Vector2(_viewport.Width / 2, _viewport.Height / 2 + 25), new DrawComponent(drawRec: new AABB(50, 50, 100, 20)), new Rectangle(50, 50, 100, 20), "audio_button", "Toggle Audio", Assets.GetFont("Test"), new Vector2(0, 0)));
            _buttons.Add(new ButtonWithText(new Vector2(_viewport.Width / 2, _viewport.Height / 2 + 50), new DrawComponent(drawRec: new AABB(50, 50, 100, 20)), new Rectangle(50, 50, 100, 20), "return_button", "Return", Assets.GetFont("Test"), new Vector2(0, 0)));

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

<<<<<<< HEAD
                        if (button.ButtonHandle.Equals("audio_button"))
                        {
                            if (SoundEffect.MasterVolume == 1)
                            {
                                SoundEffect.MasterVolume = 0;
                            }
                            else
                            {
                                SoundEffect.MasterVolume = 1;
                            }
                        }
                        if (button.ButtonHandle.Equals("sync_button"))
                        {
                            _screenManager.Graphics.SynchronizeWithVerticalRetrace =
                                !_screenManager.Graphics.SynchronizeWithVerticalRetrace;
                        }
=======
                        if (button.ButtonHandle.Equals("audio_button")) Console.WriteLine("Toggled Audio");
                        if (button.ButtonHandle.Equals("sync_button")) Console.WriteLine("Toggled Vsync");
>>>>>>> 8645d874ee37e774e3b8a6cea3ac3e8ef156e2e0
                        if (button.ButtonHandle.Equals("return_button")) _screenManager.RemoveScreen(this);

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

            _screenManager.Graphics.GraphicsDevice.Clear(Color.Gray);

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
