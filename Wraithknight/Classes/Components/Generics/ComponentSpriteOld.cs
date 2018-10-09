using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Wraithknight
{
    public class ComponentSpriteOld
    {
        public Texture2D Texture;
        public Rectangle DrawRec;
        public Point Offset; //the offset from the collision center


        public void changeSize(int x, int y)
        {
            DrawRec.Width = x;
            DrawRec.Height = y;
        }

        #region Constructors
        public ComponentSpriteOld()
        {
            Texture = Assets.GetTexture("DummyTexture");
            changeSize(16,16);
            Texture.SetData(new Color[] { Color.Purple });
            automaticOffset();
        }
        public ComponentSpriteOld(Texture2D texture)
        {
            Texture = texture;
            DrawRec = new Rectangle(0, 0, texture.Width, texture.Height);
            automaticOffset();
        }
        public ComponentSpriteOld(Texture2D texture, Point offset)
        {
            Texture = texture;
            DrawRec = new Rectangle(0, 0, texture.Width, texture.Height);
            Offset = offset;
        }
        #endregion

        private void automaticOffset() // ??? 
        {
            Offset.X = -(int) Texture.Width / 2;
            Offset.Y = -(int) Texture.Height / 2;
        }
    }
}
