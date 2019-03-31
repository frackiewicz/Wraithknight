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
        Acting,
        Dying,
        Dead,
        None
    }
    class StateComponent : Component
    {
        //TODO Breunig any better way to define this?
        private readonly Dictionary<EntityState, int> _statePriorities = new Dictionary<EntityState, int>()
        {
            { EntityState.Idle, 0 },
            { EntityState.Moving, 1 },
            { EntityState.Attacking, 2 },
            { EntityState.Acting, 2 },
            { EntityState.Dying, 98 },
            { EntityState.Dead, 99 },
            { EntityState.None, -1 }
        };

        public EntityState PreviousState;
        public EntityState CurrentState { get; private set; }
        public int CurrentStatePriority { get; private set; }
        public Direction Direction;
        public Direction Orientation;

        public bool Blinking = false;
        public bool Dead = false;

        public bool ReadyToChange; //To fixate states and put them on a timer maybe?
        public bool RecentlyChanged;
        public String StateIdentifier; //To coordinate with AnimationIdentifier?

        public StateComponent()
        {
            Clear();
        }

        public void Clear()
        {
            CurrentState = EntityState.Idle;
            CurrentStatePriority = 0;
            Direction = Direction.Down;
            ReadyToChange = true;
            StateIdentifier = "";
        }

        public void TryToSetState(EntityState state)
        {
            if (CurrentStatePriority < _statePriorities[state])
            {
                CurrentState = state;
                CurrentStatePriority = _statePriorities[state];
            }
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
