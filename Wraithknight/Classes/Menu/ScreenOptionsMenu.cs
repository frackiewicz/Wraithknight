using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
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
            AABB aabb = new AABB(50, 50, 451, 75);
            Rectangle rectangle = new Rectangle(50, 50, 451, 75);

            _buttons.Add(new Button(new Vector2(_viewport.Width / 2, _viewport.Height / 2 - 100), new DrawComponent(Assets.GetTexture("vsync"), aabb), rectangle, "sync_button"));
            _buttons.Add(new Button(new Vector2(_viewport.Width / 2, _viewport.Height / 2), new DrawComponent(Assets.GetTexture("sound"), aabb), rectangle, "audio_button"));
            _buttons.Add(new Button(new Vector2(_viewport.Width / 2, _viewport.Height / 2 + 100), new DrawComponent(Assets.GetTexture("return"), aabb), rectangle, "return_button"));

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
