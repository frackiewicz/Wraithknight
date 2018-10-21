using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Wraithknight.Classes.ECS.Components
{
    class InputComponent : Component
    {
        //Maybe link to all the other components which might require input?

        public Vector2 MovementVector; //necessary?

        public KeyboardState PreviousKeyboardState;
        public KeyboardState CurrentKeyboardState;

        public MouseState PreviousMouseState;
        public MouseState CurrentMouseState;
        public Point PreviousCursorPos;
        public Point CurrentCursorPos;


        public GamePadCapabilities GamePadCapabilities;
        public GamePadState PreviousGamePadState;
        public GamePadState CurrentGamePadState;

    }
}
