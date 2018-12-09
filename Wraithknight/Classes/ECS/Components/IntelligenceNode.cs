using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace Wraithknight
{
    class IntelligenceNode : Component
    {
        public EntityType Type;
        public Vector2Ref Pos;

        public IntelligenceNode(EntityType type, Vector2Ref pos)
        {
            Type = type;
            Pos = pos;
        }
    }
}
