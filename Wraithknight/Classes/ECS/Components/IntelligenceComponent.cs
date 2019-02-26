using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Configuration;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
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
        public OrderType Type;
        public int Priority; //higher = more important
        public int UpdateCooldownMilliseconds;
        public bool Reset; //onNotExecuted

        public IntelligenceOrder(EntityType target, int range, OrderType type, int priority, int updateCooldownMilliseconds, bool reset)
        {
            Target = target;
            Range = range;
            Type = type;
            Priority = priority;
            UpdateCooldownMilliseconds = updateCooldownMilliseconds;
            Reset = reset;
        }
    }
    class IntelligenceComponent : BindableComponent //TODO Add triggered orders, hit wall -> random movement, take damage -> seek source
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
