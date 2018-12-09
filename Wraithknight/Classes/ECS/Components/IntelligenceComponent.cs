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
        Move,
        Null
    }
    internal struct IntelligenceOrder //maybe bind this to Input?
    {
        public EntityType Target;
        public int Range;
        public OrderType Order;
        public int Priority; //higher = more important
        public int UpdateCooldownMilliseconds;

        public IntelligenceOrder(EntityType target, int range, OrderType order, int priority, int updateCooldownMilliseconds)
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
        public double UpdateCooldownMilliseconds = 0;
        public Vector2Ref Pos; //use this as a source Node

        public Vector2Ref TargetPos;
        public OrderType State;

        public IntelligenceComponent(List<IntelligenceOrder> orders, Vector2Ref pos)
        {
            Orders.AddRange(orders);
            Orders.Sort((a, b) => b.Priority.CompareTo(a.Priority));
            Pos = pos;
        }

    }
}
