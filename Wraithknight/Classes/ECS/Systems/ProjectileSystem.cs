using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace Wraithknight
{
    class ProjectileSystem : System //obsolete with the new subsystem in place, think of another use?
    {
        private HashSet<ProjectileComponent> _components = new HashSet<ProjectileComponent>();

        public override void RegisterComponents(ICollection<Entity> entities)
        {
            throw new NotImplementedException();
        }

        public ProjectileSystem(ECS ecs) : base(ecs)
        {
            _ecs = ecs;
            throw new NotImplementedException();
        }

        public override void Update(GameTime gameTime)
        {
            throw new NotImplementedException();
        }

        public override void Reset()
        {
            _components.Clear();
        }
    }
}