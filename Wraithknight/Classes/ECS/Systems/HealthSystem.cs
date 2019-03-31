using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace Wraithknight
{
    class HealthSystem : System
    {
        private HashSet<HealthComponent> _components = new HashSet<HealthComponent>();

        public override void RegisterComponents(ICollection<Entity> entities)
        {
            CoupleComponent(_components, entities);
        }
        public override void Update(GameTime gameTime)
        {
            foreach (var health in _components)
            {
                if(health.IsDead) Kill(health);
                InvincibilityLogic(health, gameTime);
                ApplyVariables(health);
            }
        }

        public override void Reset()
        {
            _components.Clear();
        }

        private static void Kill(HealthComponent health)
        {
            health.CurrentEntityState.Dead = true;
        }

        private static void InvincibilityLogic(HealthComponent health, GameTime gameTime) 
        {
            if (health.RemainingInvincibilityTimeMilliseconds > 0)
            {
                health.RemainingInvincibilityTimeMilliseconds -= gameTime.ElapsedGameTime.TotalMilliseconds;
                if (health.RemainingInvincibilityTimeMilliseconds < 0) health.RemainingInvincibilityTimeMilliseconds = 0;
            }
            else if (health.ProcessedHealth < health.CurrentHealth)
            {
                health.RemainingInvincibilityTimeMilliseconds = health.InvincibilityTimeMilliseconds;
                FlashDrawComponentColor(health);
            }
            else
            {
                ResetDrawComponentColor(health);
            }
        }

        private static void FlashDrawComponentColor(HealthComponent health)
        {
            if (health.Bindings.TryGetValue(typeof(DrawComponent), out var binding))
            {
                DrawComponent draw = binding as DrawComponent;
                draw.Tint = Color.Red;
            }
        }

        private static void ResetDrawComponentColor(HealthComponent health)
        {
            if (health.Bindings.TryGetValue(typeof(DrawComponent), out var binding)) //TODO think of a way to prevent this shit being called all the time
            {
                DrawComponent draw = binding as DrawComponent;
                if (draw.Tint == Color.Red)
                {
                    draw.Tint = Color.White;
                }
            }
        }

        private static void ApplyVariables(HealthComponent health)
        {
            health.CurrentHealth = health.ProcessedHealth;
        }
    }
}
