using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace Wraithknight
{
    public class Hero : Actor // The main character, controlled by the player
    {
        // From Actor
        // public readonly ComponentMovement Movement = new ComponentMovement();
        // public readonly DrawComponent Sprite = new DrawComponent();

        public Hero() : base()
        {
            
            Sprite = new DrawComponent(Assets.GetTexture("DummyTexture"));
            Sprite.ChangeSize(16,16);
        }


        public void Update()
        {

        }

        


    }
}
