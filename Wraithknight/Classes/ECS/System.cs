using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace Wraithknight
{
    abstract class System
    {
        private static int IDcount = 0;

        public readonly int ID = IDcount++;

        protected ECS _ecs;

        public abstract void RegisterComponents(ICollection<Entity> entities);
        public          void RegisterComponents(Entity entity) { RegisterComponents(new List<Entity>(){entity}); }
        public abstract void Update(GameTime gameTime);
        public abstract void Reset();

        public System(ECS ecs)
        {
            _ecs = ecs;
        }

        protected void CoupleComponent<T>(ICollection<T> Target, Entity entity) //Add boolean to entity if it has multiples of same component types
        {
            if (entity.Components.TryGetValue(typeof(T), out var component))
            {
                Target.Add(Functions_Operators.CastComponent<T>(component));
                component.Activate(); //do you want this?
            }
        }

        protected void CoupleComponent<T>(ICollection<T> Target, ICollection<Entity> entities)
        {
            foreach (var entity in entities)
            {
                CoupleComponent(Target, entity);
            }
        }

        public override int GetHashCode()
        {
            return ID.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            var temp = obj as System;
            if (temp == null) return false;
            return ID.GetHashCode() == temp.GetHashCode();
        }
    }
}
