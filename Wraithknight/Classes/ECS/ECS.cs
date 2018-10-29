using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Wraithknight.Classes.ECS.Systems;

namespace Wraithknight
{
    internal enum ecsBootRoutine
    {
        Testing
    }

    public enum EntityType
    {
        Hero,
        Wall
    }

    internal class ECS
    {
        private readonly List<Entity> _entityList = new List<Entity>();
        private readonly List<System> _systemList = new List<System>(); // replace with map for direct communication?
        private DrawSystem drawSystem; //maybe turn into Interface + List for multiple systems?

        public void StartupRoutine(ecsBootRoutine routine)
        {
            CreateEntities(routine);
            CreateSystems(routine);
            RegisterAllEntities();
        }

        public void UpdateSystems(GameTime gameTime)
        {
            foreach (var System in _systemList)
            {
                System.Update(gameTime);
            }
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

        private void CreateSystems(ecsBootRoutine routine) // figure out good Routine
        {
            drawSystem = new DrawSystem();

            if (routine == ecsBootRoutine.Testing)
            {
                _systemList.Add(new InputSystem());
                _systemList.Add(new HeroControlSystem());
                _systemList.Add(new MovementSystem());
                _systemList.Add(new CollisionSystem());
            }

            _systemList.Add(drawSystem); //add last for "true data"
        }

        private void RegisterAllEntities()
        {
            foreach (var System in _systemList)
            {
                System.RegisterComponents(_entityList);
            }
        }
        #endregion

        public System GetSystem<T>()
        {
            return _systemList.FirstOrDefault(system => system.GetType() == typeof(T));
        }

        private Entity CreateEntity(EntityType type)
        {
            Entity entity = new Entity(type);
            if (type == EntityType.Hero)
            {
                entity.Components.Add(new DrawComponent());
                entity.Components.Add(new InputComponent());
                entity.Components.Add(new MovementComponent().ChangeAccelerationBase(600).ChangeMaxSpeed(200).ChangeFriction(500));
                entity.Components.Add(new CollisionComponent());
            }

            else if (type == EntityType.Wall)
            {
                entity.Components.Add(new DrawComponent().ChangeTint(Color.Blue));
                entity.Components.Add(new CollisionComponent().ChangeCollisionRectangleWidth(16).ChangeCollisionRectangleHeight(16));
            }

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
