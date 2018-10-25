using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace Wraithknight //TODO structs could use some improvements
{
    public struct Momentum
    {
        public PolarCoord Polar;
        public Vector2 Cartesian;

        public Momentum (PolarCoord polar)
        {
            Polar = polar;
            Cartesian = new Vector2((float) (polar.Length * Math.Cos(Functions_Math.DegreeToRadian(polar.Angle))), (float) (polar.Length * Math.Sin(Functions_Math.DegreeToRadian(polar.Angle))));
        }

        public Momentum (Vector2 cartesian)
        {
            Polar = new PolarCoord(cartesian);
            Cartesian = cartesian;
        }
    }

    public struct PolarCoord
    {
        public float Length;
        public float Angle;

        public PolarCoord(float length, float angle)
        {
            Length = length;
            Angle = angle;
        }

        public PolarCoord(Vector2 vector)
        {
            Length = (float) Math.Sqrt(Math.Pow(vector.X, 2) + Math.Pow(vector.Y, 2));
            Angle = Functions_Math.RadianToDegree((float) Math.Atan2(vector.X, vector.Y));
        }
    }

    public class MovementComponent : Component // TODO Implement movement with angles
    {
        public Direction Direction = Direction.Down; //havent figured out the use yet
        public Boolean Moving = false;

        public Vector2 Position = new Vector2(0, 0);
        public Vector2 Speed = new Vector2(0, 0);
        public Vector2 Acceleration = new Vector2(0, 0);

        public float AccelerationBase = 0;
        public float MaxSpeed = 0.0f;
        public float Friction = 0.0f; //inertia

        public MovementComponent ChangeAccelerationBase(float value)
        {
            AccelerationBase = value;
            return this;
        }

        public MovementComponent ChangeMaxSpeed(float value)
        {
            MaxSpeed = value;
            return this;
        }

        public MovementComponent ChangeFriction(float value)
        {
            Friction = value;
            return this;
        }

    }
}
