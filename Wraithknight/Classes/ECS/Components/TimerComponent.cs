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
        Death
    }

    class TimerComponent : Component
    {
        public TimerType Type;
        public double StartTimeInMilliseconds;
        public int TargetLifespanInMilliseconds;

        public TimerComponent(TimerType type, GameTime startTime = null,int targetLifespanInMilliseconds = 0)
        {
            Type = type;
            if (startTime != null) StartTimeInMilliseconds = startTime.TotalGameTime.TotalMilliseconds;
            else StartTimeInMilliseconds = -1;
            TargetLifespanInMilliseconds = targetLifespanInMilliseconds;
        }
    }
}
