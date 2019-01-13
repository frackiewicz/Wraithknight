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
        public bool IsAnimated;
        public AABB DrawRec;
        public Vector2Ref BoundPos;

        public Vector2 Scale;
        public Rectangle SourceRec;
        public Vector2 Offset; //the offset from the collision center

        public float Rotation;
        public Color Tint;
        public float LayerDepth;
        public bool FlipHorizontally;

        #region Constructors
        public DrawComponent(Texture2D texture = null, AABB? drawRec = null, Rectangle? sourceRec = null, Vector2Ref boundPos = null, Vector2? scale = null, Vector2? offset = null, float rotation = 0, Color? tint = null, float layerDepth = 0.1f) //TODO enum for layerdepth? USE CONSTANTS INSTEAD
        {
            Texture = texture ?? Assets.GetTexture("DummyTexture");
            DrawRec = drawRec ?? new AABB(0, 0, Texture.Width, Texture.Height);
            SourceRec = sourceRec ?? new Rectangle(0, 0, Texture.Width, Texture.Height);
            BoundPos = boundPos;
            Scale = scale ?? new Vector2(1,1);
            if (offset != null) Offset = (Vector2) offset;
            ApplyOffset();
            Rotation = rotation;
            if (tint == null) Tint = Color.White; else Tint = (Color) tint;
            LayerDepth = layerDepth;
        }
        #endregion

        public void ApplyOffset()
        {
            DrawRec.Center += Offset;
        }

    }
}
