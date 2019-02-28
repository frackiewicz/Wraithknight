using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Wraithknight
{
    class InputComponent : BindableComponent
    {
        public readonly bool UserInput;
        public bool Blocked;
        public TimerComponent BlockedTimer = new TimerComponent();

        public Vector2 MovementDirection = new Vector2(0, 0); //WASD
        public bool PrimaryAttack = false; //LMB
        public bool SecondaryAttack = false; //RMB
        public bool SwitchWeapons = false; //Space
        public bool Action = false; //F
        public bool Blink = false; //Shift
        #region Triggers
        public bool PrimaryAttackTriggered = false; //LMB
        public bool SecondaryAttackTriggered = false; //RMB
        public bool SwitchWeaponsTriggered = false; //Space
        public bool ActionTriggered = false; //F
        public bool BlinkTriggered = false; //Shift
        #endregion
        public Point CursorPoint = new Point();

        public InputComponent(bool userInput)
        {
            UserInput = userInput;
        }
    }
}
