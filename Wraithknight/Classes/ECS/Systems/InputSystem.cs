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
                if (component.Inactive) continue;
                if (component.UserInput) ReadInput(component);
                HandleInputLogic(component, gameTime);
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

            component.PrimaryAttack = InputReader.IsMouseButtonPressed(MouseButtons.LMB);
            component.SecondaryAttack = InputReader.IsMouseButtonPressed(MouseButtons.RMB);
            component.SwitchWeapons = InputReader.IsKeyTriggered(Keys.Space);
            component.Action = InputReader.IsKeyTriggered(Keys.F);
            component.Blink = InputReader.IsKeyPressed(Keys.LeftShift);
            component.CursorPoint = _camera.ConvertScreenToWorld(InputReader.CurrentCursorPos);
        }

        #region Logic

        private void HandleInputLogic(InputComponent component, GameTime gameTime)
        {
            MovementLogic(component);
            AttackLogic(component, gameTime);
        }

        #region Attack

        private void AttackLogic(InputComponent input, GameTime gameTime)
        {
            if ((input.PrimaryAttack || input.SecondaryAttack) && input.Bindings.TryGetValue(typeof(AttackBehaviorComponent), out var attackBehaviorBinding))
            {
                AttackBehaviorComponent attackBehavior = attackBehaviorBinding as AttackBehaviorComponent;
                attackBehavior.RemainingAttackCooldownMilliseconds -= gameTime.ElapsedGameTime.TotalMilliseconds;

                if (attackBehavior.RemainingAttackCooldownMilliseconds >= 0) return;
                foreach (var attack in attackBehavior.AttackComponents)
                {
                    if (AttackTriggered(input, attack)) // && same attackState 
                    {
                        ExecuteAttack(input, attackBehavior, attack, gameTime);
                        break;
                    }
                }
            }
        }

        private bool AttackTriggered(InputComponent input, AttackComponent attack)
        {
            return input.PrimaryAttack && attack.Type == AttackType.Primary || input.SecondaryAttack && attack.Type == AttackType.Secondary;
        }

        private void ExecuteAttack(InputComponent input, AttackBehaviorComponent attackBehavior, AttackComponent attack, GameTime gameTime)
        {
            _ecs.RegisterEntity(_ecs.CreateEntity(attack.Projectile, position: new Vector2Ref(attack.SourcePos), speed: new Coord2(new Vector2(input.CursorPoint.X - attack.SourcePos.X, input.CursorPoint.Y - attack.SourcePos.Y)).ChangePolarLength(attack.StartSpeed), gameTime: gameTime));
            attackBehavior.RemainingAttackCooldownMilliseconds = attack.AttackCooldownMilliseconds;
        }

        #endregion

        #region Movement

        private void MovementLogic(InputComponent input)
        {
            if (input.Bindings.TryGetValue(typeof(MovementComponent), out var binding))
            {
                MovementComponent movement = binding as MovementComponent;

                movement.Acceleration.X = input.MovementDirection.X * movement.AccelerationBase;
                movement.Acceleration.Y = input.MovementDirection.Y * movement.AccelerationBase;
            }
        }

        #endregion

        #endregion
    }
}