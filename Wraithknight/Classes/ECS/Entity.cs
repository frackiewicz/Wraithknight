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
        public Dictionary<Type, Component> Components = new Dictionary<Type, Component>(); //no multiples of same type possible

        public Entity(EntityType type, Allegiance allegiance = Allegiance.Neutral) 
        {
            Type = type;
            SetAllegiance(allegiance);
        }

        public Entity SetAllegiance(Allegiance allegiance)
        {
            Allegiance = allegiance;
            return this;
        }

        public void AddComponent(Component component)
        {
            Components.Add(component.GetType(), component);
        }

        public void AddBindedComponent(BindableComponent component, Component bind)
        {
            component.AddBinding(bind);
            AddComponent(component);
        }

        public Component GetComponent<T>() //Ignore this function, Use TryGetValue on the Dictionary instead
        {
            Components.TryGetValue(typeof(T), out var value);
            return value;
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
