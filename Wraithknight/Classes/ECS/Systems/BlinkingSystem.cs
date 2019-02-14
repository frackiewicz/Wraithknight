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
                        if(ExecuteBlink(blink)) SetBlinkCooldown(blink, gameTime);
                    }
                    ResetBlink(blink);
                }
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

        private static bool ExecuteBlink(BlinkComponent blink)
        {
            InputComponent input = GetInputComponent(blink);

            return MovementBlink(blink, input) ||
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

        private static bool MovementBlink(BlinkComponent blink, InputComponent input)
        {
            if (blink.Bindings.TryGetValue(typeof(MovementComponent), out var movementBinding))
            {
                MovementComponent movement = movementBinding as MovementComponent;
                if (!input.MovementDirection.Equals(Vector2.Zero))
                {
                    movement.Speed.AddVector2(new Coord2(input.MovementDirection).ChangePolarLength(400).Cartesian);
                    return true;
                }
            }

            return false;
        }

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
