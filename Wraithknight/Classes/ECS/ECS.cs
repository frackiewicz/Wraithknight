using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Runtime.CompilerServices;
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
        Presenting,
        Generation
    }

    class ECS
    {
        private readonly Dictionary<int, Entity> _entityDictionary = new Dictionary<int, Entity>();
        //TODO Also replace Dictionary with Hashtable, might improve performance slightly
        private readonly List<Entity> _actors = new List<Entity>(); //this is only for debugging lol
        private readonly HashSet<System> _systemSet = new HashSet<System>();
        private DrawSystem drawSystem;
        private readonly Camera2D _camera;

        //enum for biome

        public ECS(Camera2D camera)
        {
            _camera = camera;
            ECS_CreateEntity.RegisterECS(this);
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
        //TODO Breunig lmao
        public Entity CreateEntity(EntityType type, Vector2Ref position = null, Coord2? speed = null, GameTime gameTime = null, Allegiance allegiance = Allegiance.Neutral)
        {
            Entity entity = ECS_CreateEntity.CreateEntity(type, position, speed, gameTime, allegiance);

            if (type == EntityType.Hero || type == EntityType.Forest_Wolf || type == EntityType.Forest_Knight)
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

        public void AddEntity(Entity entity)
        {
            _entityDictionary.Add(entity.ID, entity);
        }

        public void AddEntityList(List<Entity> list)
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

        #region LevelManagement

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
                    if (level.Data[x, y] != EntityType.Nothing)
                    {
                        AddEntity(CreateEntity(level.Data[x, y], new Vector2Ref(x * level.TileWidth + level.TileWidth / 2, y * level.TileHeight + level.TileHeight / 2)));
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

        #endregion
    }
}
