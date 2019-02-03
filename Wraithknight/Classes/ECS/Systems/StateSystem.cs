using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace Wraithknight
{ 
    class StateSystem : System
    {
        private readonly ECS _ecs;
        private List<StateComponent> _components = new List<StateComponent>();

        public StateSystem(ECS ecs)
        {
            _ecs = ecs;
        }
        
        public override void RegisterComponents(ICollection<Entity> entities)
        {
            CoupleComponent(_components, entities);
        }

        public override void Update(GameTime gameTime)
        {
            foreach (var stateComponent in _components)
            {
                SetPreviousState(stateComponent);
                KillIfDead(stateComponent);
            }
        }

        public override void Reset()
        {
            _components.Clear();
        }

        private void SetPreviousState(StateComponent component) //gets called when all computation is finished and it is preparing for the next game update
        {
            component.RecentlyChanged = component.PreviousState != component.CurrentState;
            component.PreviousState = component.CurrentState;
        }

        private void KillIfDead(StateComponent component)
        {
            if(component.Dead) _ecs.KillEntity(component.RootID);
        }
    }
}
