﻿using System;
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
        Other
    }

    public enum HealthState
    {
        Alive,
        TookDamage
    }
    class EntityStateController
    {
        public EntityState CurrentState;
        public int CurrentStatePriority;
        public Direction Direction;

        public bool ReadyToChange = true; //To fixate states and put them on a timer maybe?
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
     *
     * Idle = 0
     * Moving = 1
     * Attacking = 2
     * Acting = 2
     */
}
