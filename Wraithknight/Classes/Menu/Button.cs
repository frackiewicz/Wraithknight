using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace Wraithknight
{
    internal class Button : MenuObject, IInteractableMenuObject
    {
        public Rectangle ButtonRec; //the area in which the button will accept input (eg mouseclicks)
        public String ButtonHandle;

        public bool IsClicked = false;

        public Button(DrawComponent drawComponent, Rectangle buttonRec, String buttonHandle) : base(drawComponent)
        {
            ButtonRec = buttonRec;
            ButtonHandle = buttonHandle;
        }

        public void HandleInput()
        {
            throw new NotImplementedException();
        }

        public bool HandleMouseClick()
        {
            IsClicked = InputReader.CurrentCursorPos.X > ButtonRec.X && InputReader.CurrentCursorPos.X < ButtonRec.X + ButtonRec.Width &&
                        InputReader.CurrentCursorPos.Y > ButtonRec.Y && InputReader.CurrentCursorPos.Y < ButtonRec.Y + ButtonRec.Height;

            return IsClicked;
        }
    }
}
