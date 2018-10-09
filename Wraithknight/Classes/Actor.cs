using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace Wraithknight
{
    public class Actor// break inheritance?
    //newsflash: GET THE FUCK OUTTA HERE
    {
        public ActorType ActorType;
        public Boolean Active;

        public DrawComponent Sprite;
        public ComponentMovement Movement;
        public ComponentCollision Collision;
        public ComponentIntelligence Intelligence;

        public bool Enemy = true;


        public Actor()
        {
            ActorType = ActorType.Empty;
            Sprite = new DrawComponent();
            Movement = new ComponentMovement();
            Collision = new ComponentCollision();
            Intelligence = new ComponentIntelligence();
        }

        public void SetActor(ActorType actorType, DrawComponent sprite = null, ComponentMovement movement = null,
            ComponentCollision collision = null, ComponentIntelligence intelligence = null)
        {
            ActorType = actorType;
            if (sprite != null) Sprite = sprite;
            if (movement != null) Movement = movement;
            if (collision != null) Collision = collision;
            if (intelligence != null) Intelligence = intelligence;
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
