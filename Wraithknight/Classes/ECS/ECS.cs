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
                for (int i = 0; i < 10; i++)
                {
                    Entity entity = new Entity(EntityType.Player);
                    entity.AddComponent(new DrawComponent());
                    _entityList.Add(entity);
                }
            }
        }

        private void CreateSystems(ecsBootRoutine routine) // figure out good Routine
        {
            drawSystem = new DrawSystem();
            _systemList.Add(drawSystem);

            if (routine == ecsBootRoutine.Testing)
            {

            }
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

        public Entity CreateEntity(EntityType type)
        {
            Entity entity = new Entity(type);
            List<Component> components = new List<Component>();
            if (type == EntityType.Hero)
            {
                components.Add(new DrawComponent());
                components.Add(new InputComponent());
                components.Add(new MovementComponent());
            }
            return entity;
        }

    }
}
