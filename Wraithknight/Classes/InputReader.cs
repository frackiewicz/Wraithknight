using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Wraithknight
{
    public enum MouseButtons { LMB, RMB }
    public static class InputReader //is static alright here? TODO Breunig
    {

        #region attributes
        private static KeyboardState _previousKeyboardState;
        private static KeyboardState _currentKeyboardState;

        private static MouseState _previousMouseState;
        private static MouseState _currentMouseState;
        public static Point PreviousCursorPos { get; private set; }
        public static Point CurrentCursorPos { get; private set; }


        private static GamePadCapabilities _gamePadCapabilities;
        private static GamePadState _previousGamePadState;
        private static GamePadState _currentGamePadState;
        #endregion


        public static void Initialize()
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
        public static bool IsKeyPressed(Keys key)
        {
            return _currentKeyboardState.IsKeyDown(key);
        }

        public static bool IsKeyTriggered(Keys key)
        {
            return _currentKeyboardState.IsKeyDown(key) &&
                   _previousKeyboardState.IsKeyUp(key);
        }

        public static bool IsKeyReleased(Keys key)
        {
            return _currentKeyboardState.IsKeyUp(key) &&
                   _previousKeyboardState.IsKeyDown(key);
        }
        #endregion

        #region mouse
        public static bool IsMouseButtonPressed(MouseButtons button)
        {
            if (button == MouseButtons.LMB)
            {
                return _currentMouseState.LeftButton == ButtonState.Pressed;
            }

            if (button == MouseButtons.RMB)
            {
                return _currentMouseState.RightButton == ButtonState.Pressed;
            }
            return false;
        }

        public static bool IsMouseButtonTriggered(MouseButtons button)
        {
            if (button == MouseButtons.LMB)
            {
                return _currentMouseState.LeftButton == ButtonState.Pressed && 
                       _previousMouseState.RightButton == ButtonState.Released;
            }

            if (button == MouseButtons.RMB)
            {
                return _currentMouseState.RightButton == ButtonState.Pressed &&
                       _previousMouseState.RightButton == ButtonState.Released;
            }
            return false;
        }

        public static bool IsMouseButtonReleased(MouseButtons button)
        {
            if (button == MouseButtons.LMB)
            {
                return _currentMouseState.LeftButton == ButtonState.Released &&
                       _previousMouseState.LeftButton == ButtonState.Pressed;
            }

            if (button == MouseButtons.RMB)
            {
                return _currentMouseState.RightButton == ButtonState.Released &&
                       _previousMouseState.RightButton == ButtonState.Pressed;
            }
            return false;
        }

        public static bool IsScrollingUp()
        {
            return _previousMouseState.ScrollWheelValue < _currentMouseState.ScrollWheelValue;
        }

        public static bool IsScrollingDown()
        {
            return _previousMouseState.ScrollWheelValue > _currentMouseState.ScrollWheelValue;
        }

        public static int GetMouseWheel()
        {
            return _currentMouseState.ScrollWheelValue;
        }

        public static int GetPreviousMouseWheel()
        {
            return _previousMouseState.ScrollWheelValue;
        }

        public static int GetHorizontalMouseWheel() //thefukisthis
        {
            return _currentMouseState.HorizontalScrollWheelValue;
        }
        #endregion

        #endregion

        #region update

        public static void Update()
        {
            UpdateStates();
            UpdateMousePos();
        }

        private static void UpdateStates()
        {
            _previousKeyboardState = _currentKeyboardState;
            _currentKeyboardState = Keyboard.GetState();

            _previousMouseState = _currentMouseState;
            _currentMouseState = Mouse.GetState();

            _previousGamePadState = _currentGamePadState;
            _currentGamePadState = GamePad.GetState(PlayerIndex.One);
        }

        private static void UpdateMousePos()
        {
            PreviousCursorPos = CurrentCursorPos;
            CurrentCursorPos = new Point(_currentMouseState.X, _currentMouseState.Y);
        }

        #endregion

    }
}
