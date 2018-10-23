using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wraithknight
{
    public class Pool //TODO this
    // newsflash: oh shit
    {
        private int _activeActors;
        public readonly List<Actor> PseudoActorPool; //push created actors here, but dont remove them until cleanup


        public Pool()
        {
            PseudoActorPool = new List<Actor>();
        }

        public void Update()
        {
            foreach (Actor actor in PseudoActorPool)
            {
            }
            //check for AI here
        }

        public void Draw()
        {
            foreach (Actor actor in PseudoActorPool)
            {
                Functions_Draw.Draw(actor);
            }
        }



        public void TempTestingEnvironment()
        {
            //Set a few units for testing.
        }

        public void FillActorPool(int i)
        {
            while (i > 0)
            {
                PseudoActorPool.Add(new Actor());
                i--;
            }
        }

        private void HandleAi(Actor actor)
        {
            ComponentIntelligence ai = actor.Intelligence;
            AiType type = ai.AiType;
        }
    }
}
