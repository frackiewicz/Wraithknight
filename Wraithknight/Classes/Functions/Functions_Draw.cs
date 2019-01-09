using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

#pragma warning disable CS0618 // Type or member is obsolete

namespace Wraithknight
{
    //Stateless Renderer
    //Used for all drawing in the game, static is appropiate here
    static class Functions_Draw
    {
        private static SpriteBatch _spriteBatch;
        private static Texture2D _dummyTexture = Assets.GetTexture("DummyTexture");

        #region GC helpers

        private static Vector2 _Vector2 = new Vector2();
        private static Vector2 MakeVector2(float x, float y)
        {
            _Vector2.X = x;
            _Vector2.Y = y;
            return _Vector2;
        }
        #endregion

        public static void setSpriteBatch(SpriteBatch spriteBatch)
        {
            if(_spriteBatch == null) _spriteBatch = spriteBatch;
        }

        public static void clearSpriteBatch() //use unknown
        {
            _spriteBatch = null;
        }

        public static void Draw(DrawComponent sprite)
        {
            _spriteBatch.Draw(sprite.Texture, position: MakeVector2(sprite.DrawRec.X, sprite.DrawRec.Y), scale: MakeVector2(sprite.DrawRec.Width / sprite.Texture.Width, sprite.DrawRec.Height / sprite.Texture.Height), color: sprite.Tint * 1, layerDepth: sprite.LayerDepth - 0.000000001f * sprite.DrawRec.Y);
        }

        #region Debug

        public static void DrawDebug(Rectangle rectangle)
        {
            _spriteBatch.Draw(_dummyTexture, rectangle, Color.Coral);
        }

        public static void DrawDebug(AABB rectangle)
        {
            _spriteBatch.Draw(_dummyTexture, new Rectangle((int)rectangle.X, (int)rectangle.Y, (int)rectangle.Width, (int)rectangle.Height), Color.Coral);
        }

        public static void DrawDebug(Vector2 point)
        {
            _spriteBatch.Draw(_dummyTexture, point, Color.Black);
        }
        public static void DrawDebug(Vector2 point, Color color)
        {
            _spriteBatch.Draw(_dummyTexture, point, color);
        }

        public static void Draw(String text, SpriteFont font, Vector2 location)
        {
            _spriteBatch.DrawString(font, text, location, Color.Blue);
        }
        public static void Draw(String text, SpriteFont font, Vector2 location, Color color)
        {
            _spriteBatch.DrawString(font, text, location, color);
        }

        #endregion


    }
}
