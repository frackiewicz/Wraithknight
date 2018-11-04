using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Wraithknight
{
    class ButtonWithText : Button
    {
        public String Text;
        public SpriteFont Font;
        public Vector2 Offset;
        public Vector2 Location;
        public Color Color;

        public ButtonWithText(DrawComponent drawComponent, Rectangle buttonRec, String buttonHandle, String text, SpriteFont font, Vector2 offset, Color? color = null) : base(drawComponent, buttonRec, buttonHandle)
        {
            Text = text;
            Font = font;
            Offset = offset;
            Location = new Vector2(DrawComponent.DrawRec.X + Offset.X, DrawComponent.DrawRec.Y + Offset.Y);
            Color = color ?? Color.White;
        }

        public override void Draw()
        {
            Functions_Draw.Draw(DrawComponent);
            Functions_Draw.Draw(Text, Font, Location);
        }
    }
}
