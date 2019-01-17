﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace Wraithknight
{
    class AttackBehaviorComponent : Component
    {
        public class DelayedAttackClass
        {
            public double RemainingAttackDelayMilliseconds;
            public AttackComponent Attack;
            public Point Cursor;

            public DelayedAttackClass(double remainingAttackDelayMilliseconds, AttackComponent attack, Point cursor)
            {
                RemainingAttackDelayMilliseconds = remainingAttackDelayMilliseconds;
                Attack = attack;
                Cursor = cursor;
            }
        }

        public List<AttackComponent> AttackComponents = new List<AttackComponent>();
        public double RemainingAttackCooldownMilliseconds = 0;
        public DelayedAttackClass DelayedAttack;
        public bool CurrentlyBlockingAttack;

        public AttackBehaviorComponent(List<AttackComponent> attackComponents)
        {
            AttackComponents.AddRange(attackComponents);
        }

        public override void SetAllegiance(Allegiance allegiance)
        {
            base.SetAllegiance(allegiance);
            foreach (var attackComponent in AttackComponents)
            {
                attackComponent.SetAllegiance(allegiance);
            }
        }
    }
}
