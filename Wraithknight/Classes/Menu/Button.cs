using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Wraithknight
{
    internal class Button : MenuObject
    {
        public Rectangle ButtonRec; //the area in which the button will accept input (eg mouseclicks)
        public String ButtonHandle;

        public bool IsClicked = false;

        public Button(Vector2 position, DrawComponent drawComponent, Rectangle buttonRec, String buttonHandle) : base(position, drawComponent)
        {
            ButtonRec = buttonRec;
            ButtonHandle = buttonHandle;
        }

        public bool HandleMouseClick()
        {
            IsClicked = InputReader.CurrentCursorPos.X > ButtonRec.X && InputReader.CurrentCursorPos.X < ButtonRec.X + ButtonRec.Width &&
                        InputReader.CurrentCursorPos.Y > ButtonRec.Y && InputReader.CurrentCursorPos.Y < ButtonRec.Y + ButtonRec.Height;

            return IsClicked;
        }

        public override void Align(Viewport viewport)
        {
            base.Align(viewport);

            if (DrawOrigin == Origin.Center)
            {
                ButtonRec.X = (int) Position.X - DrawComponent.DrawRec.Width / 2;
                ButtonRec.Y = (int) Position.Y - DrawComponent.DrawRec.Height / 2;
            }
        }
    }
}
