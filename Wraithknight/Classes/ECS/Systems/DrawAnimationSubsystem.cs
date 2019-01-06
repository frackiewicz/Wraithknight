using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace Wraithknight
{
    class DrawAnimationSubsystem
    {
        private List<AnimationComponent> animationComponents = new List<AnimationComponent>();


        public void RegisterAnimationComponent(AnimationComponent component)
        {
            animationComponents.Add(component);
        }

        public void Update(GameTime gameTime)
        {

        }

    }
}
