using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wraithknight
{
    class CollisionLogicSubsystem
    {
        private ECS _ecs;

        public CollisionLogicSubsystem(ECS ecs)
        {
            _ecs = ecs;
        }

        public void RegisterComponents(ICollection<Entity> entities)
        {
            foreach (var entity in entities)
            {
                RegisterComponents(entity);
            }
        }

        public void RegisterComponents(Entity entity)
        {
        }

        public void ResetSystem()
        {
        }

        public void HandleCollision(CollisionComponent actor, CollisionComponent target)
        {
            if (actor.Bindings.TryGetValue(typeof(ProjectileComponent), out var actorProjectileComponent))
            {
                if (target.Bindings.TryGetValue(typeof(ProjectileComponent), out var targetProjectileComponent))
                {
                    ProjectileOnProjectile(actorProjectileComponent as ProjectileComponent, targetProjectileComponent as ProjectileComponent);
                }

                if (target.Bindings.TryGetValue(typeof(HealthComponent), out var targetHealthComponent))
                {
                    ProjectileOnHealth(actorProjectileComponent as ProjectileComponent, targetHealthComponent as HealthComponent);
                }
            }
        }

        private void ProjectileOnProjectile(ProjectileComponent actor, ProjectileComponent target)
        {
            if (actor.IsPhasing && target.IsPhasing) //slash vs slash
            {
                int original = actor.Power;
                actor.Power -= target.Power;
                target.Power -= original;
            }
            else if (actor.IsPhasing && !target.IsPhasing) //slash vs arrow
            {
                actor.Power -= Math.Min(target.Damage, target.Power);
                if (actor.Power > 0)
                {
                    if (actor.Allegiance == Allegiance.Friendly) target.Allegiance = Allegiance.Friendly;
                    else target.Allegiance = Allegiance.Neutral;

                    //Deflect
                }
            }

            if (actor.Power <= 0) _ecs.KillGameObject(actor.RootID);
            if (target.Power <= 0) _ecs.KillGameObject(target.RootID);
        }

        private void ProjectileOnHealth(ProjectileComponent actor, HealthComponent target)
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

            if (actor.Power <= 0) _ecs.KillGameObject(actor.RootID);
            if (target.CurrentHealth <= 0) _ecs.KillGameObject(target.RootID);
            else if (!actor.IsPhasing) //projectile didnt penetrate because target survived
            {
                _ecs.KillGameObject(actor.RootID);
            }
        }
    }
}