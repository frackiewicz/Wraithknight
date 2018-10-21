using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Wraithknight.Classes.ECS.Components;

namespace Wraithknight.Classes.ECS.Systems
{
    class InputSystem : System
    {
        private List<InputComponent> _components = new List<InputComponent>();
        public readonly InputReader Input = new InputReader();

        public override void RegisterComponents(ICollection<Entity> entities)
        {
            base.CoupleComponents(_components, entities);
        }

        public override void Update(GameTime gameTime)
        {
            throw new NotImplementedException();
        }

        public override void ResetSystem()
        {
            throw new NotImplementedException();
        }


    }
}
