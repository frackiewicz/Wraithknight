using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting;
using System.Text;
using System.Threading.Tasks;

namespace Wraithknight
{
    public abstract class Component
    {
        public bool Active { get; protected set; } = false;
        public int RootID;

        public virtual void Activate() { Active = true; }
        public virtual void Deactivate() { Active = false; }

    }
}
