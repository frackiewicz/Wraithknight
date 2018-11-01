﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Wraithknight
{
    internal enum ecsBootRoutine
    {
        Testing
    }

    internal enum EntityType
    {
        Hero,
        Wall,
        KnightSlash
    }

    class ECS
    {
        private readonly Dictionary<int ,Entity> _entityDictionary = new Dictionary<int, Entity>(); //early testing says: count should stay below 1k
        private readonly HashSet<System> _systemSet = new HashSet<System>(); // replace with map for direct communication? //nvm, getSystem works just fine
        private DrawSystem drawSystem; //maybe turn into Interface + List for multiple systems?
        private Camera2D _camera;

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

        #region Routines
        private void CreateEntities(ecsBootRoutine routine)
        {
            if (routine == ecsBootRoutine.Testing)
            {
                AddEntity(CreateEntity(EntityType.Hero));
                AddEntity(CreateEntity(EntityType.Wall));
            }
        }

        private void CreateSystems(ecsBootRoutine routine)
        {
            drawSystem = new DrawSystem(this);

            if (routine == ecsBootRoutine.Testing)
            {
                _systemSet.Add(new InputSystem(this, _camera));
                _systemSet.Add(new HeroControlSystem(this));
                _systemSet.Add(new MovementSystem(this));
                _systemSet.Add(new CollisionSystem(this));
                _systemSet.Add(new TimerSystem(this));
                _systemSet.Add(new ProjectileSystem(this));
                _systemSet.Add(new HealthSystem(this));
            }

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

        public Entity CreateEntity(EntityType type, Vector2? position = null, Coord2 speed = null, GameTime gameTime = null) //this here needs some work
        {
            if(position == null) position = new Vector2(0, 0);
            if(speed == null) speed = new Coord2();

            #region actors
            Entity entity = new Entity(type);
            if (type == EntityType.Hero)
            {
                entity.Components.Add(new DrawComponent());
                entity.Components.Add(new InputComponent());
                entity.Components.Add(new MovementComponent(accelerationBase: 600, maxSpeed: 200, friction: 500));
                entity.Components.Add(new CollisionComponent(collisionRectangle: new Rectangle(new Point(0,0), new Point(16,16))));
                entity.SetAllegiance(Allegiance.Friendly);
            }
            #endregion
            #region objects
            else if (type == EntityType.Wall)
            {
                entity.Components.Add(new DrawComponent(tint: Color.Blue));
                entity.Components.Add(new CollisionComponent(behavior: CollisionBehavior.Block, collisionRectangle: new Rectangle(new Point(0,0), new Point(16,16)), isImpassable: true));
            }
            #endregion
            #region projectiles
            else if (type == EntityType.KnightSlash)
            {
                float startingSpeed = 400;
                entity.Components.Add(new DrawComponent(tint: Color.Red));
                entity.Components.Add(new MovementComponent(maxSpeed: 400, friction: 50, position: (Vector2) position, speed: speed.ChangePolarLength(startingSpeed)));
                entity.Components.Add(new CollisionComponent(behavior: CollisionBehavior.Pass, collisionRectangle: new Rectangle(new Point(0, 0), new Point(16, 16)), isProjectile: true)); //WRONG ORIGIN POINT
                entity.Components.Add(new TimerComponent(TimerType.Death, startTime: gameTime, targetLifespanInMilliseconds: 3000));
                entity.Components.Add(new ProjectileComponent(power: 10, damage: 5, isPhasing: true, allegiance: Allegiance.Friendly));
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
            foreach (var component in entity.Components)
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
            foreach (var component in entity.Components)
            {
                component.RootID = entity.ID;
            }
        }

        public void PurgeTheDead() //Experimental, no idea about possible side effects
        {
            CleanEntities();
            ResetSystems();
            RegisterAllEntities();
            GC.Collect();
        }

        private void CleanEntities()
        {
            List<Entity> newList = new List<Entity>();
            foreach (var entity in _entityDictionary.Values)
            {
                if (entity.Alive)
                {
                    newList.Add(entity);
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
    }
}
