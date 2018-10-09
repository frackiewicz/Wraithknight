using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wraithknight
{
    abstract class System
    {
        public abstract void RegisterComponents(ICollection<Entity> entities);
        public          void RegisterComponents(Entity entity) { RegisterComponents(new List<Entity>(){entity}); }
        public abstract void Update();
        public abstract void ResetSystem();

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
                        component.Activate();
                    }
                }
                else { Console.WriteLine("Entity-" + entity.ID + " lacks " + typeof(T)); } // Output: Entity-0 lacks DrawComponent
            }
        }
    }
}
