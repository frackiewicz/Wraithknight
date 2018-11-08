using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Wraithknight
{
    class MenuObject
    {
        public enum Origin
        {
            TopLeft,
            Center
        }
        public Vector2 Position;

        public DrawComponent DrawComponent;

        public readonly Origin DrawOrigin;

        public MenuObject(Vector2 position, DrawComponent drawComponent, Origin drawOrigin = Origin.Center)
        {
            Position = position;
            DrawComponent = drawComponent;
            DrawOrigin = drawOrigin;
        }

        public virtual void Draw()
        {
            Functions_Draw.Draw(DrawComponent);
        }

        public virtual void Align(Viewport viewport)
        {
            if (DrawOrigin == Origin.Center)
            {
                DrawComponent.DrawRec.X = (int) Position.X - DrawComponent.DrawRec.Width / 2;
                DrawComponent.DrawRec.Y = (int)Position.Y - DrawComponent.DrawRec.Height / 2;
            }
        }
    }
}
