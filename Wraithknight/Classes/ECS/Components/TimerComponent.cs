using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace Wraithknight
{
    public enum TimerType
    {
        Death,
        Flag
    }

    class TimerComponent : Component //TODO Breunig, Calculate Timers differently?
    {
        public TimerType Type;
        public GameTime CurrentTime;
        public double StartTimeInMilliseconds;
        public int TargetLifespanInMilliseconds;

        public bool Over => StartTimeInMilliseconds + TargetLifespanInMilliseconds < CurrentTime.TotalGameTime.TotalMilliseconds;

        public TimerComponent(TimerType type, GameTime startTime = null,int targetLifespanInMilliseconds = 0)
        {
            Type = type;
            CurrentTime = startTime;
            if (startTime != null) StartTimeInMilliseconds = startTime.TotalGameTime.TotalMilliseconds;
            else StartTimeInMilliseconds = -1;
            TargetLifespanInMilliseconds = targetLifespanInMilliseconds;
        }
    }
}
