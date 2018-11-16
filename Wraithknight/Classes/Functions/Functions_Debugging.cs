using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace Wraithknight
{
    struct Message
    {
        public String Text;
        public Color Color;

        public Message(String text, Color color)
        {
            Text = text;
            Color = color;
        }
    }
    public static class Functions_Debugging //TODO Real ugly lmao
    {
        private static List<Message> _messages = new List<Message>();
        private static Stopwatch timer = new Stopwatch(); //slowdown time instead?


        public static void WriteLine(String text)
        {
            _messages.Add(new Message(text, Color.White));
        }

        public static void WriteLine(String text, Color color)
        {
            _messages.Add(new Message(text, color));
        }

        public static void Draw()
        {
            Vector2 pos = new Vector2(50,50);
            foreach (var message in _messages)
            {
                try
                {
                    Functions_Draw.Draw(message.Text, Assets.GetFont("Test"), pos);
                }
                catch (Exception e)
                {
                    Functions_Draw.Draw("Error", Assets.GetFont("Test"), pos);

                }
                pos.Y += 25;
            }
        }

        public static void Reset()
        {
            _messages.Clear();
        }
    }
}
