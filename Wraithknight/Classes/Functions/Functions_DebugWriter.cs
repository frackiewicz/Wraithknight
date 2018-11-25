using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace Wraithknight //TODO ABSOLUTE SHITE
{
    class Message
    {
        public String Text;
        public Color Color;

        public Message(String text, Color color)
        {
            Text = text;
            Color = color;
        }
    }

    //Seems to be functional for now
    public static class Functions_DebugWriter //TODO Real ugly lmao
    {
        private static List<Message> _messages = new List<Message>();
        private static int _textCount = 0;

        public static void WriteLine(String text)
        {
            WriteLine(text, Color.White);
        }

        public static void WriteLine(String text, Color color)
        {
            if (!Flags.ShowDebuggingText) return;
            if (_textCount >= _messages.Count)
            {
                _messages.Add(new Message(text, Color.White));
            }
            else
            {
                _messages[_textCount].Text = text;
                _messages[_textCount].Color = Color.White;
            }
            _textCount++;
        }


        private static Vector2 pos = new Vector2(50, 50);

        public static void Draw()
        {
            for (int i = 0; i < _textCount; i++)
            {
                try
                {
                    Functions_Draw.Draw(_messages[i].Text, Assets.GetFont("Test"), pos);
                }
                catch (Exception e)
                {
                    Functions_Draw.Draw("Error", Assets.GetFont("Test"), pos);
                }
                pos.Y += 25;
            }
            Reset();
        }

        public static void Reset()
        {
            _textCount = 0;
            pos.X = 50;
            pos.Y = 50;
        }
    }
}
