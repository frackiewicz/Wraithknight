﻿using System;
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
        public Vector2Ref SourcePos;
        public Vector2 Offset; //the offset from the collision center
        public float Rotation;
        public Color Tint;
        public float LayerDepth;

        #region Constructors
        public DrawComponent(Texture2D texture = null, AABB? drawRec = null, Vector2Ref sourcePos = null, Point? size = null, Vector2? offset = null, float rotation = 0, Color? tint = null, float layerDepth = 0.1f) //TODO enum for layerdepth? USE CONSTANTS INSTEAD
        {
            Texture = texture ?? Assets.GetTexture("DummyTexture");
            DrawRec = drawRec ?? new AABB(0, 0, Texture.Width, Texture.Height);
            SourcePos = sourcePos;
            if (size != null) ChangeSize((Point)size);
            if (offset != null) Offset = (Vector2) offset;
            ApplyOffset();
            Rotation = rotation;
            if (tint == null) Tint = Color.White; else Tint = (Color) tint;
            LayerDepth = layerDepth;
        }
        #endregion

        public void ChangeSize(Point point)
        {
            DrawRec.Width = point.X;
            DrawRec.Height = point.Y;
        }

        public void ApplyOffset()
        {
            DrawRec.Center += Offset;
        }

    }
}
