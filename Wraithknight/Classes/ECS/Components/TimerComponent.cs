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
        public double TargetTimeSpanMilliseconds;

        public bool Over => CurrentTime.TotalGameTime.TotalMilliseconds > TargetTimeSpanMilliseconds;

        public TimerComponent(TimerType type, GameTime currentTime, double targetLifespanInMilliseconds = 0)
        {
            Type = type;
            CurrentTime = currentTime;
            TargetTimeSpanMilliseconds = currentTime.TotalGameTime.TotalMilliseconds + targetLifespanInMilliseconds;
        }
    }
}
