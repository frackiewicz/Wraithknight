using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace Wraithknight
{
    //TODO How will you align sprites?
    class DrawSystem : System
    {
        private List<DrawComponent> _components = new List<DrawComponent>();

        public override void RegisterComponents(ICollection<Entity> entities)
        {
            CoupleComponents(_components, entities);
        }

        public override void Update(GameTime gameTime)  
        {
            foreach (var component in _components)
            {
                //align?
            }
        }

        public void Draw()
        {
            foreach (var drawComponent in _components)
            {
                if (drawComponent.active)
                {
                    Functions_Draw.Draw(drawComponent);
                }
            }
        }

        public override void ResetSystem()
        {
            _components = null; // components remain in entities
            //BAD IDEA?
        }
    }
}
