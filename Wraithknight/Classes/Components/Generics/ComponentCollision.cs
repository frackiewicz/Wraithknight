using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace Wraithknight
{
    public class ComponentCollision
    { // 
        public Rectangle CollisionRec = new Rectangle(0, 0, 16, 16); //used in collision checking
        public int offsetX = 0; //offsets rec from sprite.position
        public int offsetY = 0; //offsets rec from sprite.position
        public Vector2 CollisionCenter;
        public Boolean blocking = true; //impassable or interactive


        //TODO really ugly, only meant to be temporary
        #region Intersects
        public Boolean Intersects(ComponentCollision otherComponentCollision)
        {
            return CollisionRec.Intersects(otherComponentCollision.CollisionRec);
        }
        public Boolean Intersects(Rectangle otherCollisionRectangle)
        {
            return CollisionRec.Intersects(otherCollisionRectangle);
        }
        #endregion

        public void block(Rectangle otherCollisionRec) //TODO LOGIC, REMOVE THIS
        {
            //Also check for intersects
            if (CollisionRec.X < otherCollisionRec.X) //isleft TODO center it
            {
                CollisionRec.X = otherCollisionRec.X - CollisionRec.Width;
            }

            if (CollisionRec.X >= otherCollisionRec.X) //isright replace with else?
            {
                CollisionRec.X = otherCollisionRec.X + otherCollisionRec.Width;
            }

            if (CollisionRec.Y < otherCollisionRec.Y) //isbelow
            {
                CollisionRec.Y = otherCollisionRec.Y - CollisionRec.Height;
            }

            if (CollisionRec.Y >= otherCollisionRec.Y) //isabove else?
            {
                CollisionRec.Y = otherCollisionRec.Y + otherCollisionRec.Height;
            }
        }
    }
}
