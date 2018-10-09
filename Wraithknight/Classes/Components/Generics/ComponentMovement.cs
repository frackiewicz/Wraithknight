using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Wraithknight
{
    public class ComponentMovement
    {
        public Direction Direction = Direction.Down; //havent figured out the use yet
        public Boolean Moving = false;

        public Vector2 Position = new Vector2(0, 0);
        public Vector2 NextPosition = new Vector2(0, 0);
        public Vector2 Speed = new Vector2(0, 0);

        public float Acceleration = 0.0f;
        public float Friction = 0.75f;
        
    }
}
