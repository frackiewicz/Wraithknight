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
    public static class Functions_Draw
    {
        private static SpriteBatch _spriteBatch;


        public static void setSpriteBatch(SpriteBatch spriteBatch)
        {
            if(_spriteBatch == null) _spriteBatch = spriteBatch;
        }

        public static void clearSpriteBatch() //use unknown
        {
            _spriteBatch = null;
        }


        public static void Draw(Actor actor)
        {
            Draw(actor.Sprite);
            if (Flags.ShowDrawRecs)
            {
                _spriteBatch.Draw(Assets.GetTexture("DummyTexture"), actor.Sprite.DrawRec, new Color(255, 255, 255, 50));
            }

            if (Flags.ShowCollisionRecs)
            {
                _spriteBatch.Draw(Assets.GetTexture("DummyTexture"), actor.Collision.CollisionRec, new Color(255, 0, 0, 50));
                _spriteBatch.Draw(Assets.GetTexture("DummyTexture"), actor.Collision.CollisionCenter, new Color(0, 0, 0, 100));
            }
        }

        public static void Draw(DrawComponent sprite)
        {
            _spriteBatch.Draw(sprite.Texture, sprite.DrawRec, Color.White * 1);
        }


    }
}
