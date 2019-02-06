using System;
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
        public Vector2Ref SourcePos;
        public Point Cursor;
        public CursorType CursorType;
        public bool CurrentlyBlockingAttack;

        public AttackBehaviorComponent(List<AttackComponent> attackComponents, Vector2Ref sourcePos)
        {
            AttackComponents.AddRange(attackComponents);
            SourcePos = sourcePos;
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
