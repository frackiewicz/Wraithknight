using System;
using System.Collections.Generic;
using System.Linq;

namespace Wraithknight
{
    class Entity
    {
        private static int IDcount = 0;

        public readonly int ID = IDcount++;
        public bool Alive = true; //used for garbage collection
        public Allegiance Allegiance;
        public EntityType Type;
        public HashSet<Component> Components = new HashSet<Component>();

        public Entity(EntityType type, Allegiance allegiance = Allegiance.Neutral) 
        {
            Type = type;
            Allegiance = allegiance;
        }

        public Entity SetAllegiance(Allegiance allegiance)
        {
            Allegiance = allegiance;
            return this;
        }

        public Entity SetComponents(HashSet<Component> components)
        {
            Components = components ?? throw new ArgumentNullException(); //Breunig: duplicate instead
            return this;
        }

        public void AddComponent(Component component)
        {
            Components.Add(component);
        }

        public T GetComponent<T>() //Really expensive TODO Figure out a way to avoid this crap as much as possible, maybe a seperate system just to save components by type
        {
            return Functions_Operators.CastComponent<T>(Components.FirstOrDefault(component => component.GetType() == typeof(T)));
        }

        public IEnumerable<Component> GetComponents<T>()
        {
            return Components.Where(component => component.GetType() == typeof(T));
        }

        public override int GetHashCode()
        {
            return ID.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            var temp = obj as Component;
            if (temp == null) return false;
            return ID.GetHashCode() == temp.GetHashCode();
        }
    }
}
