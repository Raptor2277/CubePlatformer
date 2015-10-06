using Cube_Platformer;
using SFML.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Framework.Utilities
{
    class Log
    {
        public static List<string> Logs = new List<string>();
        public static Text Text = new Text("", Game1.plainFont);
        public static bool IsLogging = true;

        private Log()
        {

        }

        public static void draw(RenderWindow window)
        {
            if(IsLogging)
            {
                Text.Position = new SFML.System.Vector2f(0,0);
                foreach (string s in Logs)
                {
                    Text.DisplayedString = s;
                    window.Draw(Text);
                    Text.Position = new SFML.System.Vector2f(Text.Position.X , Text.Position.Y + 30);
                }
                Logs.Clear();
            }
        }

        public static void add(string s)
        {
            if(IsLogging)
                Logs.Add(s);
        }
    }
}