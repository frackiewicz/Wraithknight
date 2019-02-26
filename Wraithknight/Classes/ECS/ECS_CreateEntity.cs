using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Wraithknight
{
    internal enum EntityType
    {
        Nothing, //PLACEHOLDER FOR MAPGEN
        Hero,

        //Enemies
        Forest_Knight,
        Forest_Wolf,
        Forest_Archer,

        //Objects
        Wall,
        Floor,

        Mushroom,

        Treestump,

        //Projectiles
        HeroKnightSlashWeak,
        HeroKnightSlashStrong,
        HeroKnightThrowingDagger
    }

    static class ECS_CreateEntity
    {
        private static ECS _ecs;
        private static readonly Random _random = new Random();

        public static void RegisterECS(ECS ecs)
        {
            _ecs = ecs;
        }

        public static Entity CreateEntity(EntityType type, Vector2Ref position = null, Coord2? speed = null, GameTime gameTime = null, Allegiance allegiance = Allegiance.Neutral)
        {
            Vector2Ref safePosition = position ?? new Vector2Ref();
            Coord2 safeSpeed = speed ?? new Coord2();

            Entity entity = new Entity(type);
            switch (type)
            {
                #region Actors
                
                case EntityType.Hero:
                {
                    entity.SetAllegiance(Allegiance.Friendly);
                    entity.SetStateComponent();
                    entity.AddComponent(new MovementComponent(accelerationBase: 1000, maxSpeed: 175, friction: 650, position: safePosition));
                    entity.AddComponent(new AttackBehaviorComponent(new List<AttackComponent>()
                    {
                        new AttackComponent(EntityType.HeroKnightSlashWeak, AttackType.Primary, entity.GetComponent<MovementComponent>().Position, new Vector2(0, 20), posOffsetInDirection: 35, startSpeed: 200, attackState: 0, attackCooldownMilliseconds: 500, blockInputDurationMilliseconds: 250, selfKnockback: -150),
                        new AttackComponent(EntityType.HeroKnightThrowingDagger, AttackType.Secondary, entity.GetComponent<MovementComponent>().Position, new Vector2(0, 20), posOffsetInDirection: 25, startSpeed: 800, attackState: 0, attackCooldownMilliseconds: 400, blockInputDurationMilliseconds: 100)
                    }, entity.GetComponent<MovementComponent>().Position));
                    entity.AddComponent(new IntelligenceNode(EntityType.Hero, entity.GetComponent<MovementComponent>().Position));
                    entity.AddComponent(new DrawComponent(Assets.GetTexture("hero"), drawRec: new AABB(0, 0, 32, 64), boundPos: entity.GetComponent<MovementComponent>().Position, offset: new Vector2(0, -16)), typeof(MovementComponent));
                    entity.AddComponent(new HealthComponent(20), typeof(MovementComponent));
                    entity.AddComponent(new CollisionComponent(collisionRectangle: new AABB(safePosition, new Vector2(16, 16)), offset: new Vector2(20, 40), isPhysical: true), new List<Type> { typeof(MovementComponent), typeof(HealthComponent) });
                    entity.AddComponent(new BlinkComponent(2000, 1000, 125), new List<Type> { typeof(InputComponent), typeof(MovementComponent), typeof(AttackBehaviorComponent), typeof(CollisionComponent), typeof(HealthComponent), typeof(DrawComponent) });
                    entity.AddComponent(new InputComponent(true), new List<Type> { typeof(MovementComponent), typeof(AttackBehaviorComponent), typeof(BlinkComponent) });
                    break;
                }
                case EntityType.Forest_Knight:
                {
                    entity.SetAllegiance(Allegiance.Enemy);
                    entity.SetStateComponent();
                    entity.AddComponent(new MovementComponent(accelerationBase: 250, maxSpeed: 100, friction: 300, position: safePosition));
                    entity.AddComponent(new AttackBehaviorComponent(new List<AttackComponent>()
                    {
                        new AttackComponent(EntityType.HeroKnightSlashWeak, AttackType.Primary, entity.GetComponent<MovementComponent>().Position, new Vector2(0, 20), posOffsetInDirection: 20, startSpeed: 300, attackState: 0, attackDelayMilliseconds: 2000, attackCooldownMilliseconds: 1500, cursorType: CursorType.Relative)
                    }, entity.GetComponent<MovementComponent>().Position));
                    entity.AddComponent(new DrawComponent(Assets.GetTexture("forestknightIdle"), drawRec: new AABB(0, 0, 64, 64), scale: new Vector2(1,1), boundPos: entity.GetComponent<MovementComponent>().Position, offset: new Vector2(0, -16)), typeof(MovementComponent));
                    entity.AddComponent(new HealthComponent(20, invincibilityTimeMilliseconds: 200), new List<Type> { typeof(MovementComponent), typeof(DrawComponent) });
                    entity.AddComponent(new AnimationComponent(AnimationStructures.GetAnimationList(type)), typeof(DrawComponent));
                    entity.AddComponent(new CollisionComponent(collisionRectangle: new AABB(safePosition, new Vector2(16, 16)), isPhysical: true), new List<Type> { typeof(MovementComponent), typeof(HealthComponent) });
                    entity.AddComponent(new InputComponent(false), new List<Type> { typeof(MovementComponent), typeof(AttackBehaviorComponent) });
                    List<IntelligenceOrder> orders = new List<IntelligenceOrder>();
                    orders.Add(new IntelligenceOrder(EntityType.Hero, 100, OrderType.Attack1, 1, 1000, true));
                    orders.Add(new IntelligenceOrder(EntityType.Hero, 300, OrderType.Move, 0, 250, true));
                    entity.AddComponent(new IntelligenceComponent(orders, entity.GetComponent<MovementComponent>().Position), typeof(InputComponent));
                    break;
                }
                case EntityType.Forest_Wolf:
                {
                    entity.SetAllegiance(Allegiance.Enemy);
                    entity.SetStateComponent();
                    entity.AddComponent(new MovementComponent(accelerationBase: 1200, maxSpeed: 150, friction: 400, position: safePosition));
                    entity.AddComponent(new DrawComponent(Assets.GetTexture("wolf"), drawRec: new AABB(0, 0, 64, 32), boundPos: entity.GetComponent<MovementComponent>().Position, offset: new Vector2(0, 0)), typeof(MovementComponent));
                    entity.AddComponent(new HealthComponent(15, invincibilityTimeMilliseconds: 200), new List<Type> { typeof(MovementComponent), typeof(DrawComponent) });
                    entity.AddComponent(new AnimationComponent(AnimationStructures.GetAnimationList(type)), typeof(DrawComponent));
                    entity.AddComponent(new CollisionComponent(collisionRectangle: new AABB(safePosition, new Vector2(16, 16)), isPhysical: true), new List<Type> { typeof(MovementComponent), typeof(HealthComponent) });
                    entity.AddComponent(new InputComponent(false), typeof(MovementComponent));
                    List<IntelligenceOrder> orders = new List<IntelligenceOrder>();
                    orders.Add(new IntelligenceOrder(EntityType.Hero, 50, OrderType.Null, 1, 250, true));
                    orders.Add(new IntelligenceOrder(EntityType.Hero, 300, OrderType.Move, 0, 250, true));
                    entity.AddComponent(new IntelligenceComponent(orders, entity.GetComponent<MovementComponent>().Position), typeof(InputComponent));
                    break;
                }
                case EntityType.Forest_Archer:
                {

                    break;
                }

                #endregion

                #region Objects

                case EntityType.Wall:
                {
                    DrawComponent drawComponent;

                    Texture2D texture;
                    int rnd = _random.Next(0, 100);
                    if (rnd <= 20)
                    {
                        texture = Assets.GetTexture("tree1");
                    }
                    else if (rnd <= 40)
                    {
                        texture = Assets.GetTexture("tree2");
                    }
                    else
                    {
                        texture = Assets.GetTexture("tree3");
                    }

                    drawComponent = new DrawComponent(texture, new AABB(safePosition.X, safePosition.Y, 32, 64), offset: new Vector2(0, -32));
                    entity.AddComponent(drawComponent);
                    entity.AddComponent(new CollisionComponent(behavior: CollisionBehavior.Block, collisionRectangle: new AABB(safePosition.X, safePosition.Y, 32, 32), isWall: true, isPhysical: true));
                    break;
                }
                case EntityType.Floor:
                {
                    DrawComponent drawComponent;

                    Texture2D texture;
                    int rnd = _random.Next(0, 100);
                    if (rnd <= 75)
                    {
                        texture = Assets.GetTexture("floor1");
                        }
                    else if (rnd <= 80)
                    {
                        texture = Assets.GetTexture("floor2");
                    }
                    else if (rnd <= 85)
                    {
                        texture = Assets.GetTexture("floor3");
                    }
                    else if (rnd <= 90)
                    {
                        texture = Assets.GetTexture("floor4");
                    }
                    else
                    {
                        texture = Assets.GetTexture("floor5");
                    }

                    drawComponent = new DrawComponent(texture, new AABB(safePosition.X, safePosition.Y, 32, 32), layerDepth: 0.5f);
                    entity.AddComponent(drawComponent);

                    rnd = _random.Next(0, 1000);
                    if (rnd <= 5)
                    {
                        _ecs.AddEntity(CreateEntity(EntityType.Mushroom, safePosition));
                    }

                    break;
                }
                case EntityType.Mushroom:
                {
                    DrawComponent drawComponent;

                    Texture2D texture;
                    int rnd = _random.Next(0, 100);
                    if (rnd <= 50)
                    {
                        texture = Assets.GetTexture("mushroom1");
                    }
                    else
                    {
                        texture = Assets.GetTexture("mushroom2");
                    }

                    drawComponent = new DrawComponent(texture, new AABB((int)safePosition.X, (int)safePosition.Y, 32, 32), layerDepth: 0.45f);
                    entity.AddComponent(drawComponent);
                    break;
                }
                case EntityType.Treestump:
                {
                    DrawComponent drawComponent;
                    
                    Texture2D texture;
                    int rnd = _random.Next(0, 100);
                    if (rnd <= 50)
                    {
                        texture = Assets.GetTexture("treestump1");
                    }
                    else
                    {
                        texture = Assets.GetTexture("treestump1");
                    }

                    drawComponent = new DrawComponent(texture, new AABB((int)safePosition.X, (int)safePosition.Y, 32, 32), layerDepth: 0.4f);
                    entity.AddComponent(drawComponent);
                    break;
                }

                #endregion

                #region Projectiles

                case EntityType.HeroKnightSlashWeak:
                {
                    Color tint = allegiance == Allegiance.Enemy ? Color.Red : Color.White;
                    entity.SetAllegiance(allegiance);
                    entity.SetStateComponent();
                    entity.AddComponent(new MovementComponent(maxSpeed: 100, friction: 200, position: safePosition, speed: safeSpeed));
                    entity.AddComponent(new TimerComponent(TimerType.Death, currentTime: gameTime, targetLifespanInMilliseconds: 500));
                    entity.AddComponent(new DrawComponent(Assets.GetTexture("heroslashweak"), drawRec: new AABB((int) safePosition.X, (int) safePosition.Y, 64, 64), boundPos: entity.GetComponent<MovementComponent>().Position, tint: tint, getRotationFromMovementVector: true), typeof(MovementComponent));
                    entity.AddComponent(new AnimationComponent(AnimationStructures.GetAnimationList(type)), typeof(DrawComponent));
                    entity.AddComponent(new ProjectileComponent(power: 10, damage: 5, knockback: 200, isPhasing: true, hitCooldownMilliseconds: 200), typeof(MovementComponent));
                    entity.AddComponent(new CollisionComponent(behavior: CollisionBehavior.Pass, collisionRectangle: new AABB(safePosition, new Vector2(16, 16))), new List<Type> {typeof(MovementComponent), typeof(ProjectileComponent) });
                    break;
                }
                case EntityType.HeroKnightSlashStrong:
                {
                    entity.SetAllegiance(allegiance);
                    entity.SetStateComponent();
                    entity.AddComponent(new MovementComponent(maxSpeed: 800, friction: 2000, position: safePosition, speed: safeSpeed));
                    entity.AddComponent(new TimerComponent(TimerType.Death, currentTime: gameTime, targetLifespanInMilliseconds: 500));
                    entity.AddComponent(new DrawComponent(Assets.GetTexture("heroslashweak"), drawRec: new AABB((int)safePosition.X, (int)safePosition.Y, 32, 32), boundPos: entity.GetComponent<MovementComponent>().Position, getRotationFromMovementVector: true, tint: Color.Blue), typeof(MovementComponent));
                    entity.AddComponent(new ProjectileComponent(power: 20, damage: 10, knockback: 400, isPhasing: true, hitCooldownMilliseconds: 200), typeof(MovementComponent));
                    entity.AddComponent(new CollisionComponent(behavior: CollisionBehavior.Pass, collisionRectangle: new AABB(safePosition, new Vector2(16, 16))), new List<Type> { typeof(MovementComponent), typeof(ProjectileComponent) });
                    break;
                }
                case EntityType.HeroKnightThrowingDagger:
                {
                    entity.SetAllegiance(allegiance);
                    entity.SetStateComponent();
                    entity.AddComponent(new MovementComponent(maxSpeed: 800, friction: 0, position: safePosition, speed: safeSpeed));
                    entity.AddComponent(new TimerComponent(TimerType.Death, currentTime: gameTime, targetLifespanInMilliseconds: 500));
                    entity.AddComponent(new DrawComponent(Assets.GetTexture("herothrowingdagger"), drawRec: new AABB((int)safePosition.X, (int)safePosition.Y, 16, 16), boundPos: entity.GetComponent<MovementComponent>().Position, getRotationFromMovementVector: true), typeof(MovementComponent));
                    entity.AddComponent(new AnimationComponent(AnimationStructures.GetAnimationList(type)), typeof(DrawComponent));
                    entity.AddComponent(new ProjectileComponent(power: 4, damage: 2, knockback: 50, isPhasing: false, hitCooldownMilliseconds: 0), typeof(MovementComponent));
                    entity.AddComponent(new CollisionComponent(behavior: CollisionBehavior.Pass, collisionRectangle: new AABB(safePosition, new Vector2(8, 8))), new List<Type> { typeof(MovementComponent), typeof(ProjectileComponent) });
                    break;
                }

                    #endregion
            }

            entity.FinalizeCreation();
            return entity;
        }
    }
}
