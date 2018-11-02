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

        public HealthSystem(ECS ecs) : base(ecs)
        {
            _ecs = ecs;
        }

        public override void RegisterComponents(ICollection<Entity> entities)
        {
            CoupleComponent(_components, entities);
        }
        public override void Update(GameTime gameTime)
        {
            foreach (var health in _components)
            {
                if (health.CurrentHealth < 0)
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
