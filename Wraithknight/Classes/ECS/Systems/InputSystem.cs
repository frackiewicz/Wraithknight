using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Wraithknight
{
    class InputSystem : System
    {
        private HashSet<InputComponent> _components = new HashSet<InputComponent>();
        private Camera2D _camera;

        public InputSystem(ECS ecs, Camera2D camera) : base(ecs)
        {
            _ecs = ecs;
            _camera = camera;
        }
        public override void RegisterComponents(ICollection<Entity> entities)
        {
            CoupleComponent(_components, entities);
        }

        public override void Update(GameTime gameTime)
        {
            foreach (var component in _components) //TODO cleanup?
            {
                if(component.Inactive) continue;
                if (component.UserInput)
                {
                    ReadInput(component);
                }
            }
        }

        public override void Reset()
        {
            _components.Clear();
        }

        private void ReadInput(InputComponent component)
        {
            component.MovementDirection.X = 0;
            component.MovementDirection.Y = 0;
            if (InputReader.IsKeyPressed(Keys.W))
            {
                component.MovementDirection.Y = -1;
            }
            if (InputReader.IsKeyPressed(Keys.A))
            {
                component.MovementDirection.X = -1;
            }
            if (InputReader.IsKeyPressed(Keys.S))
            {
                component.MovementDirection.Y = 1;
            }
            if (InputReader.IsKeyPressed(Keys.D))
            {
                component.MovementDirection.X = 1;
            }

            component.PrimaryAttack = InputReader.IsMouseButtonTriggered(MouseButtons.LMB);
            component.SecondaryAttack = InputReader.IsMouseButtonTriggered(MouseButtons.RMB);
            component.SwitchWeapons = InputReader.IsKeyTriggered(Keys.Space);
            component.Action = InputReader.IsKeyTriggered(Keys.F);
            component.Blink = InputReader.IsKeyPressed(Keys.LeftShift);
            component.CursorPoint = _camera.ConvertScreenToWorld(InputReader.CurrentCursorPos);
        }
    }
}
