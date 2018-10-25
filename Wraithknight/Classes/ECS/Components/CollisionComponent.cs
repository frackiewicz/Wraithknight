using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Wraithknight.Classes.ECS.Components
{
    public enum CollisionBehavior
    {
        Disappear,
        Bounce,
        Stick
    }

    class CollisionComponent
    {
        public CollisionBehavior Behavior = CollisionBehavior.Disappear;
        public Rectangle CollisionRectangle;


    }
}
