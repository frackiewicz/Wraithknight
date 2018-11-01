using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Wraithknight
{
    class InputComponent : Component
    {
        /*
        public KeyboardState PreviousKeyboardState;
        public KeyboardState CurrentKeyboardState;

        public MouseState PreviousMouseState;
        public MouseState CurrentMouseState;
        public Point PreviousCursorPos;
        public Point CurrentCursorPos;


        public GamePadCapabilities GamePadCapabilities;
        public GamePadState PreviousGamePadState;
        public GamePadState CurrentGamePadState;
        */

        public Vector2 MovementDirection = new Vector2(0, 0); //WASD
        public bool PrimaryAttack = false; //LMB
        public bool SecondaryAttack = false; //RMB
        public bool SwitchWeapons = false; //Space
        public bool Action = false; //F
        public bool Blink = false; //Shift
        public Point CursorPoint = new Point();
    }
}
