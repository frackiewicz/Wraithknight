using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace Wraithknight
{
    class AnimationSystem : System
    {
        private List<AnimationComponent> _animationComponents = new List<AnimationComponent>();

        public override void RegisterComponents(ICollection<Entity> entities)
        {
            foreach (var entity in entities)
            {
                CoupleComponent(_animationComponents, entity);                
            }
        }

        public override void Update(GameTime gameTime)
        {
            foreach (var component in _animationComponents)
            {
                if (component.Inactive) continue;
                if (component.CurrentEntityState.RecentlyChanged)
                {
                    StartAnimation(component, component.CurrentEntityState.CurrentState, gameTime);
                }
                if (component.CurrentAnimation == null) return;

                ProcessAnimation(component, gameTime);

                if (component.CurrentAnimation.AllowMirroring) ApplyMirroring(component); else ResetMirroring(component);
            }
        }

        public override void Reset()
        {
            _animationComponents.Clear();
        }

        private static void StartAnimation(AnimationComponent component, EntityState trigger, GameTime gameTime) //for now just randomize it
        {
            List<Animation> animations = component.Animations.FindAll(a => a.Trigger == trigger);
            if (animations.Count == 0) return;

            Random random = new Random();
            component.CurrentAnimation = animations[random.Next(0, animations.Count - 1)];
            component.CurrentAnimation.StartTimeMilliseconds = gameTime.TotalGameTime.TotalMilliseconds;
            component.BoundDrawComponent.Texture = component.CurrentAnimation.SpriteSheet;
        }
        private static void StartAnimation(AnimationComponent component, String identifier, GameTime gameTime)
        {
            component.CurrentAnimation = component.Animations.Find(a => a.Identifier.Equals(identifier));
            component.CurrentAnimation.StartTimeMilliseconds = gameTime.TotalGameTime.TotalMilliseconds;
            component.BoundDrawComponent.Texture = component.CurrentAnimation.SpriteSheet;
        }
        private static void ReplayAnimation(AnimationComponent component, GameTime gameTime)
        {
            component.CurrentAnimation.StartTimeMilliseconds = gameTime.TotalGameTime.TotalMilliseconds;
        }

        private static void ProcessAnimation(AnimationComponent component, GameTime gameTime)
        {
            if (component.CurrentAnimation.Finished(gameTime))
            {
                if (component.CurrentAnimation.Trigger == EntityState.Dying)
                {
                    component.CurrentEntityState.Dead = true;
                    return;
                }
                if (component.CurrentAnimation.Looping) ReplayAnimation(component, gameTime);
                else StartAnimation(component, component.CurrentAnimation.NextAnimationIdentifier, gameTime);
            }

            ApplyAnimationRec(component, gameTime);
        }

        private static void ApplyAnimationRec(AnimationComponent component, GameTime gameTime)
        {
            component.BoundDrawComponent.SourceRec = component.CurrentAnimation.GetSourceRec(gameTime);
        }

        private static void ApplyMirroring(AnimationComponent component)
        {
            if (component.CurrentEntityState.Orientation == Direction.Left) component.BoundDrawComponent.FlipHorizontally = true;
            else component.BoundDrawComponent.FlipHorizontally = false;
        }

        private static void ResetMirroring(AnimationComponent component)
        {
            component.BoundDrawComponent.FlipHorizontally = false;
        }


    }
}
