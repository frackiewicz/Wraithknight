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
                if (health.IsDead)
                {
                    health.CurrentEntityState.Dead = true;
                }
            }
        }

        public override void Reset()
        {
            _components.Clear();
        }
    }
}
