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
        public Dictionary<Type, Component> Components = new Dictionary<Type, Component>();
        public Dictionary<Type, List<Component>> MultiComponents = new Dictionary<Type, List<Component>>(); //for multiples?

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
            if (component.MultiBinding) AddMultiComponent(component);
            else Components.Add(component.GetType(), component);
        }

        public void AddBindableComponent(BindableComponent component, Component bind)
        {
            component.AddBinding(bind);
            AddComponent(component);
        }

        public void AddBindableComponent(BindableComponent component, ICollection<Component> binds)
        {
            foreach (var bind in binds)
            {
                component.AddBinding(bind);
            }
            AddComponent(component);
        }

        public void AddMultiComponent(Component component) //only difference here is the data structure, you will differentiate manually
        {
            if (MultiComponents.TryGetValue(component.GetType(), out var list))
            {
                list.Add(component);
            }
            else
            {
                MultiComponents.Add(component.GetType(), new List<Component>());
                AddMultiComponent(component);
            }
        }

        //Add MultiBindedComponent if necessary

        public T GetComponent<T>() //Ignore this function, Use TryGetValue on the Dictionary instead
        {
            Components.TryGetValue(typeof(T), out var value);
            return (T)(object)value;
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
