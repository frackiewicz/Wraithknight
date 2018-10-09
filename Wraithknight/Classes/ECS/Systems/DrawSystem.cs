using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wraithknight
{
    class DrawSystem : System
    {
        private List<DrawComponent> _components = new List<DrawComponent>();


        public override void RegisterComponents(ICollection<Entity> entities)
        {
            CoupleComponents(_components, entities);
        }

        public override void Update()
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
                Functions_Draw.Draw(drawComponent);
            }
        }

        public override void ResetSystem()
        {
            _components = null; // components remain in entities
            //BAD IDEA?
        }
    }
}
