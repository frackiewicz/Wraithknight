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
                        ExecuteBlink(blink);
                        SetBlinkCooldown(blink, gameTime);
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

        private static void ExecuteBlink(BlinkComponent blink)
        {
            Console.WriteLine("blink");
            if (blink.Bindings.TryGetValue(typeof(MovementComponent), out var binding))
            {
                MovementComponent movement = binding as MovementComponent;
                movement.Speed.AddVector2(new Coord2(movement.Speed.Cartesian).ChangePolarLength(400).Cartesian);
            }
        }

        private static void ResetBlink(BlinkComponent blink)
        {
            blink.BlinkTrigger = false;
        }
    }
}
