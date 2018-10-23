using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace Wraithknight
{
    //TODO How will you align sprites?
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
        private List<DrawComponent> _drawComponents = new List<DrawComponent>();
        private List<Pair> _moveableDrawComponents = new List<Pair>();

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
                        if(movementComponents != null) {
                            foreach (var movementComponent in movementComponents)
                            {
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
            Align();
            foreach (var drawComponent in _drawComponents)
            {
                if (drawComponent.active)
                {
                    Functions_Draw.Draw(drawComponent);
                }
            }
        }

        private void Align()
        {
            foreach (var pair in _moveableDrawComponents) // TODO Improvements possible
            {
                pair.Draw.DrawRec.X = (int)pair.Move.Position.X;
                pair.Draw.DrawRec.Y = (int)pair.Move.Position.Y;
            }
        }

        public override void ResetSystem()
        {
            throw new NotImplementedException();
        }
    }
}
