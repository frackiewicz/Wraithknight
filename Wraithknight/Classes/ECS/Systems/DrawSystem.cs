using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace Wraithknight
{
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

        public DrawSystem(ECS ecs) : base(ecs)
        {
            _ecs = ecs;
        }

        public override void RegisterComponents(ICollection<Entity> entities) //modified version of CoupleComponent to allow pairing //Ugly as fuck
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
                Functions_Draw.Draw(component);
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
            foreach (var pair in _moveableDrawComponents) // TODO Improvements possible
            {
                pair.Draw.DrawRec.X = (int)pair.Move.Position.X;
                pair.Draw.DrawRec.Y = (int)pair.Move.Position.Y;
            }
        }

        public override void Reset()
        {
            _moveableDrawComponents.Clear();
            _drawComponents.Clear();
        }

        
    }
}
