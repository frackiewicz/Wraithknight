using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace Wraithknight
{
    class BlinkingSystem : System
    {
        private List<BlinkComponent> _components = new List<BlinkComponent>();

        public override void RegisterComponents(ICollection<Entity> entities)
        {
            CoupleComponent(_components, entities);
        }

        public override void Update(GameTime gameTime)
        {
            foreach (var blink in _components)
            {
                if (blink.BlinkTrigger)
                {
                    if (blink.Cooldown.Over)
                    {
                        if(ExecuteBlink(blink, gameTime)) SetBlinkCooldown(blink, gameTime);
                    }
                    ResetBlink(blink);
                }

                MaintainMovementBlink(blink);
            }
        }

        public override void Reset()
        {
            _components.Clear();
        }

        private static void SetBlinkCooldown(BlinkComponent blink, GameTime gameTime)
        {
            blink.Cooldown.SetTimer(gameTime, blink.CooldownMilliseconds);
        }

        private static bool ExecuteBlink(BlinkComponent blink, GameTime gameTime)
        {
            InputComponent input = GetInputComponent(blink);
            return ExecuteMovementBlink(blink, input, gameTime) ||
                   AttackBlink(blink, input);
        }

        private static InputComponent GetInputComponent(BlinkComponent blink) 
        {
            if (blink.Bindings.TryGetValue(typeof(InputComponent), out var inputBinding))
            {
                return inputBinding as InputComponent;
            }
            return null;
        }

        #region Movement

        private static bool ExecuteMovementBlink(BlinkComponent blink, InputComponent input, GameTime gameTime)
        {
            if (blink.Bindings.TryGetValue(typeof(MovementComponent), out var movementBinding))
            {
                MovementComponent movement = movementBinding as MovementComponent;
                if (!input.MovementDirection.Equals(Vector2.Zero))
                {
                    SetMovementVariables(blink, movement, input, gameTime);
                    SetMovementBlinkSpeed(blink, movement);
                    return true;
                }
            }

            return false;
        }

        private static void MaintainMovementBlink(BlinkComponent blink) //TODO Think about this, kinda wanna reorganize the blink logic in general
        {
            if (blink.Bindings.TryGetValue(typeof(MovementComponent), out var movementBinding))
            {
                MovementComponent movement = movementBinding as MovementComponent;
                if (!blink.MovementDurationTimer.Over)
                {
                    SetMovementBlinkSpeed(blink, movement);
                } else if (blink.MovementDurationTimer.Over && !blink.PreviousTimerOver)
                {
                    ExitMovementBlink(blink, movement);
                }

                blink.PreviousTimerOver = blink.MovementDurationTimer.Over;
            }
        }

        private static void ExitMovementBlink(BlinkComponent blink, MovementComponent movement)
        {
            movement.Speed = blink.MovementExitSpeed;
            if (blink.Bindings.TryGetValue(typeof(CollisionComponent), out var collisionBinding))
            {
                CollisionComponent collision = collisionBinding as CollisionComponent;
                collision.IsPhasing = false;
            }
            if (blink.Bindings.TryGetValue(typeof(DrawComponent), out var drawBinding))
            {
                DrawComponent draw = drawBinding as DrawComponent;
                if (draw.Tint == Color.Cyan) draw.Tint = Color.White;
            }
            blink.CurrentEntityState.Blinking = false;
        }

        private static void SetMovementVariables(BlinkComponent blink, MovementComponent movement, InputComponent input, GameTime gameTime)
        {
            blink.MovementDurationTimer.SetTimer(gameTime, blink.BlinkMovementDurationInMilliseconds);
            blink.MovementDirection = input.MovementDirection;
            blink.MovementExitSpeed = movement.Speed;

            SetMovementVariablesForBindings(blink);

            blink.CurrentEntityState.Blinking = true;
        }

        private static void SetMovementVariablesForBindings(BlinkComponent blink)
        {
            if (blink.InvulnerableOnMovementBlink)
            {
                if (blink.Bindings.TryGetValue(typeof(HealthComponent), out var healthBinding))
                {
                    HealthComponent health = healthBinding as HealthComponent;
                    //TODO make this shit invincible, use blink duration
                }
            }

            if (blink.Bindings.TryGetValue(typeof(CollisionComponent), out var collisionBinding))
            {
                CollisionComponent collision = collisionBinding as CollisionComponent;
                collision.IsPhasing = true;
            }

            if (blink.Bindings.TryGetValue(typeof(DrawComponent), out var drawBinding)) //TODO Breunig talk about Coupling
            {
                DrawComponent draw = drawBinding as DrawComponent;
                draw.Tint = Color.Cyan;
            }
        }

        private static void SetMovementBlinkSpeed(BlinkComponent blink, MovementComponent movement)
        {
            movement.Speed.SetVector2(new Coord2(blink.MovementDirection).ChangePolarLength(blink.BlinkMovementSpeed).Cartesian);
        }

        #endregion

        private static bool AttackBlink(BlinkComponent blink, InputComponent input)
        {
            if (blink.Bindings.TryGetValue(typeof(AttackBehaviorComponent), out var attackBinding))
            {
                AttackBehaviorComponent attackBehavior = attackBinding as AttackBehaviorComponent;
            }

            return false;
        }

        private static void ResetBlink(BlinkComponent blink)
        {
            blink.BlinkTrigger = false;
        }
    }
}
