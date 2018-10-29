using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Wraithknight
{
    public enum CollisionBehavior
    {
        Block,
        Disappear,
        Bounce,
        Stick,
        Pass //useful for sticky floors, event triggers etc
    }

    class CollisionComponent : Component //you will have to be efficient with this one
    {
        public CollisionBehavior Behavior = CollisionBehavior.Block;
        public Rectangle CollisionRectangle = new Rectangle();

        public CollisionComponent ChangeCollisionRectangleWidth(int width)
        {
            CollisionRectangle.Width = width;
            return this;
        }
        public CollisionComponent ChangeCollisionRectangleHeight(int height)
        {
            CollisionRectangle.Height = height;
            return this;
        }
    }
}

/*
 * All Background Objects will be rectangular and axis aligned
 * Projectiles might be rotated -> think of an implementation
 * Characters are also very likely rectangular and axis aligned
 *
 */