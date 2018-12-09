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
        DisappearOnWall,
        Bounce,
        Stick,
        Pass //useful for sticky floors, event triggers etc
    }

    class CollisionComponent : BindableComponent //TODO implement PositionRef here?
    {
        public CollisionBehavior Behavior;
        public AABB CollisionRectangle;
        public Point Offset;

        public bool IsWall; //Should use coordinate efficiency
        public bool IsPhysical;


        public CollisionComponent(CollisionBehavior behavior = CollisionBehavior.Block, AABB collisionRectangle = new AABB(), Point offset = new Point(), bool isWall = false, bool isPhysical = false)
        {
            Behavior = behavior;
            SetIsPhysical();
            CollisionRectangle = collisionRectangle;
            Offset = offset;
            IsWall = isWall;
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