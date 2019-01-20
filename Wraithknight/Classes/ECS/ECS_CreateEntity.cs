using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace Wraithknight
{
    class ECS_CreateEntity
    {
        private readonly ECS _ecs;
        private readonly List<Entity> _actors = new List<Entity>(); //this is only for debugging lol
        private readonly Random _random = new Random();

        public ECS_CreateEntity(ECS ecs, List<Entity> actors)
        {
            _ecs = ecs;
        }


        public Entity CreateEntity(EntityType type, Vector2Ref position = null, Coord2 speed = null, GameTime gameTime = null, Allegiance allegiance = Allegiance.Neutral)
        {
            //this might be enough lol
            Vector2Ref safePosition = position ?? new Vector2Ref();
            Coord2 safeSpeed = speed ?? new Coord2();

            Entity entity = new Entity(type);
            //TODO make a switch here
            #region actors
            if (type == EntityType.Hero)
            {
                entity.SetAllegiance(Allegiance.Friendly);
                entity.SetStateComponent();
                entity.AddComponent(new MovementComponent(accelerationBase: 1000, maxSpeed: 175, friction: 650, position: safePosition));
                entity.AddComponent(new AttackBehaviorComponent(new List<AttackComponent>()
                {
                    new AttackComponent(EntityType.HeroKnightSlashWeak, AttackType.Primary, entity.GetComponent<MovementComponent>().Position, new Vector2(0, 20), posOffsetInDirection: 35, startSpeed: 200, attackState: 0, attackCooldownMilliseconds: 500),
                    new AttackComponent(EntityType.HeroKnightSlashStrong, AttackType.Secondary, entity.GetComponent<MovementComponent>().Position, new Vector2(0, 20), posOffsetInDirection: 100, startSpeed: 800, attackState: 0, attackCooldownMilliseconds: 1000)
                }));
                entity.AddComponent(new HealthComponent(20));
                entity.AddComponent(new IntelligenceNode(EntityType.Hero, entity.GetComponent<MovementComponent>().Position));
                entity.AddBindableComponent(new DrawComponent(Assets.GetTexture("hero"), drawRec: new AABB(0, 0, 32, 64), boundPos: entity.GetComponent<MovementComponent>().Position, offset: new Vector2(0, -16)), entity.Components[typeof(MovementComponent)]);
                entity.AddBindableComponent(new CollisionComponent(collisionRectangle: new AABB(safePosition, new Vector2(16, 16)), offset: new Vector2(20, 40), isPhysical: true), new List<Component> { entity.Components[typeof(MovementComponent)], entity.Components[typeof(HealthComponent)] });
                entity.AddBindableComponent(new InputComponent(true), new List<Component> { entity.Components[typeof(MovementComponent)], entity.Components[typeof(AttackBehaviorComponent)] });
            }

            if (type == EntityType.Forest_Knight)
            {
                entity.SetAllegiance(Allegiance.Enemy);
                entity.SetStateComponent();
                entity.AddComponent(new MovementComponent(accelerationBase: 250, maxSpeed: 100, friction: 300, position: safePosition));
                entity.AddComponent(new AttackBehaviorComponent(new List<AttackComponent>()
                {
                    new AttackComponent(EntityType.HeroKnightSlashWeak, AttackType.Primary, entity.GetComponent<MovementComponent>().Position, new Vector2(0, 20), posOffsetInDirection: 20, startSpeed: 300, attackState: 0, attackDelayMilliseconds: 2000, attackCooldownMilliseconds: 1500)
                }));
                entity.AddComponent(new HealthComponent(20));
                entity.AddBindableComponent(new DrawComponent(Assets.GetTexture("hero"), drawRec: new AABB(0, 0, 32, 64), boundPos: entity.GetComponent<MovementComponent>().Position, offset: new Vector2(0, -16), tint: Color.Blue), entity.Components[typeof(MovementComponent)]);
                entity.AddBindableComponent(new CollisionComponent(collisionRectangle: new AABB(safePosition, new Vector2(16, 16)), isPhysical: true), new List<Component> { entity.Components[typeof(MovementComponent)], entity.Components[typeof(HealthComponent)] });
                entity.AddBindableComponent(new InputComponent(false), new List<Component> { entity.Components[typeof(MovementComponent)], entity.Components[typeof(AttackBehaviorComponent)] });
                List<IntelligenceOrder> orders = new List<IntelligenceOrder>();
                orders.Add(new IntelligenceOrder(EntityType.Hero, 100, OrderType.Attack1, 1, 1000, true));
                orders.Add(new IntelligenceOrder(EntityType.Hero, 300, OrderType.Move, 0, 250, true));
                entity.AddBindableComponent(new IntelligenceComponent(orders, entity.GetComponent<MovementComponent>().Position), entity.Components[typeof(InputComponent)]);
            }

            if (type == EntityType.Forest_Wolf)
            {
                entity.SetAllegiance(Allegiance.Enemy);
                entity.SetStateComponent();
                entity.AddComponent(new MovementComponent(accelerationBase: 1200, maxSpeed: 150, friction: 400, position: safePosition));
                entity.AddComponent(new HealthComponent(15));
                entity.AddBindableComponent(new DrawComponent(Assets.GetTexture("wolf"), drawRec: new AABB(0, 0, 64, 32), boundPos: entity.GetComponent<MovementComponent>().Position, offset: new Vector2(0, 0)), entity.Components[typeof(MovementComponent)]);
                entity.AddBindableComponent(new AnimationComponent(AnimationStructures.GetAnimationList(type)), entity.Components[typeof(DrawComponent)]);
                entity.AddBindableComponent(new CollisionComponent(collisionRectangle: new AABB(safePosition, new Vector2(16, 16)), isPhysical: true), new List<Component> { entity.Components[typeof(MovementComponent)], entity.Components[typeof(HealthComponent)] });
                entity.AddBindableComponent(new InputComponent(false), new List<Component>
                {
                    entity.Components[typeof(MovementComponent)]
                });
                List<IntelligenceOrder> orders = new List<IntelligenceOrder>();
                orders.Add(new IntelligenceOrder(EntityType.Hero, 50, OrderType.Null, 1, 250, true));
                orders.Add(new IntelligenceOrder(EntityType.Hero, 300, OrderType.Move, 0, 250, true));
                entity.AddBindableComponent(new IntelligenceComponent(orders, entity.GetComponent<MovementComponent>().Position), entity.Components[typeof(InputComponent)]);

            }
            #endregion
            #region objects
            //TODO If specifies Texture, Drawrec is defined after. REDUNDANCY
            else if (type == EntityType.Wall)
            {
                DrawComponent drawComponent;
                int rnd = _random.Next(0, 100);
                if (rnd <= 20)
                {
                    drawComponent = new DrawComponent(Assets.GetTexture("tree1"), new AABB(safePosition.X, safePosition.Y, 32, 64), offset: new Vector2(0, -32));
                }
                else if (rnd <= 40)
                {
                    drawComponent = new DrawComponent(Assets.GetTexture("tree2"), new AABB(safePosition.X, safePosition.Y, 32, 64), offset: new Vector2(0, -32));
                }
                else
                {
                    drawComponent = new DrawComponent(Assets.GetTexture("tree3"), new AABB(safePosition.X, safePosition.Y, 32, 64), offset: new Vector2(0, -32));
                }
                entity.AddComponent(drawComponent);
                entity.AddComponent(new CollisionComponent(behavior: CollisionBehavior.Block, collisionRectangle: new AABB(safePosition.X, safePosition.Y, 32, 32), isWall: true, isPhysical: true));
            }
            else if (type == EntityType.Floor)
            {
                DrawComponent drawComponent;
                int rnd = _random.Next(0, 100);
                if (rnd <= 75)
                {
                    drawComponent = new DrawComponent(Assets.GetTexture("floor1"), new AABB(safePosition.X, safePosition.Y, 32, 32), layerDepth: 0.5f);
                }
                else if (rnd <= 80)
                {
                    drawComponent = new DrawComponent(Assets.GetTexture("floor2"), new AABB(safePosition.X, safePosition.Y, 32, 32), layerDepth: 0.5f);
                }
                else if (rnd <= 85)
                {
                    drawComponent = new DrawComponent(Assets.GetTexture("floor3"), new AABB(safePosition.X, safePosition.Y, 32, 32), layerDepth: 0.5f);
                }
                else if (rnd <= 90)
                {
                    drawComponent = new DrawComponent(Assets.GetTexture("floor4"), new AABB(safePosition.X, safePosition.Y, 32, 32), layerDepth: 0.5f);
                }
                else
                {
                    drawComponent = new DrawComponent(Assets.GetTexture("floor5"), new AABB(safePosition.X, safePosition.Y, 32, 32), layerDepth: 0.5f);
                }

                entity.AddComponent(drawComponent);

                rnd = _random.Next(0, 1000);
                if (rnd <= 5)
                {
                    _ecs.AddEntity(CreateEntity(EntityType.Mushroom, safePosition));
                }
            }
            else if (type == EntityType.Mushroom)
            {
                DrawComponent drawComponent;

                int rnd = _random.Next(0, 100);
                if (rnd <= 50)
                {
                    drawComponent = new DrawComponent(Assets.GetTexture("mushroom1"), new AABB((int)safePosition.X, (int)safePosition.Y, 32, 32), layerDepth: 0.45f);
                }
                else
                {
                    drawComponent = new DrawComponent(Assets.GetTexture("mushroom2"), new AABB((int)safePosition.X, (int)safePosition.Y, 32, 32), layerDepth: 0.45f);
                }
                entity.AddComponent(drawComponent);
            }
            else if (type == EntityType.Treestump)
            {
                DrawComponent drawComponent;

                int rnd = _random.Next(0, 100);
                if (rnd <= 50)
                {
                    drawComponent = new DrawComponent(Assets.GetTexture("treestump1"), new AABB((int)safePosition.X, (int)safePosition.Y, 32, 32), layerDepth: 0.4f);
                }
                else
                {
                    drawComponent = new DrawComponent(Assets.GetTexture("treestump1"), new AABB((int)safePosition.X, (int)safePosition.Y, 32, 32), layerDepth: 0.4f);
                }
                entity.AddComponent(drawComponent);
            }
            #endregion
            #region projectiles
            else if (type == EntityType.HeroKnightSlashWeak)
            {
                entity.SetAllegiance(allegiance);
                entity.AddComponent(new MovementComponent(maxSpeed: 100, friction: 200, position: safePosition, speed: safeSpeed));
                entity.AddComponent(new TimerComponent(TimerType.Death, currentTime: gameTime, targetLifespanInMilliseconds: 500));
                entity.AddBindableComponent(new DrawComponent(drawRec: new AABB((int)safePosition.X, (int)safePosition.Y, 32, 32), boundPos: entity.GetComponent<MovementComponent>().Position, tint: Color.Red), entity.Components[typeof(MovementComponent)]);
                entity.AddComponent(new ProjectileComponent(power: 10, damage: 5, isPhasing: true, hitCooldownMilliseconds: 200));
                entity.AddBindableComponent(new CollisionComponent(behavior: CollisionBehavior.Pass, collisionRectangle: new AABB(safePosition, new Vector2(16, 16))), new List<Component> { entity.Components[typeof(MovementComponent)], entity.Components[typeof(ProjectileComponent)] });

            }
            else if (type == EntityType.HeroKnightSlashStrong)
            {
                entity.SetAllegiance(allegiance);
                entity.AddComponent(new MovementComponent(maxSpeed: 800, friction: 2000, position: safePosition, speed: safeSpeed));
                entity.AddComponent(new TimerComponent(TimerType.Death, currentTime: gameTime, targetLifespanInMilliseconds: 500));
                entity.AddBindableComponent(new DrawComponent(drawRec: new AABB((int)safePosition.X, (int)safePosition.Y, 32, 32), boundPos: entity.GetComponent<MovementComponent>().Position, tint: Color.Blue), entity.Components[typeof(MovementComponent)]);
                entity.AddComponent(new ProjectileComponent(power: 20, damage: 10, isPhasing: true, hitCooldownMilliseconds: 200));
                entity.AddBindableComponent(new CollisionComponent(behavior: CollisionBehavior.Pass, collisionRectangle: new AABB(safePosition, new Vector2(16, 16))), new List<Component> { entity.Components[typeof(MovementComponent)], entity.Components[typeof(ProjectileComponent)] });
            }
            #endregion

            if (type == EntityType.Hero || type == EntityType.Forest_Wolf || type == EntityType.Forest_Knight)
            {
                _actors.Add(entity);
            }
            return entity;
        }
    }
}
