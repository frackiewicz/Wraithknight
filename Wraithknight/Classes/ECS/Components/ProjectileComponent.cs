using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wraithknight
{ 
    class ProjectileComponent : Component
    {
        public class HitTarget
        {
            public Component Component;
            public bool HasCooldown;
            public TimerComponent Cooldown;

            public HitTarget(Component component, bool hasCooldown, TimerComponent cooldown)
            {
                Component = component;
                HasCooldown = hasCooldown;
                Cooldown = cooldown;
            }
        }

        public int Power;
        public int Damage;
        public bool IsPhasing; //To allow the projectile to pass through actors, for example sword slashes should hit multiple enemies
        public bool InfinitePower; //Touching damage? TODO missing logic

        public List<HitTarget> HitTargets;
        public bool HasHitCooldown;
        public double HitCooldownMilliseconds;

        public ProjectileComponent(int power = 0, int damage = 0, bool isPhasing = false, bool infinitePower = false, double hitCooldownMilliseconds = -1)
        {
            Power = power;
            Damage = damage;
            IsPhasing = isPhasing;
            InfinitePower = infinitePower;
            if (IsPhasing)
            {
                HitTargets = new List<HitTarget>();
                HasHitCooldown = hitCooldownMilliseconds > -1;
                HitCooldownMilliseconds = hitCooldownMilliseconds;
            }
        }
    }
}
