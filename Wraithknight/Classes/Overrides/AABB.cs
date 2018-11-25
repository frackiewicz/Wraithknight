using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace Wraithknight
{
    internal struct AABB //float alternative to Rectangle
    {
        public Vector2 Center;
        public Vector2 Extents; //Width+Height

        public float X
        {
            get => Center.X - Extents.X;
            set => Center.X = value + Extents.X;
        }
        public float Y
        {
            get => Center.Y - Extents.Y;
            set => Center.Y = value + Extents.Y;
        }

        public float Width //int recommended
        {
            get => Extents.X * 2;
            set => Extents.X = value / 2;
        }  
        public float Height //int recommended
        {
            get => Extents.Y * 2;
            set => Extents.Y = value / 2;
        }
        public float Left => X;
        public float Right => Center.X + Extents.X;
        public float Top => Y;
        public float Bottom => Center.Y + Extents.Y;


        public AABB(Vector2 center, Vector2 extents)
        {
            Center = center;
            Extents = extents;
        }

        //uses topleft as spawn
        public AABB(float x, float y, float width, float height)
        {
            Center = new Vector2(x + width/2, y + height/2);
            Extents = new Vector2(width/2, height/2);
        }

        public AABB(int x, int y, int width, int height)
        {
            Center = new Vector2(x + width/2, y + height/2);
            Extents = new Vector2(width/2, height/2);
        }

        public static AABB operator +(AABB box, Vector2 vector)
        {
            box.Center += vector;
            return box;
        }
        public static AABB operator -(AABB box, Vector2 vector)
        {
            box.Center -= vector;
            return box;
        }

        public void SetCenter(Vector2 center) //TODO Breunig possible binding to movementcomponent?
        {
            Center.X = center.X;
            Center.Y = center.Y;
        }

        public bool Intersects(AABB value)
        {
            return value.Left < Right && Left < value.Right && value.Top < Bottom && Top < value.Bottom;
        }

        public bool Contains(Point point)
        {
            return point.X < Right && point.X > Left && point.Y < Bottom && point.Y > Top;
        }

        public bool Contains(Vector2 point)
        {
            return point.X < Right && point.X > Left && point.Y < Bottom && point.Y > Top;
        }
    }
}
