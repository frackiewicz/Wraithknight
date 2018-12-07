using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace Wraithknight
{
    public enum OrderType
    {
        Attack1,
        Attack2,
        Follow,
        Move
    }
    internal struct IntelligenceOrder //maybe bind this to Input?
    {
        public EntityType Target;
        public int Range;
        public OrderType Order;
        public int Priority; //higher = more important
        public int UpdateCooldownMilliseconds;

        IntelligenceOrder(EntityType target, int range, OrderType order, int priority, int updateCooldownMilliseconds)
        {
            Target = target;
            Range = range;
            Order = order;
            Priority = priority;
            UpdateCooldownMilliseconds = updateCooldownMilliseconds;
        }
    }
    class IntelligenceComponent : BindableComponent //TODO bind to Input
    {
        public List<IntelligenceOrder> Orders = new List<IntelligenceOrder>();
        public int UpdateCooldownMilliseconds = 0;
        public Vector2 Pos; //use this as a source Node

        public IntelligenceComponent(List<IntelligenceOrder> orders)
        {
            Orders.AddRange(orders);
            Orders.Sort((a, b) => b.Priority.CompareTo(a.Priority));
        }

    }
}
