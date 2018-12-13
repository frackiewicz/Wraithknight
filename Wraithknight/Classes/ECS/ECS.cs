using System;
using System.Collections.Generic;
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
        Wolf,

        //Objects
        Wall,
        Floor,

        //Projectiles
        HeroKnightSlashWeak,
        HeroKnightSlashStrong
    }

    class ECS
    {
        private readonly Dictionary<int, Entity> _entityDictionary = new Dictionary<int, Entity>(); //TODO Breunig, a way to move hero to #1 spot? Performance to avoid long enumerations
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
            // Console.WriteLine(_entityDictionary.Count);
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

        public Entity GetHero()
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

        //TODO CreateEntities from 2D array (levelmaps)

        private void CreateSystems(ecsBootRoutine routine)
        {
            drawSystem = new DrawSystem(this, _camera);

            _systemSet.Add(new InputSystem(this, _camera));
            //_systemSet.Add(new HeroControlSystem(this));
            _systemSet.Add(new CollisionSystem(this));
            _systemSet.Add(new MovementSystem(this));
            _systemSet.Add(new TimerSystem(this));
            _systemSet.Add(new HealthSystem(this));
            _systemSet.Add(new IntelligenceSystem(this));

            _systemSet.Add(drawSystem); //add last for "true data"
        }
        #endregion

        #region EntityManagement

        public Entity CreateEntity(EntityType type, Vector2Ref position = null, Coord2 speed = null, GameTime gameTime = null, Allegiance allegiance = Allegiance.Neutral) //TODO Breunig, how to handle default values?
        {
            //this might be enough lol
            Vector2Ref safePosition = position ?? new Vector2Ref();
            Coord2 safeSpeed = speed ?? new Coord2();

            Entity entity = new Entity(type);

            #region actors
            if (type == EntityType.Hero)
            {
                entity.SetAllegiance(Allegiance.Friendly);
                entity.AddComponent(new MovementComponent(accelerationBase: 600, maxSpeed: 200, friction: 500, position: safePosition));
                entity.AddComponent(new AttackBehaviorComponent(new List<AttackComponent>()
                {
                    new AttackComponent(EntityType.HeroKnightSlashWeak, AttackType.Primary, entity.GetComponent<MovementComponent>().Position, 400, 0, 500),
                    new AttackComponent(EntityType.HeroKnightSlashStrong, AttackType.Secondary, entity.GetComponent<MovementComponent>().Position, 800, 0, 1000)
                }));
                entity.AddComponent(new IntelligenceNode(EntityType.Hero, entity.GetComponent<MovementComponent>().Position));
                entity.AddBindableComponent(new DrawComponent(Assets.GetTexture("hero"), drawRec: new AABB(0, 0, 16, 32), offset: new Point(0, -5)), entity.Components[typeof(MovementComponent)]);
                entity.AddBindableComponent(new CollisionComponent(collisionRectangle: new AABB(safePosition, new Vector2(8, 8)), isPhysical: true), entity.Components[typeof(MovementComponent)]);
                entity.AddBindableComponent(new InputComponent(true), new List<Component> { entity.Components[typeof(MovementComponent)], entity.Components[typeof(AttackBehaviorComponent)] }); //TODO Breunig, any better way to handle Multicomponents here? (i dont like the[0])
            }

            if (type == EntityType.Forest_Knight)
            {
                entity.SetAllegiance(Allegiance.Enemy);
                entity.AddComponent(new MovementComponent(accelerationBase: 400, maxSpeed: 100, friction: 300, position: safePosition));
                entity.AddComponent(new AttackBehaviorComponent(new List<AttackComponent>()
                {
                    new AttackComponent(EntityType.HeroKnightSlashWeak, AttackType.Primary, entity.GetComponent<MovementComponent>().Position, 300, 0, 1500)
                }));
                entity.AddComponent(new HealthComponent(20));
                entity.AddBindableComponent(new DrawComponent(Assets.GetTexture("hero"), drawRec: new AABB(0, 0, 16, 32), offset: new Point(0, -5), tint: Color.Blue), entity.Components[typeof(MovementComponent)]);
                entity.AddBindableComponent(new CollisionComponent(collisionRectangle: new AABB(safePosition, new Vector2(8, 8)), isPhysical: true), entity.Components[typeof(MovementComponent)]);
                entity.AddBindableComponent(new InputComponent(false), new List<Component> { entity.Components[typeof(MovementComponent)], entity.Components[typeof(AttackBehaviorComponent)] }); //TODO Breunig, any better way to handle Multicomponents here? (i dont like the[0])
                List<IntelligenceOrder> orders = new List<IntelligenceOrder>();
                orders.Add(new IntelligenceOrder(EntityType.Hero, 100, OrderType.Attack1, 1, 1000, true));
                orders.Add(new IntelligenceOrder(EntityType.Hero, 300, OrderType.Move, 0, 250, true));
                entity.AddBindableComponent(new IntelligenceComponent(orders, entity.GetComponent<MovementComponent>().Position), entity.Components[typeof(InputComponent)]);
            }
            #endregion
            #region objects
            else if (type == EntityType.Wall)
            {
                DrawComponent drawComponent;
                int rnd = _random.Next(0, 100);
                if (rnd <= 20)
                {
                    drawComponent = new DrawComponent(Assets.GetTexture("tree32"), new AABB((int)safePosition.X, (int)safePosition.Y, 32, 32), offset: new Point(-8, -16));
                }
                else
                {
                    drawComponent = new DrawComponent(Assets.GetTexture("tanne16"), new AABB((int)safePosition.X, (int)safePosition.Y, 16, 32), offset: new Point(0, -16));
                }
                entity.AddComponent(drawComponent);
                entity.AddComponent(new CollisionComponent(behavior: CollisionBehavior.Block, collisionRectangle: new AABB(safePosition.X, safePosition.Y, 16, 16), isWall: true, isPhysical: true));
            }
            else if (type == EntityType.Floor)
            {
                DrawComponent drawComponent;
                int rnd = _random.Next(0, 100);
                if (rnd <= 3)
                {
                    drawComponent = new DrawComponent(Assets.GetTexture("32_1"), new AABB((int)safePosition.X, (int)safePosition.Y, 16, 16), layerDepth: 0.5f);
                }
                else if (rnd <= 6)
                {
                    drawComponent = new DrawComponent(Assets.GetTexture("32_2"), new AABB((int)safePosition.X, (int)safePosition.Y, 16, 16), layerDepth: 0.5f);
                }
                else if (rnd <= 9)
                {
                    drawComponent = new DrawComponent(Assets.GetTexture("32_3"), new AABB((int)safePosition.X, (int)safePosition.Y, 16, 16), layerDepth: 0.5f);
                }
                else if (rnd <= 15)
                {
                    drawComponent = new DrawComponent(Assets.GetTexture("32_4"), new AABB((int)safePosition.X, (int)safePosition.Y, 16, 16), layerDepth: 0.5f);
                }
                else
                {
                    drawComponent = new DrawComponent(Assets.GetTexture("32_5"), new AABB((int)safePosition.X, (int)safePosition.Y, 16, 16), layerDepth: 0.5f);
                }
                entity.AddComponent(drawComponent);
            }
            #endregion
            #region projectiles
            else if (type == EntityType.HeroKnightSlashWeak)
            {
                entity.SetAllegiance(allegiance);
                entity.AddComponent(new MovementComponent(maxSpeed: 400, friction: 50, position: safePosition, speed: safeSpeed));
                entity.AddBindableComponent(new DrawComponent(size: new Point(16, 16), tint: Color.Red), entity.Components[typeof(MovementComponent)]);
                entity.AddBindableComponent(new CollisionComponent(behavior: CollisionBehavior.Pass, collisionRectangle: new AABB(safePosition, new Vector2(8, 8))), entity.Components[typeof(MovementComponent)]); //WRONG ORIGIN POINT
                entity.AddComponent(new TimerComponent(TimerType.Death, startTime: gameTime, targetLifespanInMilliseconds: 3000));
                entity.AddBindableComponent(new ProjectileComponent(power: 10, damage: 5, isPhasing: true), entity.Components[typeof(CollisionComponent)]);
            }
            else if (type == EntityType.HeroKnightSlashStrong)
            {
                entity.SetAllegiance(allegiance);
                entity.AddComponent(new MovementComponent(maxSpeed: 800, friction: 200, position: safePosition, speed: safeSpeed));
                entity.AddBindableComponent(new DrawComponent(size: new Point(16, 16), tint: Color.Blue), entity.Components[typeof(MovementComponent)]);
                entity.AddBindableComponent(new CollisionComponent(behavior: CollisionBehavior.Pass, collisionRectangle: new AABB(safePosition, new Vector2(8, 8))), entity.Components[typeof(MovementComponent)]); //WRONG ORIGIN POINT
                entity.AddComponent(new TimerComponent(TimerType.Death, startTime: gameTime, targetLifespanInMilliseconds: 2000));
                entity.AddBindableComponent(new ProjectileComponent(power: 20, damage: 10, isPhasing: true), entity.Components[typeof(CollisionComponent)]);
            }
            #endregion
            SetRoots(entity);
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

        private void SetRoots(Entity entity)
        {
            foreach (var component in entity.Components.Values)
            {
                component.RootID = entity.ID;
                component.Allegiance = entity.Allegiance;
            }
        }

        public void PurgeTheDead() //Experimental, no idea about possible side effects //TODO Breunig talk about direct Component killing
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
                        AddEntity(CreateEntity(EntityType.Forest_Knight, new Vector2Ref(x * level.TileWidth + level.TileWidth / 2, y * level.TileHeight + level.TileHeight / 2)));
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
