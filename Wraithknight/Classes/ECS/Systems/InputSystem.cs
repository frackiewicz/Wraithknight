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
            foreach (var component in _components)
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
            else if (InputReader.IsKeyPressed(Keys.S))
            {
                component.MovementDirection.Y = 1;
            }

            if (InputReader.IsKeyPressed(Keys.A))
            {
                component.MovementDirection.X = -1;
            }
            else if (InputReader.IsKeyPressed(Keys.D))
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

        private void AttackLogic(InputComponent input, GameTime gameTime) //TODO Breunig, maybe move all this down into another class? I dont like how cluttered all this is
        {
            if (input.Bindings.TryGetValue(typeof(AttackBehaviorComponent), out var attackBehaviorBinding))
            {
                AttackBehaviorComponent attackBehavior = attackBehaviorBinding as AttackBehaviorComponent;

                if (HasDelayedAttack(attackBehavior))
                {
                    input.MovementDirection = Vector2.Zero;
                    CountdownRemainingAttackDelay(attackBehavior, gameTime);
                    TrySpawnDelayedAttack(input, attackBehavior, gameTime);
                    return;
                }
                if (IsInAttackCooldown(attackBehavior))
                {
                    CountdownRemainingAttackCooldown(attackBehavior, gameTime);
                }

                if (!(input.PrimaryAttack || input.SecondaryAttack)) return;
                if (attackBehavior.RemainingAttackCooldownMilliseconds > 0) return;

                FindAndExecuteAttack(input, attackBehavior, gameTime);
            }
        }

        private bool HasDelayedAttack(AttackBehaviorComponent attackBehavior)
        {
            return attackBehavior.DelayedAttack != null;
        }

        private void CountdownRemainingAttackDelay(AttackBehaviorComponent attackBehavior, GameTime gameTime)
        {
            attackBehavior.DelayedAttack.RemainingAttackDelayMilliseconds -= gameTime.ElapsedGameTime.TotalMilliseconds;
        }

        private void TrySpawnDelayedAttack(InputComponent input, AttackBehaviorComponent attackBehavior, GameTime gameTime)
        {
            if (attackBehavior.DelayedAttack.RemainingAttackDelayMilliseconds <= 0)
            {
                SpawnAttack(attackBehavior.DelayedAttack.Cursor, attackBehavior, attackBehavior.DelayedAttack.Attack, gameTime);
            }
        }

        private bool IsInAttackCooldown(AttackBehaviorComponent attackBehavior)
        {
            return attackBehavior.RemainingAttackCooldownMilliseconds > 0;
        }

        private void CountdownRemainingAttackCooldown(AttackBehaviorComponent attackBehavior, GameTime gameTime)
        {
            attackBehavior.RemainingAttackCooldownMilliseconds -= gameTime.ElapsedGameTime.TotalMilliseconds;

        }



        private void FindAndExecuteAttack(InputComponent input , AttackBehaviorComponent attackBehavior, GameTime gameTime)
        {
            foreach (var attack in attackBehavior.AttackComponents)
            {
                if (AttackTriggered(input, attack)) // && same attackState 
                {
                    ExecuteAttack(input, attackBehavior, attack, gameTime);
                    break;
                }
            }
        }

        private bool AttackTriggered(InputComponent input, AttackComponent attack)
        {
            return input.PrimaryAttack && attack.Type == AttackType.Primary || input.SecondaryAttack && attack.Type == AttackType.Secondary;
        }

        private void ExecuteAttack(InputComponent input, AttackBehaviorComponent attackBehavior, AttackComponent attack, GameTime gameTime)
        {
            if (attack.AttackDelayMilliseconds > 0)
            {
                attackBehavior.DelayedAttack = new AttackBehaviorComponent.DelayedAttackClass(attack.AttackDelayMilliseconds, attack, input.CursorPoint);
            }
            else SpawnAttack(input.CursorPoint, attackBehavior, attack, gameTime);
        }

        private void SpawnAttack(Point cursor, AttackBehaviorComponent attackBehavior, AttackComponent attack, GameTime gameTime)
        {
            Vector2 cursorDelta = new Vector2(cursor.X, cursor.Y) - attack.SourcePos.Vector2;

            _ecs.RegisterEntity(_ecs.CreateEntity(attack.Projectile, 
                position: new Vector2Ref(attack.SourcePos.Vector2 + new Coord2(cursorDelta).ChangePolarLength(attack.PosOffsetInDirection).Cartesian), 
                speed: new Coord2(new Vector2(cursor.X - attack.SourcePos.X, cursor.Y - attack.SourcePos.Y)).ChangePolarLength(attack.StartSpeed), 
                gameTime: gameTime, allegiance: attack.Allegiance));
            attackBehavior.RemainingAttackCooldownMilliseconds = attack.AttackCooldownMilliseconds;
            attackBehavior.DelayedAttack = null;
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