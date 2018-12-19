using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace Wraithknight
{
    class Vector2Ref
    {
        public Vector2 Vector2;

        public float X
        {
            get => Vector2.X;
            set => Vector2.X = value;
        }
        public float Y
        {
            get => Vector2.Y;
            set => Vector2.Y = value;
        }

        public Vector2Ref(Vector2 vector)
        {
            Vector2 = vector;
        }

        public Vector2Ref(float x, float y)
        {
            Vector2 = new Vector2(x, y);
        }

        public Vector2Ref()
        {
            Vector2 = new Vector2();
        }

        public static implicit operator Vector2(Vector2Ref obj)
        {
            return obj.Vector2;
        }

        public static implicit operator Point(Vector2Ref obj)
        {
            return new Point((int)obj.X, (int)obj.Y);
        }

        public static Vector2Ref operator +(Vector2Ref value1, Vector2 value2)
        {
            value1.X += value2.X;
            value1.Y += value2.Y;
            return value1;
        }
        public static Vector2Ref operator +(Vector2Ref value1, Vector2Ref value2)
        {
            value1.X += value2.X;
            value1.Y += value2.Y;
            return value1;
        }

        public static Vector2Ref operator -(Vector2Ref value1, Vector2 value2)
        {
            value1.X += value2.X;
            value1.Y += value2.Y;
            return value1;
        }
        public static Vector2Ref operator -(Vector2Ref value1, Vector2Ref value2)
        {
            value1.X += value2.X;
            value1.Y += value2.Y;
            return value1;
        }
    }
}
