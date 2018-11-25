using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace Wraithknight
{
    class DebugDrawer
    {
        public ECS Ecs;
        private DrawSystem _drawSystem;


        private CollisionSystem _collisionSystem;
        private HashSet<CollisionComponent> _collisionComponents;

        private HashSet<MovementComponent> _movementComponents;

        public DebugDrawer(ECS ecs)
        {
            Ecs = ecs;
            RegisterSystems();
        }

        public void Draw()
        {
            if (Flags.ShowCollisionRecs)
            {
                foreach (var collisionComponent in _collisionComponents)
                {
                    if (!collisionComponent.Inactive)
                    {
                        Functions_Draw.DrawDebug(collisionComponent.CollisionRectangle);
                        Functions_Draw.DrawDebug(collisionComponent.CollisionRectangle.Center);
                    }
                }
                //collision.DrawMinkowski();
            }
            if (Flags.ShowMovementCenters)
            {
                foreach (var movementComponent in _movementComponents)
                {
                    if (!movementComponent.Inactive)
                    {
                        Functions_Draw.DrawDebug(movementComponent.Position, Color.Yellow);
                    }
                }
            }
        }

        public void RegisterSystems()
        {
            _drawSystem = Ecs.GetSystem<DrawSystem>();
            _collisionSystem = Ecs.GetSystem<CollisionSystem>();
            _collisionComponents = _collisionSystem.GetCollisionComponents();
            _movementComponents = Ecs.GetSystem<MovementSystem>().GetMovementComponents();
        }
    }
}
