using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wraithknight
{ 
    class ProjectileComponent : Component
    {
        public int Power;
        public int Damage;
        public bool IsPhasing; //To allow the projectile to pass through actors, for example sword slashes should hit multiple enemies
        public Allegiance Allegiance; //use this to differentiate between behavior

        public List<Entity> CurrentTargets = new List<Entity>();

        public ProjectileComponent(int power = 0, int damage = 0, bool isPhasing = false, Allegiance allegiance = Allegiance.Neutral)
        {
            Power = power;
            Damage = damage;
            IsPhasing = isPhasing;
            Allegiance = allegiance;
        }
    }
}
