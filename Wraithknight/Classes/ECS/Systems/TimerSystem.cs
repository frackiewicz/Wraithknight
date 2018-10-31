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
        private ECS _ecs;

        public TimerSystem(ECS ecs)
        {
            _ecs = ecs;
        }

        public override void RegisterComponents(ICollection<Entity> entities)
        {
            CoupleComponents(_timerComponents, entities);
        }

        public override void Update(GameTime gameTime)
        {
            foreach (var timer in _timerComponents)
            {
                if (!timer.Active) continue;
                if (timer.StartTimeInMilliseconds != -1 && timer.StartTimeInMilliseconds + timer.TargetLifespanInMilliseconds < gameTime.TotalGameTime.TotalMilliseconds)
                {
                    if(timer.Type == TimerType.Death)
                    _ecs.KillEntity(timer.RootID);
                }
            }
        }

        public override void Reset()
        {
            _timerComponents.Clear();
        }
    }
}
