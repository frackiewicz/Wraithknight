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

    class CollisionComponent : BindableComponent //you will have to be efficient with this one
    {
        public CollisionBehavior Behavior;
        public Rectangle CollisionRectangle;
        public Point Offset;

        public bool IsImpassable;
        public bool IsPhysical;

        public CollisionComponent(CollisionBehavior behavior = CollisionBehavior.Block, Rectangle collisionRectangle = new Rectangle(), Point offset = new Point(), bool isImpassable = false, bool isPhysical = false)
        {
            Behavior = behavior;
            SetIsPhysical();
            CollisionRectangle = collisionRectangle;
            Offset = offset;
            IsImpassable = isImpassable;
            IsPhysical = isPhysical;
        }

        private void SetIsPhysical()
        {
            IsPhysical = Behavior == CollisionBehavior.Block || Behavior == CollisionBehavior.Bounce;
        }
    }
}

/*
 * All Background Objects will be rectangular and axis aligned
 * Projectiles might be rotated -> think of an implementation
 * Characters are also very likely rectangular and axis aligned
 *
 */