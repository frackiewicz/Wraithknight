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
        public CollisionBehavior Behavior;
        public Rectangle CollisionRectangle;
        public Point Offset;
        public bool IsImpassable;

        public CollisionComponent(CollisionBehavior behavior = CollisionBehavior.Block, Rectangle collisionRectangle = new Rectangle(), Point offset = new Point(), bool isImpassable = false)
        {
            Behavior = behavior;
            CollisionRectangle = collisionRectangle;
            Offset = offset;
            IsImpassable = isImpassable;
        }

        public CollisionComponent ChangeCollisionBehavior(CollisionBehavior behavior)
        {
            Behavior = behavior;
            return this;
        }

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

        public CollisionComponent ChangeOffset(Point point)
        {
            Offset = point;
            return this;
        }

        public CollisionComponent ChangeIsImpassable(bool value)
        {
            IsImpassable = value;
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