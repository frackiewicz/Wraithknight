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
        private HashSet<TimerComponent> _timerComponents = new HashSet<TimerComponent>();

        public TimerSystem(ECS ecs) : base(ecs)
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
                    _ecs.KillGameObject(timer.RootID);
                }
            }
        }

        public override void Reset()
        {
            _timerComponents.Clear();
        }
    }
}
