using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Wraithknight
{
    class ButtonWithText : Button //TODO this code could use some cleanup
    {
        //TODO enum for dynamic text placement //left, right, center, top, combinations etc
        public String Text;
        public SpriteFont Font;
        public Vector2 Offset;
        public bool CenteredText;
        public Vector2 Location;
        public Color Color;

        public ButtonWithText(Vector2 position, DrawComponent drawComponent, Rectangle buttonRec, String buttonHandle, String text, SpriteFont font, Vector2 offset, Color? color = null, bool centeredText = true) : base(position, drawComponent, buttonRec, buttonHandle)
        {
            Text = text;
            Font = font;
            CenteredText = centeredText;
            SetLocation(font, text);
            Offset = offset;
            Color = color ?? Color.White;
        }

        public override void Draw()
        {
            Functions_Draw.Draw(DrawComponent);
            Functions_Draw.Draw(Text, Font, Location);
        }

        public override void Align(Viewport viewport)
        {
            base.Align(viewport);
            SetLocation(Font, Text);
        }

        private void SetLocation(SpriteFont font, String text)
        {
            if (CenteredText) Location = new Vector2(DrawComponent.DrawRec.X + DrawComponent.DrawRec.Width/2 - font.MeasureString(text).X/2 + Offset.X, DrawComponent.DrawRec.Y + DrawComponent.DrawRec.Height / 2 - font.MeasureString(text).Y / 2 + Offset.Y);
            else Location = new Vector2(DrawComponent.DrawRec.X + Offset.X, DrawComponent.DrawRec.Y + Offset.Y);
        }
    }
}
