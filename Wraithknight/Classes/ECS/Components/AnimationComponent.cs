using System;
using System.Collections.Generic;
using System.Linq;
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
        public double StartTimeMilliseconds;
        
        public bool Looping;

        public AnimationFrame GetAnimationFrame(GameTime currentTime)
        {
            double elapsedMilliseconds = currentTime.TotalGameTime.TotalMilliseconds - StartTimeMilliseconds;

            foreach (var animationFrame in Frames)
            {
                elapsedMilliseconds -= animationFrame.DisplayTimeMilliseconds;
                if (elapsedMilliseconds <= 0) return animationFrame;
            }

            return Frames[Frames.Count-1];
        }
    }

    class AnimationFrame
    {
        public double DisplayTimeMilliseconds;
        public Texture2D Texture;
    }

    class AnimationComponent : Component
    {
        public List<Animation> Animations = new List<Animation>();

    }
}
