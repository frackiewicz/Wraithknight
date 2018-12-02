using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Wraithknight
{
    class DrawComponent : BindableComponent
    {
        public Texture2D Texture;
        public AABB DrawRec; //maybe make this an AABB?
        public Point Offset; //the offset from the collision center
        public float Rotation;
        public Color Tint;
        public float LayerDepth;

        #region Constructors
        public DrawComponent(Texture2D texture = null, AABB? drawRec = null, Point? size = null, Point? offset = null, float rotation = 0, Color? tint = null, float layerDepth = 0.1f) //TODO enum for layerdepth?
        {
            Texture = texture ?? Assets.GetTexture("DummyTexture");
            DrawRec = drawRec ?? new AABB(0, 0, Texture.Width, Texture.Height);
            if (size != null) ChangeSize((Point)size);
            if (offset != null) Offset = (Point) offset;
            ApplyOffset();
            Rotation = rotation;
            if (tint == null) Tint = Color.White; else Tint = (Color) tint;
            LayerDepth = layerDepth;
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


        public void ApplyOffset()
        {
            DrawRec.X += Offset.X;
            DrawRec.Y += Offset.Y;
        }

        public DrawComponent ChangeTint(Color tint)
        {
            Tint = tint;
            return this;
        }

        public override void Activate()
        {
            Inactive = false;
        }

        public override void Deactivate()
        {
            Inactive = true;
        }
    }
}
