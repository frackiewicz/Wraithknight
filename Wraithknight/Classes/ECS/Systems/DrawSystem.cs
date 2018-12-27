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
        private struct Pair //TODO REMOVE THIS
        {
            public readonly DrawComponent Draw;
            public readonly MovementComponent Move;

            public Pair(DrawComponent draw, MovementComponent move)
            {
                Draw = draw;
                Move = move;
            }
        }

        private readonly Dictionary<Texture2D, HashSet<DrawComponent>> _sortedDrawComponents = new Dictionary<Texture2D, HashSet<DrawComponent>>(); //to avoid texture swapping, hows the impact on movementbinding?
        private readonly HashSet<Pair> _moveableDrawComponents = new HashSet<Pair>();
        private readonly Camera2D _camera;

        public DrawSystem(ECS ecs, Camera2D camera) : base(ecs)
        {
            _ecs = ecs;
            _camera = camera;
        }

        public override void RegisterComponents(ICollection<Entity> entities)
        {
            CoupleDrawComponents(entities);
            BindMovementComponents();
        }

        public override void Update(GameTime gameTime)  
        {

        }

        public void Draw()
        {
            AlignAllPairs(); 
            foreach (var batch in _sortedDrawComponents.Values)
            {
                foreach (var component in batch)
                {
                    if (component.Inactive) continue;
                    if (component.DrawRec.Intersects(_camera.CullRec)) //isVisible
                    {
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
                    component.Activate(); //do you want this?
                }
            }
        }

        private void BindMovementComponents()
        {
            Component bind;
            foreach (var batch in _sortedDrawComponents.Values)
            {
                foreach (var component in batch)
                {
                    if (component.Bindings.TryGetValue(typeof(MovementComponent), out bind))
                    {
                        _moveableDrawComponents.Add(new Pair(component, bind as MovementComponent));
                    }
                }
            }
            
        }

        private void AlignAllPairs()
        {
            foreach (var pair in _moveableDrawComponents) 
            {
                pair.Draw.DrawRec.Center = pair.Move.Position.Vector2 + pair.Draw.Offset; //TODO BREUNIG Calculations with Vector2ref change the source
            }
        }

        public override void Reset()
        {
            _moveableDrawComponents.Clear();
            _sortedDrawComponents.Clear();
        }

        
    }
}
