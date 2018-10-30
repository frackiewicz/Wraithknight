using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace Wraithknight
{
    internal enum ecsBootRoutine
    {
        Testing
    }

    public enum EntityType
    {
        Hero,
        Wall,
        KnightSlash
    }

    internal class ECS
    {
        private readonly List<Entity> _entityList = new List<Entity>(); //early testing says: count should stay below 1k
        private readonly List<System> _systemList = new List<System>(); // replace with map for direct communication? //nvm, getSystem works just fine
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
            foreach (var system in _systemList)
            {
                system.Update(gameTime);
            }
            // Console.WriteLine(_entityList.Count);
        }

        public void Draw()
        {
            drawSystem.Draw();
        }

        #region Routines
        private void CreateEntities(ecsBootRoutine routine)
        {
            if (routine == ecsBootRoutine.Testing)
            {
                _entityList.Add(CreateEntity(EntityType.Hero));
                _entityList.Add(CreateEntity(EntityType.Wall));
            }
        }

        private void CreateSystems(ecsBootRoutine routine)
        {
            drawSystem = new DrawSystem();

            if (routine == ecsBootRoutine.Testing)
            {
                _systemList.Add(new InputSystem(_camera));
                _systemList.Add(new HeroControlSystem(this));
                _systemList.Add(new MovementSystem());
                _systemList.Add(new CollisionSystem());
            }

            _systemList.Add(drawSystem); //add last for "true data"
        }
        #endregion

        private void RegisterAllEntities()
        {
            foreach (var system in _systemList)
            {
                system.RegisterComponents(_entityList);
            }
        }

        public void RegisterEntity(Entity entity)
        {
            _entityList.Add(entity);
            foreach (var system in _systemList)
            {
                system.RegisterComponents(entity);
            }
        }

        public T GetSystem<T>()
        {
            return Functions_Operators.CastSystem<T>(_systemList.FirstOrDefault(system => system.GetType() == typeof(T)));
        }

        public Entity CreateEntity(EntityType type, Vector2? position = null, Coord2 speed = null)
        {
            #region actors
            Entity entity = new Entity(type);
            if (type == EntityType.Hero)
            {
                entity.Components.Add(new DrawComponent());
                entity.Components.Add(new InputComponent());
                entity.Components.Add(new MovementComponent(accelerationBase: 600, maxSpeed: 200, friction: 500));
                entity.Components.Add(new CollisionComponent(collisionRectangle: new Rectangle(new Point(0,0), new Point(16,16))));
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
                entity.Components.Add(new CollisionComponent(behavior: CollisionBehavior.Pass, collisionRectangle: new Rectangle(new Point(0,0), new Point(16, 16)))); //WRONG ORIGIN POINT
            }
            #endregion
            SetRootIDs(entity);
            return entity;
        }

        private void SetRootIDs(Entity entity)
        {
            foreach (var component in entity.Components)
            {
                component.RootID = entity.ID;
            }
        }

    }
}
