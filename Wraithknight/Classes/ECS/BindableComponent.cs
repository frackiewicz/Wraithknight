using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wraithknight
{
    abstract class BindableComponent : Component
    {
        public readonly Dictionary<Type, Component> Bindings = new Dictionary<Type, Component>(); //ONLY ALLOWS 1:1 Bindings
        public readonly Dictionary<Type, List<Component>> MultiBindings = new Dictionary<Type, List<Component>>();

        public BindableComponent AddBinding(ICollection<Component> components)
        {
            foreach (var component in components)
            {
                AddBinding(component); 
            }
            return this;
        }

        public BindableComponent AddBinding(Component component) //TODO Breunig, maybe add a filter by type here to redirect to MultiBindings
        {
            Bindings.Add(component.GetType(), component);
            return this;
        }

        public BindableComponent AddMultiBinding(ICollection<Component> components)
        {
            foreach (var component in components)
            {
                AddMultiBinding(component);
            }

            return this;
        }

        public BindableComponent AddMultiBinding(Component component)
        {
            if (MultiBindings.TryGetValue(component.GetType(), out var list))
            {
                list.Add(component);
            }
            else
            {
                MultiBindings.Add(component.GetType(), new List<Component>());
                AddMultiBinding(component); //lol lazy
            }

            return this;
        }

        private bool TryMultiBinding(Component component) //TODO Breunig, is this clever?
        {
            if (Bindings.TryGetValue(component.GetType(), out var oldComponent))
            {
                MultiBindings.Add(component.GetType(), new List<Component>());
                if (MultiBindings.TryGetValue(component.GetType(), out var list))
                {
                    list.Add(oldComponent);
                    list.Add(component);
                }

                Bindings.Remove(component.GetType());
                return true;
            }

            return false;
        }
    }
}
