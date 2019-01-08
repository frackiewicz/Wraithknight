using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Wraithknight
{
    class Animation
    {
        public String Identifier;
        public String NextAnimationIdentifier;
        public List<AnimationFrame> Frames = new List<AnimationFrame>();
        public double StartTimeMilliseconds = -1;

        public bool AnimationFinished;
        public bool Looping;

        public AnimationFrame GetAnimationFrame(GameTime currentTime) // This feels wrong
        {
            if (StartTimeMilliseconds == -1) return null; //TODO can be removed later
            double elapsedMilliseconds = currentTime.TotalGameTime.TotalMilliseconds - StartTimeMilliseconds;

            foreach (var animationFrame in Frames)
            {
                elapsedMilliseconds -= animationFrame.DisplayTimeMilliseconds;
                if (elapsedMilliseconds <= 0) return animationFrame;
            }

            AnimationFinished = true;
            return Frames[Frames.Count-1];
        }
    }

    class AnimationFrame
    {
        public double DisplayTimeMilliseconds;
        public Texture2D Texture;

        public static implicit operator Texture2D(AnimationFrame obj)
        {
            return obj.Texture;
        }
    }

    class AnimationComponent : BindableComponent
    {
        public List<Animation> Animations = new List<Animation>();
        public Animation CurrentAnimation;
        public DrawComponent BoundDrawComponent;

        public override void Activate()
        {
            if (Bindings.TryGetValue(typeof(DrawComponent), out var binding))
            {
                DrawComponent drawComponent = binding as DrawComponent;
                BoundDrawComponent = drawComponent;
                drawComponent.IsAnimated = true;
            }
            base.Activate();
        }
    }
}
