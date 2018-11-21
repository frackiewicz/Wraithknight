﻿using System;
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
        Wall,
        Floor,
        KnightSlash
    }

    class ECS
    {
        private readonly Dictionary<int, Entity> _entityDictionary = new Dictionary<int, Entity>();
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

        public void DrawDebug()
        {
            foreach (var system in _systemSet)
            {
                if (system.GetType() == typeof(CollisionSystem))
                {
                    CollisionSystem collision = system as CollisionSystem;
                    HashSet<CollisionComponent> collisionComponents = collision.GetCollisionComponents();
                    foreach (var collisionComponent in collisionComponents)
                    {
                        if (!collisionComponent.Inactive)
                        {
                            Functions_Draw.DrawDebug(collisionComponent.CollisionRectangle);
                            Functions_Draw.DrawDebug(collisionComponent.CollisionRectangle.Center);
                        }
                    }
                    collision.DrawMinkowski();
                }
            }
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
                AddEntity(CreateEntity(EntityType.Hero, position: new Vector2(100, 0)));
                AddEntity(CreateEntity(EntityType.Wall, position: new Vector2(200, 0)));
            }
        }

        //TODO CreateEntities from 2D array (levelmaps)

        private void CreateSystems(ecsBootRoutine routine)
        {
            drawSystem = new DrawSystem(this);

            _systemSet.Add(new InputSystem(this, _camera));
            _systemSet.Add(new HeroControlSystem(this));
            _systemSet.Add(new CollisionSystem(this));
            _systemSet.Add(new MovementSystem(this));
            _systemSet.Add(new TimerSystem(this));
            _systemSet.Add(new HealthSystem(this));

            _systemSet.Add(drawSystem); //add last for "true data"
        }
        #endregion

        #region EntityManagement

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

        public Entity CreateEntity(EntityType type, Vector2? position = null, Coord2 speed = null, GameTime gameTime = null) //TODO Problem with Drawrecs
        {
            //this might be enough lol
            Vector2 safePosition = position ?? new Vector2(0, 0);
            Coord2 safeSpeed = speed ?? new Coord2();

            #region actors
            Entity entity = new Entity(type);
            if (type == EntityType.Hero)
            {
                entity.AddComponent(new MovementComponent(accelerationBase: 600, maxSpeed: 200, friction: 500, position: safePosition));
                entity.AddBindedComponent(new DrawComponent(Assets.GetTexture("hero"), drawRec: new Rectangle(0, 0, 16, 32), offset: new Point(0, -5)), entity.Components[typeof(MovementComponent)]);
                entity.AddBindedComponent(new CollisionComponent(collisionRectangle: new AABB(safePosition.X, safePosition.Y, 16, 16), isPhysical: true), entity.Components[typeof(MovementComponent)]);
                entity.AddComponent(new InputComponent());
                entity.SetAllegiance(Allegiance.Friendly);
            }
            #endregion
            #region objects
            else if (type == EntityType.Wall)
            {
                DrawComponent drawComponent;
                int rnd = _random.Next(0, 100);
                if (rnd <= 20)
                {
                    drawComponent = new DrawComponent(Assets.GetTexture("tree32"), new Rectangle((int) safePosition.X, (int) safePosition.Y, 32, 32), offset: new Point(-8, -16));
                }
                else
                {
                    drawComponent = new DrawComponent(Assets.GetTexture("tanne16"), new Rectangle((int) safePosition.X, (int) safePosition.Y, 16, 32), offset: new Point(0, -16));
                }
                entity.AddComponent(drawComponent);
                entity.AddComponent(new CollisionComponent(behavior: CollisionBehavior.Block, collisionRectangle: new AABB(safePosition.X, safePosition.Y, 16, 16), isImpassable: true, isPhysical: true));
            }
            else if (type == EntityType.Floor)
            {
                DrawComponent drawComponent;
                int rnd = _random.Next(0, 100);
                if (rnd <= 20)
                {
                    drawComponent = new DrawComponent(Assets.GetTexture("32_1"), new Rectangle((int)safePosition.X, (int)safePosition.Y, 16, 16), layerDepth: 0.5f);
                }
                else if (rnd <= 40)
                {
                    drawComponent = new DrawComponent(Assets.GetTexture("32_2"), new Rectangle((int)safePosition.X, (int)safePosition.Y, 16, 16), layerDepth: 0.5f);
                }
                else if (rnd <= 60)
                {
                    drawComponent = new DrawComponent(Assets.GetTexture("32_3"), new Rectangle((int)safePosition.X, (int)safePosition.Y, 16, 16), layerDepth: 0.5f);
                }
                else if (rnd <= 80)
                {
                    drawComponent = new DrawComponent(Assets.GetTexture("32_4"), new Rectangle((int)safePosition.X, (int)safePosition.Y, 16, 16), layerDepth: 0.5f);
                }
                else
                {
                    drawComponent = new DrawComponent(Assets.GetTexture("32_5"), new Rectangle((int)safePosition.X, (int)safePosition.Y, 16, 16), layerDepth: 0.5f);
                }
                entity.AddComponent(drawComponent);
            }
            #endregion
            #region projectiles
            else if (type == EntityType.KnightSlash)
            {
                float startingSpeed = 400;
                entity.AddComponent(new MovementComponent(maxSpeed: 400, friction: 50, position: safePosition, speed: safeSpeed.ChangePolarLength(startingSpeed)));
                entity.AddBindedComponent(new DrawComponent(size: new Point(16,16), tint: Color.Red), entity.Components[typeof(MovementComponent)]);
                entity.AddBindedComponent(new CollisionComponent(behavior: CollisionBehavior.Pass, collisionRectangle: new AABB(safePosition.X, safePosition.Y, 16, 16)), entity.Components[typeof(MovementComponent)]); //WRONG ORIGIN POINT
                entity.AddComponent(new TimerComponent(TimerType.Death, startTime: gameTime, targetLifespanInMilliseconds: 3000));
                entity.AddBindedComponent(new ProjectileComponent(power: 10, damage: 5, isPhasing: true), entity.Components[typeof(CollisionComponent)]);
                entity.SetAllegiance(Allegiance.Friendly);
            }
            #endregion
            SetRootIDs(entity);
            return entity;
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

        private void SetRootIDs(Entity entity)
        {
            foreach (var component in entity.Components.Values)
            {
                component.RootID = entity.ID;
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
            CleanEntities(CleanType.Hero);
            ResetSystems();
            RegisterAllEntities();
            //camera?
            GC.Collect();
        }

        private enum CleanType
        {
            Regular,
            Hero
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
                    AddEntity(CreateEntity(EntityType.Floor, new Vector2(x * level.TileWidth, y * level.TileHeight)));
                    if (level.Walls[x, y])
                    {
                        AddEntity(CreateEntity(EntityType.Wall, new Vector2(x * level.TileWidth, y * level.TileHeight)));
                    }

                    if (level.Data[x, y] == LevelData.HeroSpawn)
                    {
                        AddEntity(CreateEntity(EntityType.Hero, new Vector2(x * level.TileWidth, y * level.TileHeight)));
                    }
                }
            }
            RegisterAllEntities();
        }
    }
}
