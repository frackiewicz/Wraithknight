using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace Wraithknight
{
    class InputSystem : System
    {
        private List<InputComponent> _components = new List<InputComponent>();

        public override void RegisterComponents(ICollection<Entity> entities)
        {
            base.CoupleComponents(_components, entities);
        }

        public override void Update(GameTime gameTime)
        {
            foreach (var component in _components)
            {
                InputReader.UpdateInputComponent(component);
            }
        }

        public override void ResetSystem()
        {
            throw new NotImplementedException();
        }


    }
}
