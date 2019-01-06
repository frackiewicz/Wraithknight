using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace Wraithknight
{
    public enum EntityState
    {
        Idle,
        Moving,
        Attacking,
        Other
    }
    class EntityStateController
    {
        public EntityState State;
        public int CurrentStatePriority;
        public Direction Direction;
        public String StateIdentifier; //To coordinate with AnimationIdentifier?

        public void Clear()
        {
            State = EntityState.Idle;
            CurrentStatePriority = 0;
            Direction = Direction.Down;
            StateIdentifier = "";
        }
    }
}
