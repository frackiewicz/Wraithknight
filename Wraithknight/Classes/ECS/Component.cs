using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting;
using System.Text;
using System.Threading.Tasks;

namespace Wraithknight
{
    abstract class Component
    {
        private static int IDcount = 0;

        public readonly int ID = IDcount++;

        public bool MultiBinding = false;
        public bool Inactive { get; protected set; } = true;
        public int RootID;
        public Allegiance Allegiance; //RootAllegiance
        public EntityStateController EntityState;

        public virtual void Activate() { Inactive = false; }
        public virtual void Deactivate() { Inactive = true; }

        public virtual void SetAllegiance(Allegiance allegiance) { Allegiance = allegiance; }

        public override int GetHashCode()
        {
            return ID;
        }

        public override bool Equals(object obj)
        {
            var temp = obj as Component;
            if (temp == null) return false;
            return ID.GetHashCode() == temp.GetHashCode();
        }
    }
}
