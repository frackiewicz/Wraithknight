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
                if (intelligence.Inactive) continue;
                if (intelligence.UpdateCooldownMilliseconds > 0)
                {
                    intelligence.UpdateCooldownMilliseconds -= gameTime.ElapsedGameTime.TotalMilliseconds;
                    continue;
                }
                ProcessOrders(intelligence);
            }
        }

        public override void Reset()
        {
            _components.Clear();
            _nodes.Clear();
        }

        private void ProcessOrders(IntelligenceComponent intelligence)
        {
            foreach (var order in intelligence.Orders)
            {
                foreach (var node in _nodes)
                {
                    if (node.Inactive) continue;
                    if (order.Target == node.Type && Vector2.Distance(intelligence.Pos, node.Pos) <= order.Range)
                    {
                        ExecuteOrder(intelligence, order, node);
                        return;
                    }
                }
            }

            ResetInputs(intelligence);
        }

        private void ExecuteOrder(IntelligenceComponent intelligence, IntelligenceOrder order, IntelligenceNode node)
        {
            InputComponent input = null;
            if(intelligence.Bindings.TryGetValue(typeof(InputComponent), out var binding)) input = binding as InputComponent;

            intelligence.State = order.Order;


            if (order.Order == OrderType.Attack1 || order.Order == OrderType.Attack2)
            {
                input.CursorPoint = node.Pos;
                intelligence.TargetPos = node.Pos;
                if (order.Order == OrderType.Attack1) input.PrimaryAttack = true;
                if (order.Order == OrderType.Attack2) input.SecondaryAttack = true;
            }

            if (order.Order == OrderType.Follow || order.Order == OrderType.Move)
            {
                input.MovementDirection = VectorDirection(intelligence.Pos, node.Pos);
                if (order.Order == OrderType.Follow) intelligence.TargetPos = node.Pos;
                if (order.Order == OrderType.Move) intelligence.TargetPos = new Vector2Ref(node.Pos);
            }

            intelligence.UpdateCooldownMilliseconds = order.UpdateCooldownMilliseconds;
        }

        private void ResetInputs(IntelligenceComponent intelligence)
        {
            intelligence.TargetPos = null;
            intelligence.State = OrderType.Null;

            InputComponent input = null;
            if (intelligence.Bindings.TryGetValue(typeof(InputComponent), out var binding)) input = binding as InputComponent;

            input.MovementDirection.X = 0;
            input.MovementDirection.Y = 0;
            input.PrimaryAttack = false;
            input.SecondaryAttack = false;
            input.SwitchWeapons = false;
            input.Action = false;
            input.Blink = false;
        }

        private Coord2 _polar = new Coord2();
        private Vector2 VectorDirection(Vector2Ref actor, Vector2Ref target)
        {
            _polar.ChangeX(actor.X - target.X);
            _polar.ChangeY(actor.Y - target.Y);
            _polar.ChangePolarLength(1);
            return _polar.Cartesian;
        }
    }
}
