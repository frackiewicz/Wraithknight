using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace Wraithknight
{
    /*
     * Alright,
     * lets use multiple statetypes over here
     * since there are way too many combinations it would be retarded to just make a single enum for all of this
     */
    public enum EntityState
    {
        Idle,
        Moving,
        Attacking,
        Dying,
        Dead, //TODO figure out a good way to do this
        Other,
        None
    }
    class StateComponent : Component
    {
        public EntityState PreviousState;
        public EntityState CurrentState;
        public int CurrentStatePriority;
        public Direction Direction;
        public Direction Orientation;

        public bool ReadyToChange = true; //To fixate states and put them on a timer maybe?
        public bool RecentlyChanged;
        public String StateIdentifier; //To coordinate with AnimationIdentifier?

        public void Clear()
        {
            CurrentState = EntityState.Idle;
            CurrentStatePriority = 0;
            Direction = Direction.Down;
            ReadyToChange = true;
            StateIdentifier = "";
        }
    }
    /*
     * Priorities:
     * write them in code maybe?
     *
     * Idle = 0
     * Moving = 1
     * Attacking = 2
     * Acting = 2
     */
}
