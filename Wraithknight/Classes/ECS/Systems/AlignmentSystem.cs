using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace Wraithknight.Classes.ECS.Systems
{
    class AlignmentSystem : System //Aligns Vector2s
    {
        public AlignmentSystem(Wraithknight.ECS ecs) : base(ecs)
        {
        }

        public override void RegisterComponents(ICollection<Entity> entities)
        {

        }

        public override void Update(GameTime gameTime)
        {
            throw new NotImplementedException();
        }

        public override void Reset()
        {
            throw new NotImplementedException();
        }
    }
}
