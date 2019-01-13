using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Wraithknight
{
    internal enum ecsBootRoutine
    {
        Testing,
        Presenting
    }

    internal enum EntityType
    {
        Hero,
        
        //Enemies
        Forest_Knight,
        Forest_Wolf,

        //Objects
        Wall,
        Floor,

        Mushroom,

        Treestump,

        //Projectiles
        HeroKnightSlashWeak,
        HeroKnightSlashStrong
    }

    class ECS
    {
        private readonly Dictionary<int, Entity> _entityDictionary = new Dictionary<int, Entity>();
        //TODO Also replace Dictionary with Hashtable, might improve performance slightly
        private readonly List<Entity> _actors = new List<Entity>(); //this is only for debugging lol
        private readonly HashSet<System> _systemSet = new HashSet<System>();
        private DrawSystem drawSystem;
        private readonly Camera2D _camera;
        private readonly Random _random = new Random();

        //enum for biome

        public ECS(Camera2D camera)
        {
            _camera = camera;
        }

        public void StartupRoutine(ecsBootRoutine routine)
        {
            CreateEntities(routine);
            CreateSystems(routine);
            RegisterAllEntities();
        }

        public void UpdateSystems(GameTime gameTime)
        {
            if (InputReader.IsKeyPressed(Keys.RightShift))
            {
                PurgeTheDead();
            }
            foreach (var system in _systemSet)
            {
                system.Update(gameTime);
            }
        }

        public void Draw()
        {
            drawSystem.Draw();
        }

        public T GetSystem<T>()
        {
            return Functions_Operators.CastSystem<T>(_systemSet.FirstOrDefault(system => system.GetType() == typeof(T)));
        }

        public Entity GetEntity(int id)
        {
            return _entityDictionary[id];
        }

        public Entity GetHero() //TODO Maybe save Hero ID, since its always the same
        {
            foreach (var entity in _entityDictionary.Values)
            {
                if (entity.Type == EntityType.Hero)
                {
                    return entity;
                }
            }

            return null;
        }

        #region Routines
        private void CreateEntities(ecsBootRoutine routine)
        {
            if (routine == ecsBootRoutine.Testing)
            {
                AddEntity(CreateEntity(EntityType.Hero, position: new Vector2Ref(new Vector2(100, 0))));
                AddEntity(CreateEntity(EntityType.Wall, position: new Vector2Ref(new Vector2(200, 0))));
            }
        }

        private void CreateSystems(ecsBootRoutine routine)
        {
            drawSystem = new DrawSystem(this, _camera);

            _systemSet.Add(new StateSystem());

            _systemSet.Add(new AnimationSystem());
            _systemSet.Add(new InputSystem(this, _camera));
            _systemSet.Add(new CollisionSystem(this));
            _systemSet.Add(new MovementSystem(this));
            _systemSet.Add(new TimerSystem(this));
            _systemSet.Add(new HealthSystem(this));
            _systemSet.Add(new IntelligenceSystem());

            _systemSet.Add(drawSystem);
        }
        #endregion

        #region EntityManagement

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
                entity.AddBindableComponent(new CollisionComponent(collisionRectangle: new AABB(safePosition, new Vector2(16, 16)), offset: new Vector2(20,40), isPhysical: true), new List<Component> { entity.Components[typeof(MovementComponent)], entity.Components[typeof(HealthComponent)] });
                entity.AddBindableComponent(new InputComponent(true), new List<Component> { entity.Components[typeof(MovementComponent)], entity.Components[typeof(AttackBehaviorComponent)] });
            }

            if (type == EntityType.Forest_Knight)
            {
                entity.SetAllegiance(Allegiance.Enemy);
                entity.SetStateComponent();
                entity.AddComponent(new MovementComponent(accelerationBase: 250, maxSpeed: 100, friction: 300, position: safePosition));
                entity.AddComponent(new AttackBehaviorComponent(new List<AttackComponent>()
                {
                    new AttackComponent(EntityType.HeroKnightSlashWeak, AttackType.Primary, entity.GetComponent<MovementComponent>().Position, new Vector2(0, 20), posOffsetInDirection: 20, startSpeed: 300, attackState: 0, attackCooldownMilliseconds: 1500, attackDelayMilliseconds: 2000)
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
                    AddEntity(CreateEntity(EntityType.Mushroom, safePosition));
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
                entity.AddBindableComponent(new DrawComponent(drawRec: new AABB((int)safePosition.X, (int)safePosition.Y, 32, 32), boundPos: entity.GetComponent<MovementComponent>().Position, tint: Color.Red),  entity.Components[typeof(MovementComponent)]);
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

            if (type == EntityType.Hero || type == EntityType.Forest_Wolf  || type == EntityType.Forest_Knight)
            {
                _actors.Add(entity);
            }
            return entity;
        }

        private void RegisterAllEntities()
        {
            foreach (var system in _systemSet)
            {
                system.RegisterComponents(_entityDictionary.Values.ToList());
            }
        }

        public void RegisterEntity(Entity entity)
        {
            _entityDictionary.Add(entity.ID, entity);
            foreach (var system in _systemSet)
            {
                system.RegisterComponents(entity);
            }
        }

        private void AddEntity(Entity entity)
        {
            _entityDictionary.Add(entity.ID, entity);
        }

        private void AddEntityList(List<Entity> list)
        {
            foreach (var entity in list)
            {
                _entityDictionary.Add(entity.ID, entity);
            }
        }

        public void KillGameObject(int id)
        {
            KillGameObject(GetEntity(id));
        }

        public void KillGameObject(Entity entity)
        {
            //here you will differentiate between all the entityTypes for their unique deaths
            KillEntity(entity); //for now just kill it lmao
        }

        public void KillEntity(int id)
        {
            KillEntity(GetEntity(id));
        }

        public void KillEntity(Entity entity)
        {
            foreach (var component in entity.Components.Values)
            {
                component.Deactivate();   
            }
            entity.Alive = false;
        }

        public void TrueKillEntity(Entity entity)
        {
            _entityDictionary.Remove(entity.ID);
        }

        public void PurgeTheDead()
        {
            CleanEntities(CleanType.Regular);
            ResetSystems();
            RegisterAllEntities();
            GC.Collect();
        }

        public void PurgeForNextLevel()
        {
            CleanEntities(CleanType.Full);
            ResetSystems();
            RegisterAllEntities();
            //camera?
            GC.Collect();
        }

        private enum CleanType
        {
            Regular,
            Hero,
            Full
        }

        private void CleanEntities(CleanType type)
        {
            List<Entity> newList = new List<Entity>();

            if (type == CleanType.Regular)
            {
                foreach (var entity in _entityDictionary.Values)
                {
                    if (entity.Alive)
                    {
                        newList.Add(entity);
                    }
                }
            }
            
            if (type == CleanType.Hero)
            {
                foreach (var entity in _entityDictionary.Values)
                {
                    if (entity.Type == EntityType.Hero)
                    {
                        newList.Add(entity);
                    }
                }
            }
            _entityDictionary.Clear();
            AddEntityList(newList);
        }

        #endregion

        #region Systemmanagement

        private void ResetSystems()
        {
            foreach (var system in _systemSet)
            {
                system.Reset();
            }
        }

        #endregion

        public void ProcessLevel(Level level)
        {
            for (int x = 0; x < level.Walls.GetLength(0); x++)
            {
                for (int y = 0; y < level.Walls.GetLength(1); y++)
                {
                    AddEntity(CreateEntity(EntityType.Floor, new Vector2Ref(x * level.TileWidth, y * level.TileHeight)));
                    #region Walls
                    if (level.Walls[x, y])
                    {
                        AddEntity(CreateEntity(EntityType.Wall, new Vector2Ref(x * level.TileWidth, y * level.TileHeight)));
                    }
                    #endregion

                    #region MapData
                    if (level.Data[x, y] == LevelData.HeroSpawn)
                    {
                        Entity hero = GetHero();
                        if(hero == null) AddEntity(CreateEntity(EntityType.Hero, new Vector2Ref(x * level.TileWidth + level.TileWidth/2, y * level.TileHeight + level.TileHeight/2)));
                    }

                    if (level.Data[x, y] == LevelData.EnemySpawn)
                    {
                        AddEntity(CreateEntity(EntityType.Forest_Wolf, new Vector2Ref(x * level.TileWidth + level.TileWidth / 2, y * level.TileHeight + level.TileHeight / 2)));
                    }

                    if (level.Data[x, y] == LevelData.PathBlocker)
                    {
                        AddEntity(CreateEntity(EntityType.Treestump, new Vector2Ref(x * level.TileWidth + level.TileWidth / 2, y * level.TileHeight + level.TileHeight / 2)));
                    }
                    #endregion
                }
            }
            RegisterAllEntities();
            RegisterLevel(level);
        }

        private void RegisterLevel(Level level)
        {
            GetSystem<CollisionSystem>().RegisterLevel(level);
        }
    }
}
