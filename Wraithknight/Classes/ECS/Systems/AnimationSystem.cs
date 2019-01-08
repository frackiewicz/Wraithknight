using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace Wraithknight.Classes.ECS.Systems
{
    class AnimationSystem : System
    {
        private List<AnimationComponent> _animationComponents = new List<AnimationComponent>();

        public AnimationSystem(Wraithknight.ECS ecs) : base(ecs)
        {

        }

        public override void RegisterComponents(ICollection<Entity> entities)
        {
            foreach (var entity in entities)
            {
                CoupleComponent(_animationComponents, entity);                
            }
        }

        public override void Update(GameTime gameTime)
        {
            
            throw new NotImplementedException();
        }

        public override void Reset()
        {
            throw new NotImplementedException();
        }

        private void StartAnimation(AnimationComponent component, String identifier, GameTime gameTime)
        {
            component.CurrentAnimation = component.Animations.Find(a => a.Identifier.Equals(identifier));
            component.CurrentAnimation.StartTimeMilliseconds = gameTime.TotalGameTime.TotalMilliseconds;
            
        }

        private void ApplyAnimationTexture(AnimationComponent component, GameTime gameTime)
        {
            component.BoundDrawComponent.Texture = component.CurrentAnimation.GetAnimationFrame(gameTime);
        }
    }
}
