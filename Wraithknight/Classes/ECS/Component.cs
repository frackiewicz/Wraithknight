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
        public bool active { get; protected set; } = false;
        public int RootID; // is this necessary?

        public virtual void Activate() { active = true; }
        public virtual void Deactivate() { active = false; }

    }
}
