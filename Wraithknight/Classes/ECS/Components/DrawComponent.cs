using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Wraithknight
{
    public class DrawComponent : Component
    {
        public Texture2D Texture;
        public Rectangle DrawRec;
        public Point Offset; //the offset from the collision center

        // TODO REMOVE EVERYTHING | are you sure?
        #region Constructors
        public DrawComponent() 
        {
            Texture = Assets.GetTexture("DummyTexture");
            ChangeSize(16,16);
            Texture.SetData(new Color[] { Color.Purple });
            automaticOffset();
        }

        public DrawComponent(Texture2D texture)
        {
            Texture = texture;
            DrawRec = new Rectangle(0, 0, texture.Width, texture.Height);
            automaticOffset();
        }

        public DrawComponent(Texture2D texture, Point offset)
        {
            Texture = texture;
            DrawRec = new Rectangle(0, 0, texture.Width, texture.Height);
            Offset = offset;
        }
        #endregion


        public void ChangeSize(int width, int height) //LOGIC? Does it break ecsEnvironment? TODO Ask Breunig
        {
            DrawRec.Width = width;
            DrawRec.Height = height;
        }

        private void automaticOffset() // ??? 
        {
            Offset.X = -(int) Texture.Width / 2;
            Offset.Y = -(int) Texture.Height / 2;
        }

        public override void Activate()
        {
            active = true;
        }

        public override void Deactivate()
        {
            active = false;
        }
    }
}
