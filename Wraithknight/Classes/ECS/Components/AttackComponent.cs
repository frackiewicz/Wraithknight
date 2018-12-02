using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wraithknight.Classes.ECS.Components
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
        public int AttackState; //for switching equipment?

        AttackComponent(EntityType projectile, AttackType type, int attackState = 0)
        {
            Projectile = projectile;
            Type = type;
            AttackState = attackState;
        }
    }
}
