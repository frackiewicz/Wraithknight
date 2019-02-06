using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace Wraithknight
{
    class InputAttackSubsystem
    {
        private readonly ECS _ecs;

        public InputAttackSubsystem(ECS ecs)
        {
            _ecs = ecs;
        }

        public void AttackLogic(InputComponent input, GameTime gameTime)
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

                FindAndStartTriggeredAttack(input, attackBehavior, gameTime);
                SetAttackStates(attackBehavior);
            }
        }

        #region Delay

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
                SpawnAttackProjectile(input, attackBehavior.DelayedAttack.Cursor, attackBehavior, attackBehavior.DelayedAttack.Attack, gameTime);
            }
        }

        #endregion

        #region Cooldown

        private bool IsInAttackCooldown(AttackBehaviorComponent attackBehavior)
        {
            return attackBehavior.RemainingAttackCooldownMilliseconds > 0;
        }

        private void CountdownRemainingAttackCooldown(AttackBehaviorComponent attackBehavior, GameTime gameTime)
        {
            attackBehavior.RemainingAttackCooldownMilliseconds -= gameTime.ElapsedGameTime.TotalMilliseconds;

        }

        #endregion

        private void FindAndStartTriggeredAttack(InputComponent input, AttackBehaviorComponent attackBehavior, GameTime gameTime)
        {
            foreach (var attack in attackBehavior.AttackComponents)
            {
                if (AttackTriggered(input, attack)) // && same attackState 
                {
                    StartAttack(input, attackBehavior, attack, gameTime);
                    break;
                }
            }
        }

        private static bool AttackTriggered(InputComponent input, AttackComponent attack)
        {
            return input.PrimaryAttack && attack.Type == AttackType.Primary || input.SecondaryAttack && attack.Type == AttackType.Secondary;
        }

        private void StartAttack(InputComponent input, AttackBehaviorComponent attackBehavior, AttackComponent attack, GameTime gameTime)
        {
            PlaceCursorInBehavior(input, attackBehavior, attack);

            if (attack.IsDelayedAttack)
            {
                attackBehavior.DelayedAttack = new AttackBehaviorComponent.DelayedAttackClass(attack.AttackDelayMilliseconds, attack, attackBehavior.Cursor);
                if (attack.BlockInput) BlockInput(input, gameTime, attack.BlockInputDurationMilliseconds);
            }
            else SpawnAttackProjectile(input, attackBehavior.Cursor, attackBehavior, attack, gameTime);
        }

        private void PlaceCursorInBehavior(InputComponent input, AttackBehaviorComponent attackBehavior, AttackComponent attack)
        {
            if (attack.CursorType == CursorType.Absolute)
            {
                attackBehavior.Cursor = input.CursorPoint;
            }
            else //Relative
            {
                Point cursor = input.CursorPoint;
                Point source = new Point((int)attack.SourcePos.Vector2.X, (int)attack.SourcePos.Vector2.Y);
                Point delta = cursor - source;
                attackBehavior.Cursor = delta;
            }
        }

        private void SpawnAttackProjectile(InputComponent input, Point cursor, AttackBehaviorComponent attackBehavior, AttackComponent attack, GameTime gameTime)
        {
            Vector2 cursorDelta;
            if (attack.CursorType == CursorType.Absolute)
            {
                cursorDelta = new Vector2(cursor.X, cursor.Y) - attack.SourcePos.Vector2;
            }
            else //Relative
            {
                cursorDelta = new Vector2(cursor.X, cursor.Y);
            }
            _ecs.RegisterEntity(_ecs.CreateEntity(attack.Projectile,
                position: new Vector2Ref(attack.SourcePos.Vector2 + new Coord2(cursorDelta).ChangePolarLength(attack.PosOffsetInDirection).Cartesian),
                speed: new Coord2(cursorDelta).ChangePolarLength(attack.StartSpeed),
                gameTime: gameTime, allegiance: attack.Allegiance));


            attackBehavior.RemainingAttackCooldownMilliseconds = attack.AttackCooldownMilliseconds;

            if (attack.BlockInput) BlockInput(input, gameTime, attack.BlockInputDurationMilliseconds);
            if (attack.SelfKnockback != 0) ApplySelfKnockback(input, attack.SelfKnockback, cursorDelta);

            attackBehavior.DelayedAttack = null;
        }

        private void BlockInput(InputComponent input, GameTime gameTime, double milliseconds)
        {
            input.Blocked = true;
            input.BlockedTimer.SetTimer(gameTime, milliseconds);
        }

        private void SetAttackStates(AttackBehaviorComponent attack)
        {
            if (attack.CurrentEntityState == null || !attack.CurrentEntityState.ReadyToChange) return;
            StateComponent stateComponent = attack.CurrentEntityState;

            if (attack.RemainingAttackCooldownMilliseconds > 0 || (attack.DelayedAttack != null && attack.DelayedAttack.RemainingAttackDelayMilliseconds > 0))
            {
                if (stateComponent.CurrentStatePriority < 2)
                {
                    stateComponent.CurrentState = EntityState.Attacking;
                }

                if (stateComponent.CurrentState == EntityState.Attacking)
                {
                    double PI = Math.PI;
                    Vector2 cursorDelta = new Vector2(attack.Cursor.X, attack.Cursor.Y) - attack.SourcePos.Vector2;
                    double Angle = new Coord2(cursorDelta).Polar.Angle;

                    if (Angle < PI / 2 && Angle > -PI / 2) stateComponent.Orientation = Direction.Right;
                    if (Angle >= PI / 2 || Angle <= -PI / 2) stateComponent.Orientation = Direction.Left;
                }
            }
            else
            {
                if(stateComponent.CurrentState == EntityState.Attacking) stateComponent.Clear();
            }
            /*
             *if(movement.CurrentEntityState == null || !movement.CurrentEntityState.ReadyToChange) return;
            StateComponent stateComponent = movement.CurrentEntityState;
            if (movement.IsMoving)
            {
                if (stateComponent.CurrentStatePriority < 1)
                {
                    stateComponent.CurrentState = EntityState.Moving;
                }

                if (stateComponent.CurrentState == EntityState.Moving)
                {
                    double PI = Math.PI;
                    double Angle = movement.Speed.Polar.Angle;

                    if (Angle < PI / 2 && Angle > -PI / 2) stateComponent.Orientation = Direction.Right;
                    if (Angle >= PI / 2 || Angle <= -PI / 2) stateComponent.Orientation = Direction.Left;

                    
                    if (Angle <= PI / 4 && Angle >= -PI / 4) stateComponent.Direction = Direction.Right;
                    if (Angle > PI / 4 && Angle < 3 * PI / 4) stateComponent.Direction = Direction.Down;
                    if (Angle < -PI / 4 && Angle > 3 * -PI / 4) stateComponent.Direction = Direction.Up;
                    if (Angle >= 3 * PI / 4 || Angle <= 3 * -PI / 4) stateComponent.Direction = Direction.Left;
                    
                }
            }
            else
            {
                if (stateComponent.CurrentState == EntityState.Moving)
                {
                    stateComponent.CurrentState = EntityState.Idle;
                }
            }
             */
        }

        private void ApplySelfKnockback(InputComponent input, int knockback, Vector2 cursorDelta)
        {
            if (input.Bindings.TryGetValue(typeof(MovementComponent), out var binding))
            {
                MovementComponent movement = binding as MovementComponent;
                movement.Speed.SetVector2(new Coord2(-cursorDelta).ChangePolarLength(knockback).Cartesian);
            }
        }
    }
}
