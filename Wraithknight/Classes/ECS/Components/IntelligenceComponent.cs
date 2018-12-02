using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        public int Priority;

        IntelligenceOrder(EntityType target, int range, OrderType order, int priority)
        {
            Target = target;
            Range = range;
            Order = order;
            Priority = priority; 
        }
    }
    class IntelligenceComponent : Component
    {
        public List<IntelligenceOrder> Orders = new List<IntelligenceOrder>();

        public IntelligenceComponent(List<IntelligenceOrder> list)
        {
            if (Orders == null) Orders = list;
            else Orders.AddRange(list);
        }

    }
}
