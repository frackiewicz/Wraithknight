using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace Wraithknight
{
    class TimerSystem : System
    {
        private List<TimerComponent> _timerComponents = new List<TimerComponent>();


        public override void RegisterComponents(ICollection<Entity> entities)
        {
            CoupleComponents(_timerComponents, entities);
        }

        public override void Update(GameTime gameTime)
        {
            foreach (var timer in _timerComponents)
            {
                if (timer.StartTime.TotalGameTime.Milliseconds + timer.TargetMilliseconds > gameTime.TotalGameTime.Milliseconds)
                {
                    //Do shit
                }
            }
        }

        public override void ResetSystem()
        {
            throw new NotImplementedException();
        }
    }
}
