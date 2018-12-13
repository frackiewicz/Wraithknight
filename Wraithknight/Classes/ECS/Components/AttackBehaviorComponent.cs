using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wraithknight
{
    class AttackBehaviorComponent : Component
    {
        public List<AttackComponent> AttackComponents = new List<AttackComponent>();
        public double RemainingAttackCooldownMilliseconds = 0;
        public double RemainingAttackDelayMilliseconds = 0;
        public AttackComponent DelayedAttack;

        public AttackBehaviorComponent(List<AttackComponent> attackComponents)
        {
            AttackComponents.AddRange(attackComponents);
        }

    }
}
