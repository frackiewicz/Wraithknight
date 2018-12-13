using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace Wraithknight //TODO structs could use some improvements
//TODO BREUNIG
{ //You got some calculation mistakes here
    //https://www.codeproject.com/Articles/8052/Type-casting-impact-over-execution-performance-in
    public class Coord2
    { //TODO change degrees to radian (might solve the rounding issue)
        public Polar2 Polar;
        public Vector2 Cartesian;

        #region Constructors
        public Coord2()
        {
            Polar = new Polar2(0, 0);
            Cartesian = new Vector2(0, 0);
        }

        public Coord2 (Polar2 polar)
        {
            Polar = polar;
            Cartesian = CartesianFromPolar(polar);
        }

        public Coord2 (Vector2 cartesian)
        {
            Polar = PolarFromCartesian(cartesian);
            Cartesian = cartesian;
        }
        #endregion

        public Coord2 ChangePolarLength(float newLength)
        {
            Polar.Length = newLength;
            ChangeCartesianFromPolar(Polar);
            AttemptToRoundCartesian();
            return this;
        }

        public Coord2 ChangePolarLength(double newLength)
        {
            Polar.Length = newLength;
            ChangeCartesianFromPolar(Polar);
            AttemptToRoundCartesian();
            return this;
        }

        public Coord2 AddVector2(Vector2 vector)
        {
            Cartesian += vector;
            ChangePolarFromCartesian(Cartesian);
            AttemptToRoundCartesian();
            return this;
        }

        public Coord2 SetVector2(Vector2 vector)
        {
            Cartesian.X = vector.X;
            Cartesian.Y = vector.Y;
            ChangePolarFromCartesian(Cartesian);
            AttemptToRoundCartesian();
            return this;
        }

        public Coord2 AddPolar2(Polar2 polar)
        {
            ChangePolarFromCartesian(Polar.InCartesian() + polar.InCartesian());
            ChangeCartesianFromPolar(Polar);
            AttemptToRoundCartesian();
            return this;
        }

        public Coord2 ChangeX(float newX)
        {
            Cartesian.X = newX;
            ChangePolarFromCartesian(Cartesian);
            AttemptToRoundCartesian();
            return this;
        }

        public Coord2 ChangeY(float newY)
        {
            Cartesian.Y = newY;
            ChangePolarFromCartesian(Cartesian);
            AttemptToRoundCartesian();
            return this;
        }

        #region Conversions
        public static Vector2 CartesianFromPolar(Polar2 polar)
        {
            return new Vector2((float) (polar.Length * Math.Cos(polar.Angle)), (float) (polar.Length * Math.Sin(polar.Angle)));
        }

        public static Polar2 PolarFromCartesian(Vector2 cartesian)
        {
            return new Polar2(cartesian);
        }

        private void ChangeCartesianFromPolar(Polar2 polar) //Make public?
        {
            Cartesian.X = (float)(polar.Length * Math.Cos(polar.Angle));
            Cartesian.Y = (float)(polar.Length * Math.Sin(polar.Angle));
        }

        private void ChangePolarFromCartesian(Vector2 cartesian)
        {
            Polar.ChangeLengthFromCartesian(cartesian);
            Polar.ChangeAngleFromCartesian(cartesian);
        }
        #endregion

        private void AttemptToRoundCartesian() //for pretties :)
        {
            double n = Math.Round(Cartesian.X);
            if (Math.Abs(Cartesian.X - n) < 0.001) Cartesian.X = (float)n;

            n = Math.Round(Cartesian.Y);
            if (Math.Abs(Cartesian.Y - n) < 0.001) Cartesian.Y = (float)n;
        }
    }

    public class Polar2
    {
        public double Length;
        public double Angle;

        public Polar2(Polar2 polar)
        {
            Length = polar.Length;
            Angle = polar.Angle;
        }

        public Polar2(float length, float angle)
        {
            Length = length;
            Angle = angle;
        }

        public Polar2(Vector2 cartesian)
        {
            ChangeLengthFromCartesian(cartesian);
            ChangeAngleFromCartesian(cartesian);
        }

        public void ChangeLengthFromCartesian(Vector2 cartesian)
        {
            Length = (float)Math.Sqrt(Math.Pow(cartesian.Y, 2) + Math.Pow(cartesian.X, 2));
        }

        public void ChangeAngleFromCartesian(Vector2 cartesian) //TODO angle should be given in radian instead
        {
            Angle = Math.Atan2(cartesian.Y, cartesian.X);
        }

        public Vector2 InCartesian() 
        {
            return Coord2.CartesianFromPolar(this);
        }
    }

    class MovementComponent : Component
    {
        public Direction Direction => GetDirection();
        public Boolean IsMoving = false;
        public Boolean HasCollided = false;

        public Vector2Ref Position; //maybe make a wrapper class so you can pass the reference for alignment
        public Vector2 OldPosition;

        public Coord2 Speed;
        public Vector2 Acceleration;

        public float AccelerationBase = 0;
        public float MaxSpeed = 0.0f;
        public float Friction = 0.0f;
        public bool FrictionWhileAccelerating;


        public MovementComponent(Vector2Ref position, Coord2 speed = null, Vector2 acceleration = new Vector2(), float accelerationBase = 0.0f, float maxSpeed = 0.0f, float friction = 0.0f, bool frictionWhileAccelerating = false)
        {
            Position = position;
            Speed = speed ?? new Coord2();
            Acceleration = acceleration;
            AccelerationBase = accelerationBase;
            MaxSpeed = maxSpeed;
            Friction = friction;
            FrictionWhileAccelerating = frictionWhileAccelerating;
        }

        private Direction GetDirection() //TODO Start off here next
        {
            return Direction.Down;
        }
    }
}
