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
            public DrawComponent Draw;
            public MovementComponent Move;

            public Pair(DrawComponent draw, MovementComponent move)
            {
                Draw = draw;
                Move = move;
            }
        }
        private HashSet<DrawComponent> _drawComponents = new HashSet<DrawComponent>();
        private HashSet<Pair> _moveableDrawComponents = new HashSet<Pair>();

        public DrawSystem(ECS ecs) : base(ecs)
        {
            _ecs = ecs;
        }

        public override void RegisterComponents(ICollection<Entity> entities) //modified version of CoupleComponents to allow pairing //Ugly as fuck
        {
            foreach (var entity in entities)
            {
                IEnumerable<Component> drawComponents = entity.GetComponents<DrawComponent>();
                if (drawComponents != null)
                {
                    IEnumerable<Component> movementEnumerable = entity.GetComponents<MovementComponent>();
                    List<Component> movementComponents = null;
                    if (movementEnumerable != null)
                    {
                        movementComponents = movementEnumerable.ToList();
                    }
                    foreach (var drawComponent in drawComponents)
                    {
                        _drawComponents.Add(Functions_Operators.CastComponent<DrawComponent>(drawComponent));
                        drawComponent.Activate(); //do you want this?
                        if (movementComponents != null)
                        {
                            foreach (var movementComponent in movementComponents)
                            {
                                if (movementComponent.RootID.Equals(drawComponent.RootID))
                                    _moveableDrawComponents.Add(new Pair(Functions_Operators.CastComponent<DrawComponent>(drawComponent), Functions_Operators.CastComponent<MovementComponent>(movementComponent)));
                            }
                        }
                    }
                }
                else { Console.WriteLine("Entity-" + entity.ID + " lacks " + typeof(DrawComponent)); } // Output: Entity-0 lacks DrawComponent
            }
        }

        public override void Update(GameTime gameTime)  
        {

        }

        public void Draw()
        {
            AlignAllPairs();
            foreach (var drawComponent in _drawComponents)
            {
                if (!drawComponent.Active) continue;
                Functions_Draw.Draw(drawComponent);
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
