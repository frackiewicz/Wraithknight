using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace Wraithknight
{
    public enum AttackType
    {
        Primary,
        Secondary
    }

    class AttackComponent : Component //Maybe take a more CurrentState based approach
    {
        public EntityType Projectile;
        public AttackType Type;
        public Vector2Ref SourcePos;
        public Vector2 PosOffset;
        public double PosOffsetInDirection;
        public int StartSpeed;
        public int AttackState; //for switching equipment?
        public double AttackCooldownMilliseconds;
        public double AttackDelayMilliseconds;

        public AttackComponent(EntityType projectile, AttackType type, Vector2Ref sourcePos, Vector2 posOffset ,double posOffsetInDirection = 0, int startSpeed = 0, int attackState = 0, double attackCooldownMilliseconds = 0, double attackDelayMilliseconds = 0)
        {
            MultiBinding = true;
            Projectile = projectile;
            Type = type;
            SourcePos = sourcePos;
            PosOffset = posOffset;
            PosOffsetInDirection = posOffsetInDirection;
            StartSpeed = startSpeed;
            AttackState = attackState;
            AttackCooldownMilliseconds = attackCooldownMilliseconds;
            AttackDelayMilliseconds = attackDelayMilliseconds;
        }
    }
}
