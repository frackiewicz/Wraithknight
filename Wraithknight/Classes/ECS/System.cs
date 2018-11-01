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

        //DrawSystem differs
        protected void CoupleComponents<T>(ICollection<T> Target, ICollection<Entity> From)
        {
            foreach (var entity in From)
            {
                IEnumerable<Component> newComponents = entity.GetComponents<T>();
                if (newComponents != null)
                {
                    foreach (var component in newComponents)
                    {
                        Target.Add(Functions_Operators.CastComponent<T>(component));
                        component.Activate(); //do you want this?
                    }
                }
                else { Console.WriteLine("Entity-" + entity.ID + " lacks " + typeof(T)); } // Output: Entity-0 lacks DrawComponent
            }
        }

        public virtual void FinalizeUpdate(GameTime gameTime)
        {

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
