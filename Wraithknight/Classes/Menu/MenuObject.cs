using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace Wraithknight
{
    class MenuObject
    {
        public DrawComponent DrawComponent;

        public MenuObject(DrawComponent drawComponent)
        {
            DrawComponent = drawComponent;
        }

        public virtual void Draw()
        {
            Functions_Draw.Draw(DrawComponent);
        }
    }
}
