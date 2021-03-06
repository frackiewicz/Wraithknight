﻿using System;
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
        private readonly ECS _ecs;

        private readonly HashSet<InputComponent> _components = new HashSet<InputComponent>();
        private Camera2D _camera;
        private readonly InputAttackSubsystem _attackSubsystem;

        public InputSystem(ECS ecs, Camera2D camera)
        {
            _ecs = ecs;
            _camera = camera;
            _attackSubsystem = new InputAttackSubsystem(ecs);
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

                if (BlockedTimerIsOver(component)) component.Blocked = false;
                if (!component.Blocked)
                {
                    if (component.UserInput) ReadInput(component);
                    //else handled by IntelligenceSystem
                }
                else ResetInput(component);

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
            component.SwitchWeapons = InputReader.IsKeyPressed(Keys.Space);
            component.Action = InputReader.IsKeyPressed(Keys.F);
            component.Blink = InputReader.IsKeyPressed(Keys.LeftShift);
            component.CursorPoint = _camera.ConvertScreenToWorld(InputReader.CurrentCursorPos);
            #region Triggers

            //TODO Breunig save triggers here or use "old" data in components
            component.PrimaryAttackTriggered = InputReader.IsMouseButtonTriggered(MouseButtons.LMB);
            component.SecondaryAttackTriggered = InputReader.IsMouseButtonTriggered(MouseButtons.RMB);
            component.SwitchWeaponsTriggered = InputReader.IsKeyTriggered(Keys.Space);
            component.ActionTriggered = InputReader.IsKeyTriggered(Keys.F);
            component.BlinkTriggered = InputReader.IsKeyTriggered(Keys.LeftShift);

            #endregion
        }

        private static void ResetInput(InputComponent component)
        {
            component.MovementDirection.X = 0;
            component.MovementDirection.Y = 0;

            component.PrimaryAttack = false;
            component.SecondaryAttack = false;
            component.SwitchWeapons = false;
            component.Action = false;
            component.Blink = false;
        }

        private static bool BlockedTimerIsOver(InputComponent component)
        {
            return component.Blocked && component.BlockedTimer.Over;
        }

        #region Logic

        private void HandleInputLogic(InputComponent input, GameTime gameTime)
        {
            MovementLogic(input);
            _attackSubsystem.AttackLogic(input, gameTime);
        }
       

        #region Movement

        private void MovementLogic(InputComponent input)
        {
            if (input.Bindings.TryGetValue(typeof(MovementComponent), out var binding))
            {
                MovementComponent movement = binding as MovementComponent;

                movement.Acceleration.Cartesian.X = input.MovementDirection.X * movement.AccelerationBase;
                movement.Acceleration.Cartesian.Y = input.MovementDirection.Y * movement.AccelerationBase;
            }
        }

        #endregion

        #endregion
    }
}