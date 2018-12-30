using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;

namespace Wraithknight
{
    class Animation
    {
        public List<AnimationFrame> Frames = new List<AnimationFrame>();
        public bool Looping;
    }

    class AnimationFrame
    {
        public double DisplayTimeMilliseconds;
        public Texture2D Texture;
    }

    class AnimationComponent : BindableComponent
    {

    }
}
