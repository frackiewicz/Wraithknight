using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Wraithknight
{
    public class InputComponent : Component
    {
        public KeyboardState PreviousKeyboardState;
        public KeyboardState CurrentKeyboardState;

        public MouseState PreviousMouseState;
        public MouseState CurrentMouseState;
        public Point PreviousCursorPos;
        public Point CurrentCursorPos;


        public GamePadCapabilities GamePadCapabilities;
        public GamePadState PreviousGamePadState;
        public GamePadState CurrentGamePadState;
        //TODO Use abstracts or copy from InputReader -> Breunig?
    }
}
