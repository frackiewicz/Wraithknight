using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace Wraithknight
{
    class MovementComponent : Component // TODO Implement movement with angles
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
