using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Wraithknight
{
    /*
     * Idea:
     * To avoid AlignAll Iteration
     *
     * make Pair into a class
     * MovementComponent defaultvalue is null
     * if MovementComponent is set, Align
     * then Draw
     */

    /*
     * Idea:
     * Figure out a System to avoid Texture swapping
     * Maybe a 2D drawComponents Collection where components with the same Texture are packed together
     */
    class DrawSystem : System
    {

        private readonly Dictionary<Texture2D, HashSet<DrawComponent>> _sortedDrawComponents = new Dictionary<Texture2D, HashSet<DrawComponent>>(); //to avoid texture swapping, hows the impact on movementbinding?
        private readonly Camera2D _camera;

        public DrawSystem(ECS ecs, Camera2D camera) : base(ecs)
        {
            _ecs = ecs;
            _camera = camera;
        }

        public override void RegisterComponents(ICollection<Entity> entities)
        {
            CoupleDrawComponents(entities);
        }

        public override void Update(GameTime gameTime)  
        {

        }

        public void Draw()
        {
            foreach (var batch in _sortedDrawComponents.Values)
            {
                foreach (var component in batch)
                {
                    if (component.Inactive) continue;
                    if (component.DrawRec.Intersects(_camera.CullRec)) //isVisible
                    {
                        UpdatePosition(component);
                        Functions_Draw.Draw(component);
                    }
                }
            }
        }

        private void CoupleDrawComponents(ICollection<Entity> entities)
        {
            foreach (var entity in entities)
            {
                if (entity.Components.TryGetValue(typeof(DrawComponent), out var component))
                {
                    DrawComponent drawComponent = component as DrawComponent;

                    if (!_sortedDrawComponents.ContainsKey(drawComponent.Texture)) _sortedDrawComponents.Add(drawComponent.Texture, new HashSet<DrawComponent>());
                    _sortedDrawComponents[drawComponent.Texture].Add(drawComponent);
                    component.Activate();
                }
            }
        }

        private void UpdatePosition(DrawComponent component)
        {
            if(component.SourcePos != null) component.DrawRec.Center = component.SourcePos + component.Offset;
        }

        public override void Reset()
        {
            _sortedDrawComponents.Clear();
        }

        
    }
}
