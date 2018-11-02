using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace Wraithknight
{
    class Actor// break inheritance?
    //newsflash: GET THE FUCK OUTTA HERE
    {
        public Boolean Active;

        public DrawComponent Sprite;
        public ComponentMovement Movement;
        public ComponentCollision Collision;
        public ComponentIntelligence Intelligence;

        public bool Enemy = true;


        public Actor()
        {
            Sprite = new DrawComponent();
            Movement = new ComponentMovement();
            Collision = new ComponentCollision();
            Intelligence = new ComponentIntelligence();
        }


        public virtual void MoveActor(Vector2 vector) //Move higher for collision?
        {
            Movement.Position += vector;
            Sprite.DrawRec.X = (int)(Movement.Position.X + Sprite.Offset.X);
            Sprite.DrawRec.Y = (int)(Movement.Position.Y + Sprite.Offset.Y);
            Collision.CollisionRec.X = (int)Movement.Position.X + Collision.offsetX;
            Collision.CollisionRec.Y = (int)Movement.Position.Y + Collision.offsetY;
            Collision.CollisionCenter.X = Collision.CollisionRec.X + Collision.CollisionRec.Width / 2;
            Collision.CollisionCenter.Y = Collision.CollisionRec.Y + Collision.CollisionRec.Height / 2;
        }
    }
}
