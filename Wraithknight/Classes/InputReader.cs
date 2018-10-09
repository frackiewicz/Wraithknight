using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Wraithknight
{
    public enum MouseButtons { LeftButton, RightButton }
    public class InputReader
    {

        #region attributes

        private KeyboardState _previousKeyboardState;
        private KeyboardState _currentKeyboardState;

        private MouseState _previousMouseState;
        private MouseState _currentMouseState;
        public Point PreviousCursorPos { get; private set; }
        public Point CurrentCursorPos { get; private set; }


        private GamePadCapabilities _gamePadCapabilities;
        private GamePadState _previousGamePadState;
        private GamePadState _currentGamePadState;

        #endregion


        public InputReader()
        {
            _previousKeyboardState = Keyboard.GetState();
            _currentKeyboardState = Keyboard.GetState();

            _previousMouseState = Mouse.GetState();
            _currentMouseState = Mouse.GetState();

            PreviousCursorPos = Point.Zero;
            CurrentCursorPos = Point.Zero;

            _gamePadCapabilities = GamePad.GetCapabilities(PlayerIndex.One);
            if (_gamePadCapabilities.IsConnected)
            {
                _previousGamePadState = GamePad.GetState(PlayerIndex.One);
                _currentGamePadState = GamePad.GetState(PlayerIndex.One);
            }


        }

        #region inputHandeling

        #region keyboard

        public bool IsKeyPressed(Keys key)
        {
            return _currentKeyboardState.IsKeyDown(key);
        }

        public bool IsKeyTriggered(Keys key)
        {
            return _currentKeyboardState.IsKeyDown(key) &&
                   _previousKeyboardState.IsKeyUp(key);
        }

        public bool IsKeyReleased(Keys key)
        {
            return _currentKeyboardState.IsKeyUp(key) &&
                   _previousKeyboardState.IsKeyDown(key);
        }

        #endregion

        #region mouse

        public bool IsMouseButtonPressed(MouseButtons button)
        {
            if (button == MouseButtons.LeftButton)
            {
                return _currentMouseState.LeftButton == ButtonState.Pressed;
            }

            if (button == MouseButtons.RightButton)
            {
                return _currentMouseState.RightButton == ButtonState.Pressed;
            }


            return false;
        }

        public bool IsMouseButtonTriggered(MouseButtons button)
        {
            if (button == MouseButtons.LeftButton)
            {
                return _currentMouseState.LeftButton == ButtonState.Pressed && 
                       _previousMouseState.RightButton == ButtonState.Released;
            }

            if (button == MouseButtons.RightButton)
            {
                return _currentMouseState.RightButton == ButtonState.Pressed &&
                       _previousMouseState.RightButton == ButtonState.Released;
            }


            return false;
        }


        public bool IsMouseButtonReleased(MouseButtons button)
        {
            if (button == MouseButtons.LeftButton)
            {
                return _currentMouseState.LeftButton == ButtonState.Released &&
                       _previousMouseState.LeftButton == ButtonState.Pressed;
            }

            if (button == MouseButtons.RightButton)
            {
                return _currentMouseState.RightButton == ButtonState.Released &&
                       _previousMouseState.RightButton == ButtonState.Pressed;
            }


            return false;
        }



        #endregion



        #endregion

        #region update

        public void Update()
        {
            UpdateStates();
            UpdateMousePos();
        }

        private void UpdateStates()
        {
            // update the keyboard state
            _previousKeyboardState = _currentKeyboardState;
            _currentKeyboardState = Keyboard.GetState();

            // update the mouse state
            _previousMouseState = _currentMouseState;
            _currentMouseState = Mouse.GetState();

            // update the gamepad state
            _previousGamePadState = _currentGamePadState;
            _currentGamePadState = GamePad.GetState(PlayerIndex.One);
        }

        private void UpdateMousePos()
        {
            PreviousCursorPos = CurrentCursorPos;
            CurrentCursorPos = new Point(_currentMouseState.X, _currentMouseState.Y);
        }

        #endregion

    }
}
