using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wraithknight.Classes.ECS.Components
{
    public class ProjectileComponent : Component
    {
        public int Power;
        public int Damage;
        public bool isPhasing; //To allow the projectile to pass through actors, for example sword slashes should hit multiple enemies
    }
}
