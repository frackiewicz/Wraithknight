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
        public float Rotation;
        public Color Tint;

        // TODO REMOVE EVERYTHING | are you sure?
        #region Constructors
        public DrawComponent(Texture2D texture = null, Rectangle? drawRec = null, Point? size = null, Point? offset = null, float rotation = 0, Color? tint = null)
        {
            Texture = texture ?? Assets.GetTexture("DummyTexture");
            DrawRec = drawRec ?? new Rectangle(Point.Zero, new Point(Texture.Width, Texture.Height));
            if (size == null) ChangeSize(16,16); else ChangeSize((Point)size);
            if (offset == null) AutomaticOffset(); else Offset = (Point) offset;
            Rotation = rotation;
            if (tint == null) Tint = Color.White; else Tint = (Color) tint;
            //Texture.SetData(new Color[] { Color.Purple });
        }
        #endregion


        public void ChangeSize(int width, int height)
        {
            DrawRec.Width = width;
            DrawRec.Height = height;
        }

        public void ChangeSize(Point point)
        {
            DrawRec.Width = point.X;
            DrawRec.Height = point.Y;
        }

        public DrawComponent AutomaticOffset()
        {
            Offset.X = -(int) Texture.Width / 2;
            Offset.Y = -(int) Texture.Height / 2;
            return this;
        }

        public DrawComponent ChangeTint(Color tint)
        {
            Tint = tint;
            return this;
        }

        public override void Activate()
        {
            Active = true;
        }

        public override void Deactivate()
        {
            Active = false;
        }
    }
}
