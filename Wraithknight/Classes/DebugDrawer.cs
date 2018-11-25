using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wraithknight
{
    class DebugDrawer
    {
        public ECS Ecs;
        private DrawSystem _drawSystem;


        private CollisionSystem _collisionSystem;
        private HashSet<CollisionComponent> _collisionComponents;

        public DebugDrawer(ECS ecs)
        {
            Ecs = ecs;
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
        }

        public void RegisterSystems()
        {
            _drawSystem = Ecs.GetSystem<DrawSystem>();
            _collisionSystem = Ecs.GetSystem<CollisionSystem>();
            _collisionComponents = _collisionSystem.GetCollisionComponents();
        }
    }
}
