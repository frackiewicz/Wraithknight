using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Wraithknight.Classes.Functions;

namespace Wraithknight.Classes.ECS.Systems
{
    class HeroControlSystem : System
    {
        public Entity Hero;

        public override void RegisterComponents(ICollection<Entity> entities)
        {
            foreach (Entity entity in entities)
            {
                if (entity.Type == EntityType.Hero)
                {
                    if (Hero != null)
                    {
                        Functions_ConsoleDebugging.DumpEntityInfo(entity);
                        Console.WriteLine("Multiple Heroes in Controller. Overwriting");
                    }

                    Hero = entity;
                }
            }
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