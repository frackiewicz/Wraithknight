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
        public EntityState Trigger;
        public String Identifier;
        public String NextAnimationIdentifier;
        public List<AnimationFrame> Frames = new List<AnimationFrame>();
        public Texture2D SpriteSheet;
        public Point FrameSize;
        public double StartTimeMilliseconds = -1;
        public double TotalAnimationDurationMilliseconds; //TODO substract the delta above threshold to next animation's starttime to above time clipping
        public bool AllowMirroring;

        public bool Looping;

        public Animation(Texture2D spriteSheet, String identifier, String nextAnimationIdentifier, Point frameSize, EntityState trigger = EntityState.None, bool allowMirroring = true)
        {
            Trigger = trigger;
            SpriteSheet = spriteSheet;
            Identifier = identifier;
            NextAnimationIdentifier = nextAnimationIdentifier;
            Looping = Identifier.Equals(NextAnimationIdentifier);
            FrameSize = frameSize;
            AllowMirroring = allowMirroring;
        }

        public bool Finished(GameTime currentTime)
        {
            return StartTimeMilliseconds + TotalAnimationDurationMilliseconds <= currentTime.TotalGameTime.TotalMilliseconds;
        }

        public AnimationFrame GetAnimationFrame(GameTime currentTime) // This feels wrong
        {
            double elapsedMilliseconds = currentTime.TotalGameTime.TotalMilliseconds - StartTimeMilliseconds;
            foreach (var animationFrame in Frames)
            {
                elapsedMilliseconds -= animationFrame.DisplayTimeMilliseconds;
                if (elapsedMilliseconds <= 0) return animationFrame;
            }

            return Frames[Frames.Count-1];
        }

        public Rectangle GetSourceRec(GameTime currentTime)
        {
            AnimationFrame currentFrame = GetAnimationFrame(currentTime);
            return new Rectangle(
                FrameSize.X * currentFrame.FramePosInSheet.X,
                FrameSize.Y * currentFrame.FramePosInSheet.Y,
                FrameSize.X,
                FrameSize.Y);
        }

        public Animation CreateAnimationFrames(int sheetRow, int numberOfFrames, double displayTime, int startColumn = 0)
        {
            List<AnimationFrame> frames = Frames;
            for (int i = startColumn; i < numberOfFrames; i++)
            {
                frames.Add(new AnimationFrame(new Point(i, sheetRow), displayTime));
                TotalAnimationDurationMilliseconds += displayTime;
            }
            
            return this;
        }
    }

    class AnimationFrame
    {
        public Point FramePosInSheet;
        public double DisplayTimeMilliseconds;

        public AnimationFrame(Point framePosInSheet, double displayTimeMilliseconds)
        {
            FramePosInSheet = framePosInSheet;
            DisplayTimeMilliseconds = displayTimeMilliseconds;
        }
    }

    /*
     * Maybe have a singular Spritesheet for any Entity
     * That way you can keep the batching logic you did before
     */
    class AnimationComponent : BindableComponent 
    {
        public List<Animation> Animations = new List<Animation>();
        public Animation CurrentAnimation;
        public DrawComponent BoundDrawComponent;

        public AnimationComponent(List<Animation> animations)
        {
            Animations = animations;
        }

        public override void Activate()
        {
            if (Bindings.TryGetValue(typeof(DrawComponent), out var binding))
            {
                DrawComponent drawComponent = binding as DrawComponent;
                BoundDrawComponent = drawComponent;
                drawComponent.IsAnimated = true;
            }
            CurrentAnimation = Animations.Find(a => a.Trigger == EntityState.Idle);
            base.Activate();
        }
    }
}
