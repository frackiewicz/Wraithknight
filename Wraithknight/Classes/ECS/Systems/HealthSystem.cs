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
        private List<HealthComponent> _components = new List<HealthComponent>();
        private ECS _ecs;

        public HealthSystem(ECS ecs)
        {
            _ecs = ecs;
        }

        public override void RegisterComponents(ICollection<Entity> entities)
        {
            CoupleComponents(_components, entities);
        }

        public override void Update(GameTime gameTime)
        {
            foreach (var health in _components)
            {
                if (health.CurrentHealth < health.MinHealth)
                {
                    _ecs.KillEntity(health.RootID);
                }
            }
        }

        public override void Reset()
        {
            _components.Clear();
        }
    }
}
