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
            _spriteBatch.Draw(sprite.Texture, destinationRectangle: sprite.DrawRec, color: sprite.Tint * 1, layerDepth: sprite.LayerDepth - 0.000000001f * sprite.DrawRec.Y);
        }

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

        public static void Draw(String text, SpriteFont font, Vector2 location)
        { 
            _spriteBatch.DrawString(font, text, location, Color.Blue);
        }
        public static void Draw(String text, SpriteFont font, Vector2 location, Color color)
        {
            _spriteBatch.DrawString(font, text, location, color);
        }

    }
}
