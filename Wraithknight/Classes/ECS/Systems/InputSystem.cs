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
        private List<InputComponent> _components = new List<InputComponent>();
        private Camera2D _camera;

        public InputSystem(Camera2D camera)
        {
            _camera = camera;
        }
        public override void RegisterComponents(ICollection<Entity> entities)
        {
            base.CoupleComponents(_components, entities);
        }

        public override void Update(GameTime gameTime)
        {
            foreach (var component in _components) //TODO cleanup?
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

        public override void ResetSystem()
        {
            throw new NotImplementedException();
        }


    }
}
