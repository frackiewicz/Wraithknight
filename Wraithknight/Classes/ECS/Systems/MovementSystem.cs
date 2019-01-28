using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Wraithknight;

namespace Wraithknight
{
    class MovementSystem : System
    {
        private readonly ECS _ecs;

        private HashSet<MovementComponent> _components = new HashSet<MovementComponent>();

        public MovementSystem(ECS ecs)
        {
            _ecs = ecs;
        }

        public override void RegisterComponents(ICollection<Entity> entities)
        {
            CoupleComponent(_components, entities);
        }

        public override void Update(GameTime gameTime)
        {
            if (_components.Count <= 0) return;

            foreach (MovementComponent movement in _components)
            {
                if (movement.Inactive) continue;
                movement.IsMoving = !movement.Speed.Cartesian.Equals(Constants.NullVector);

                if (movement.HasCollided) movement.HasCollided = false;
                else ApplySpeed(movement, gameTime);

                ApplyInertia(movement, gameTime);
                AccelerateUntilMaxSpeed(movement, gameTime);
                SetMovementStates(movement);
            }
        }

        public override void Reset()
        {
            _components.Clear();
        }

        public HashSet<MovementComponent> GetMovementComponents()
        {
            return _components;
        }

        private static void ApplyInertia(MovementComponent movement, GameTime gameTime)
        {
            if (movement.RootType == EntityType.Hero)
            {
                Functions_DebugWriter.WriteLine(movement.Acceleration.Cartesian.ToString());
            }
            if (movement.Speed.Polar.Length > 0 && (movement.FrictionWhileAccelerating || movement.Acceleration.Cartesian.Equals(Vector2.Zero) || movement.Speed.Polar.Length > movement.MaxSpeed))
            {
                if (movement.Speed.Polar.Length - movement.Friction * (float)gameTime.ElapsedGameTime.TotalSeconds < 0 )
                {
                    movement.Speed.ChangePolarLength(0);
                }
                else
                {
                    movement.Speed.ChangePolarLength(movement.Speed.Polar.Length - (movement.Friction * ((float) gameTime.ElapsedGameTime.TotalSeconds)));
                }
            }
        }

        private static void AccelerateUntilMaxSpeed(MovementComponent movement, GameTime gameTime)
        {
            if (movement.MaxSpeed != 0.0f) //has a max speed
            {
                double delta = movement.MaxSpeed - movement.Speed.Polar.Length;
                
                if (delta >= 0)
                {
                    movement.Speed.AddVector2(movement.Acceleration.Cartesian * (float)gameTime.ElapsedGameTime.TotalSeconds);
                }
                else
                {
                    double previousLength = movement.Speed.Polar.Length;
                    movement.Speed.AddVector2(movement.Acceleration.Cartesian * (float)gameTime.ElapsedGameTime.TotalSeconds);
                    movement.Speed.ChangePolarLength(previousLength);
                }
            }

        }

        private static void ApplySpeed(MovementComponent movement, GameTime gameTime)
        {
            movement.OldPosition.X = movement.Position.X;
            movement.OldPosition.Y = movement.Position.Y;
            movement.Position.Add(movement.Speed.Cartesian * (float)gameTime.ElapsedGameTime.TotalSeconds);
        }

        private void SetMovementStates(MovementComponent movement)
        {
            if(movement.CurrentEntityState == null || !movement.CurrentEntityState.ReadyToChange) return;
            StateComponent stateComponent = movement.CurrentEntityState;
            if (movement.IsMoving)
            {
                if (stateComponent.CurrentStatePriority < 1)
                {
                    stateComponent.CurrentState = EntityState.Moving;
                }

                if (stateComponent.CurrentState == EntityState.Moving)
                {
                    double PI = Math.PI;
                    double Angle = movement.Speed.Polar.Angle;

                    if (Angle < PI / 2 && Angle > -PI / 2) stateComponent.Orientation = Direction.Right;
                    if (Angle >= PI / 2 || Angle <= -PI / 2) stateComponent.Orientation = Direction.Left;

                    /*
                    if (Angle <= PI / 4 && Angle >= -PI / 4) stateComponent.Direction = Direction.Right;
                    if (Angle > PI / 4 && Angle < 3 * PI / 4) stateComponent.Direction = Direction.Down;
                    if (Angle < -PI / 4 && Angle > 3 * -PI / 4) stateComponent.Direction = Direction.Up;
                    if (Angle >= 3 * PI / 4 || Angle <= 3 * -PI / 4) stateComponent.Direction = Direction.Left;
                    */
                }
            }
            else
            {
                if (stateComponent.CurrentState == EntityState.Moving) stateComponent.Clear();
            }
        }
    }
}
