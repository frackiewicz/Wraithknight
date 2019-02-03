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

        public override void RegisterComponents(ICollection<Entity> entities)
        {
            CoupleComponent(_timerComponents, entities);
        }

        public override void Update(GameTime gameTime)
        {
            foreach (var timer in _timerComponents)
            {
                if (timer.Inactive) continue;
                if (timer.Over)
                {
                    if (timer.Type == TimerType.Death) timer.CurrentEntityState.Dead = true;
                }
            }
        }

        public override void Reset()
        {
            _timerComponents.Clear();
        }
    }
}
