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
                SpawnAttack(input, attackBehavior.DelayedAttack.Cursor, attackBehavior, attackBehavior.DelayedAttack.Attack, gameTime);
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

        private bool AttackTriggered(InputComponent input, AttackComponent attack)
        {
            return input.PrimaryAttack && attack.Type == AttackType.Primary || input.SecondaryAttack && attack.Type == AttackType.Secondary;
        }

        private void StartAttack(InputComponent input, AttackBehaviorComponent attackBehavior, AttackComponent attack, GameTime gameTime)
        {
            if (attack.AttackDelayMilliseconds > 0)
            {
                attackBehavior.DelayedAttack = new AttackBehaviorComponent.DelayedAttackClass(attack.AttackDelayMilliseconds, attack, input.CursorPoint);
                if (attack.BlockInput) BlockInput(input, gameTime, attack.BlockInputDurationMilliseconds);
            }
            else SpawnAttack(input, input.CursorPoint, attackBehavior, attack, gameTime);
        }

        private void SpawnAttack(InputComponent input, Point cursor, AttackBehaviorComponent attackBehavior, AttackComponent attack, GameTime gameTime)
        {
            Vector2 cursorDelta = new Vector2(cursor.X, cursor.Y) - attack.SourcePos.Vector2;

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
