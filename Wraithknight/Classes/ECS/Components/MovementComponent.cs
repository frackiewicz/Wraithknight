using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace Wraithknight //TODO structs could use some improvements
{ // Too much memory clutter?
    public class Coord2
    {
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

        public void ChangePolarLength(float newLength)
        {
            Polar.Length = newLength;
            ChangeCartesianFromPolar(Polar);
            AttemptToRoundCartesian();
        }

        public void AddVector2(Vector2 vector)
        {
            Cartesian += vector;
            ChangePolarFromCartesian(Cartesian);
            AttemptToRoundCartesian();
        }

        public void AddPolar2(Polar2 polar)
        {
            ChangePolarFromCartesian(Polar.InCartesian() + polar.InCartesian());
            ChangeCartesianFromPolar(Polar);
            AttemptToRoundCartesian();
        }

        #region Conversions
        public static Vector2 CartesianFromPolar(Polar2 polar)
        {
            return new Vector2((float) (polar.Length * Math.Cos(Functions_Math.DegreeToRadian(polar.Angle - 90f))), (float) (polar.Length * Math.Sin(Functions_Math.DegreeToRadian(polar.Angle - 90f))));
        }

        public static Polar2 PolarFromCartesian(Vector2 cartesian)
        {
            return new Polar2(cartesian);
        }

        private void ChangeCartesianFromPolar(Polar2 polar) //Make public?
        {
            Cartesian.X = (float)(polar.Length * Math.Cos(Functions_Math.DegreeToRadian(polar.Angle - 90f)));
            Cartesian.Y = (float)(polar.Length * Math.Sin(Functions_Math.DegreeToRadian(polar.Angle - 90f)));
        }

        private void ChangePolarFromCartesian(Vector2 cartesian)
        {
            Polar.ChangeLengthFromCartesian(cartesian);
            Polar.ChangeAngleFromCartesian(cartesian);
        }
        #endregion

        private void AttemptToRoundCartesian()
        {
            if ((int) (Cartesian.X + 1) - 0.001 <= Cartesian.X)
            {
                Cartesian.X = (int) Cartesian.X + 1;
            }

            if ((int) (Cartesian.Y + 1) - 0.001 <= Cartesian.Y)
            {
                Cartesian.Y = (int) Cartesian.Y + 1;
            }
        }
    }

    public class Polar2
    {
        public float Length;
        public float Angle;

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

        public void ChangeAngleFromCartesian(Vector2 cartesian)
        {
            Angle = (float)Functions_Math.RadianToDegree(Math.Atan2(cartesian.Y, cartesian.X)) + 90f;

        }

        public Vector2 InCartesian() 
        {
            return Coord2.CartesianFromPolar(this);//to reduce redundance
        }
    }

    public class MovementComponent : Component // TODO Implement movement with angles
    {
        public Direction Direction = Direction.Down; //havent figured out the use yet
        public Boolean Moving = false;

        public Vector2 Position = new Vector2(0, 0);
        public Coord2 Speed = new Coord2();
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
