using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace Wraithknight
{
    class IntelligenceSystem : System //TODO Implement similar mapping behavior like in Collision
    {
        private List<IntelligenceComponent> _components = new List<IntelligenceComponent>();
        private List<IntelligenceNode> _nodes = new List<IntelligenceNode>();

        public IntelligenceSystem(ECS ecs) : base(ecs)
        {

        }

        public override void RegisterComponents(ICollection<Entity> entities)
        {
            CoupleComponent(_components, entities);
            CoupleComponent(_nodes, entities);
        }

        public override void Update(GameTime gameTime)
        {
            foreach (var intelligence in _components)
            {
                if(intelligence.UpdateCooldownMilliseconds <= 0 || intelligence.Inactive) continue;
                ProcessOrder(intelligence);
            }
        }

        public override void Reset()
        {
            _components.Clear();
            _nodes.Clear();
        }

        private void ProcessOrder(IntelligenceComponent intelligence)
        {
            foreach (var order in intelligence.Orders)
            {
                foreach (var node in _nodes)
                {
                    if (node.Inactive) continue;
                    if (order.Target == node.Type && Vector2.Distance(intelligence.Pos, node.Pos) <= order.Range)
                    {
                        //do stuff

                    }
                }
            }
        }

        private void ExecuteOrder()
        {

        }
    }
}
