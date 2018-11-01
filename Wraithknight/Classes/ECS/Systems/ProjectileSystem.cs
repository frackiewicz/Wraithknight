using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace Wraithknight
{
    class ProjectileSystem : System
    {
        private HashSet<ProjectileComponent> _components = new HashSet<ProjectileComponent>();

        private ProjectileComponent targetProjectile = null;
        private HealthComponent targetHealth = null;

        public override void RegisterComponents(ICollection<Entity> entities)
        {
            CoupleComponents(_components, entities);
        }

        public ProjectileSystem(ECS ecs) : base(ecs)
        {
            _ecs = ecs;
        }

        public override void Update(GameTime gameTime)
        {
            foreach (var actorProjectile in _components)
            {
                if (!actorProjectile.Active) continue;
                if (actorProjectile.Power <= 0)
                {
                    _ecs.KillGameObject(actorProjectile.RootID);
                    continue;
                }

                foreach (var target in actorProjectile.CurrentTargets)
                {
                    targetProjectile = target.GetComponent<ProjectileComponent>();
                    if (targetProjectile != null && targetProjectile.Allegiance != actorProjectile.Allegiance)
                    {
                        HandleProjectileCollision(actorProjectile, targetProjectile);
                        actorProjectile.CurrentTargets.Remove(target);
                        continue;
                    }

                    targetHealth = target.GetComponent<HealthComponent>();
                    if (targetHealth != null)
                    {
                        HandleHealthCollision(actorProjectile, targetHealth);
                        actorProjectile.CurrentTargets.Remove(target);
                        continue;
                    }
                }
            }
        }

        public override void Reset()
        {
            _components.Clear();
        }

        private void HandleProjectileCollision(ProjectileComponent actor, ProjectileComponent target)
        {
            if (actor.IsPhasing && target.IsPhasing) //slash vs slash
            {
                int original = actor.Power;
                actor.Power -= target.Power;
                target.Power -= original;
                target.CurrentTargets.Remove(_ecs.GetEntity(actor.RootID));
            }
            else if (actor.IsPhasing && !target.IsPhasing) //slash vs arrow
            {
                actor.Power -= target.Damage;
                if (actor.Power > 0)
                {
                    if (actor.Allegiance == Allegiance.Friendly) target.Allegiance = Allegiance.Friendly;
                    else target.Allegiance = Allegiance.Neutral;

                    //Deflect
                }
            }
        }

        private void HandleHealthCollision(ProjectileComponent actor, HealthComponent target)
        {
            DeliverDamageToHealth(actor, target);
            if (target.CurrentHealth > 0 && !actor.IsPhasing) //projectile didnt penetrate
            {
                _ecs.KillGameObject(actor.RootID);
            }
        }


        #region CleanCode

        private static bool HasTargets(ProjectileComponent projectile)
        {
            return projectile.CurrentTargets.Count > 0;
        }

        private static void DeliverDamageToHealth(ProjectileComponent actor, HealthComponent target)
        {
            if (actor.Power >= actor.Damage) //actor can afford full strike
            {
                if (target.CurrentHealth >= actor.Damage) //target can afford full strike
                {
                    target.CurrentHealth -= actor.Damage;
                    actor.Power -= actor.Damage;
                }
                else //target takes partial strike
                {
                    actor.Power -= target.CurrentHealth;
                    target.CurrentHealth = 0;
                }
            }
            else //actor can only afford partial strike
            {
                if (target.CurrentHealth >= actor.Power) //target can afford full strike
                {
                    target.CurrentHealth -= actor.Power;
                    actor.Power = 0;
                }
                else //target takes partial strike
                {
                    actor.Power -= target.CurrentHealth;
                    target.CurrentHealth = 0;
                }
            }
        }

        #endregion
    }
}