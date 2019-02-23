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
        public StateComponent StateComponent; //for ease of access, can be removed later?
        public Dictionary<Type, Component> Components = new Dictionary<Type, Component>();

        public Entity(EntityType type) 
        {
            Type = type;
        }


        public Entity SetAllegiance(Allegiance allegiance)
        {
            Allegiance = allegiance;
            foreach (var component in Components)
            {
                component.Value.SetAllegiance(Allegiance);
            }
            return this;
        }

        public Entity SetStateComponent()
        {
            StateComponent component = new StateComponent();
            StateComponent = component;
            AddComponent(component);
            return this;
        }

        public void AddComponent(Component component)
        {
            SetComponentRoots(component);
            Components.Add(component.GetType(), component);
        }

        public void AddComponent(BindableComponent component, Type bind) 
        {
            component.AddBinding(bind);
            AddComponent(component);
        }

        public void AddComponent(BindableComponent component, ICollection<Type> binds)
        {
            foreach (var bind in binds)
            {
                component.AddBinding(bind);
            }
            AddComponent(component);
        }

        private void SetComponentRoots(Component component)
        {
            component.RootID = ID;
            component.SetAllegiance(Allegiance);
            component.RootType = Type;
            if (StateComponent != null) component.CurrentEntityState = StateComponent;
        }

        public T GetComponent<T>() //Ignore this function, Use TryGetValue on the Dictionary instead
        {
            Components.TryGetValue(typeof(T), out var value);
            return (T)(object)value;
        }

        #region Finalization

        public void FinalizeCreation()
        {
            foreach (var component in Components)
            {
                if (component.Value is BindableComponent bindableComponent)
                {
                    FinalizeBindableComponent(bindableComponent);
                }
            }
        }

        private void FinalizeBindableComponent(BindableComponent component)
        {
            for (int i = 0; i < component.Bindings.Count; i++)
            {
                Type type = component.Bindings.ElementAt(i).Key;
                if (Components.TryGetValue(type, out var binding)) component.Bindings[type] = binding;
                else throw new ArgumentException("Entity does not contain binding type");
            }
        }

        #endregion

        #region Overrides

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

        #endregion

    }
}
