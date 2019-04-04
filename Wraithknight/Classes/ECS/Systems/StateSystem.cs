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
                if (stateComponent.Inactive) continue;

                SetPreviousState(stateComponent);
                ClearCurrentState(stateComponent);
                if((stateComponent.Dead && stateComponent.CurrentState != EntityState.Dying) || stateComponent.ReadyToRemove) KillEntity(stateComponent); //TODO Statecheck is a bandaid FUCKINGHELL RESOLVE THIS
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

        private static void ClearCurrentState(StateComponent component)
        {
            if (component.CurrentState == EntityState.Dying || component.CurrentState == EntityState.Dead) return;
            component.Clear();
        }

        private void KillEntity(StateComponent component)
        {
            _ecs.KillEntity(component.RootID);
        }
    }
}
