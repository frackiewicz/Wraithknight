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

        public BindableComponent AddBinding(Type type) 
        {
            Bindings.Add(type, null);
            return this;
        }
    }
}
