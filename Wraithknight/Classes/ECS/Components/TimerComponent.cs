using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace Wraithknight
{
    class TimerComponent
    {
        public GameTime StartTime;
        public int TargetMilliseconds;

        public TimerComponent SetStartTime(GameTime startTime)
        {
            StartTime = startTime;
            return this;
        }

        public TimerComponent SetTargetMilliseconds(int target)
        {
            TargetMilliseconds = target;
            return this;
        }
    }
}
