﻿using System;
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

    class AttackComponent : Component
    {
        public EntityType Projectile;
        public AttackType Type;
        public Vector2 SourcePos; //how this?
        public int StartSpeed;
        public int AttackState; //for switching equipment?

        AttackComponent(EntityType projectile, AttackType type, Vector2 sourcePos, int startSpeed = 0, int attackState = 0)
        {
            Projectile = projectile;
            Type = type;
            SourcePos = sourcePos;
            StartSpeed = startSpeed;
            AttackState = attackState;
        }
    }
}
