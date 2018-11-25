using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

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
        private struct Pair
        {
            public readonly DrawComponent Draw;
            public readonly MovementComponent Move;

            public Pair(DrawComponent draw, MovementComponent move)
            {
                Draw = draw;
                Move = move;
            }
        }

        private readonly HashSet<DrawComponent> _drawComponents = new HashSet<DrawComponent>();
        private readonly HashSet<Pair> _moveableDrawComponents = new HashSet<Pair>(); //TODO Breunig HashSet way more efficient than list, why?
        private readonly Camera2D _camera;

        public DrawSystem(ECS ecs, Camera2D camera) : base(ecs)
        {
            _ecs = ecs;
            _camera = camera;
        }

        public override void RegisterComponents(ICollection<Entity> entities)
        {
            CoupleComponent(_drawComponents, entities);
            BindMovementComponents();
        }

        public override void Update(GameTime gameTime)  
        {

        }

        public void Draw()
        {
            AlignAllPairs();
            foreach (var component in _drawComponents)
            {
                if (component.Inactive) continue;
                if (component.DrawRec.Intersects(_camera.CullRec)) //isVisible
                {
                    Functions_Draw.Draw(component);
                }
            }
        }

        private void BindMovementComponents()
        {
            Component bind;
            foreach (var component in _drawComponents)
            {
                if (component.Bindings.TryGetValue(typeof(MovementComponent), out bind))
                {
                    _moveableDrawComponents.Add(new Pair(component, bind as MovementComponent));
                }
            }
        }

        private void AlignAllPairs()
        {
            foreach (var pair in _moveableDrawComponents) 
            {
                pair.Draw.DrawRec.X = pair.Move.Position.X - pair.Draw.DrawRec.Width / 2 + pair.Draw.Offset.X;
                pair.Draw.DrawRec.Y = pair.Move.Position.Y - pair.Draw.DrawRec.Height / 2 + pair.Draw.Offset.Y;

            }
        }

        public override void Reset()
        {
            _moveableDrawComponents.Clear();
            _drawComponents.Clear();
        }

        
    }
}
