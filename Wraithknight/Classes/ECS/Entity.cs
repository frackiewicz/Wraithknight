using System;
using System.Collections.Generic;
using System.Linq;

namespace Wraithknight
{
    public enum EntityType
    {
        Hero,
        Player
    }

    public class Entity
    {
        private static int IDcount = 0;

        public readonly int ID = IDcount++;
        public EntityType Type;
        public List<Component> Components = new List<Component>();

        public Entity(EntityType type)
        {
            Type = type;
        }

        public Entity SetComponents(List<Component> components)
        {
            Components = components ?? throw new ArgumentNullException(); //Breunig: duplicate instead
            return this;
        }

        public void AddComponent(Component component)
        {
            Components.Add(component);
        }

        public Component GetComponent<T>()
        {
            return Components.FirstOrDefault(component => component.GetType() == typeof(T));
        }

        public IEnumerable<Component> GetComponents<T>()
        {
            return Components.Where(component => component.GetType() == typeof(T)); // List.Addall possible
        }
    }
}
