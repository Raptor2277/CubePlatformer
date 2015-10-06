using Framework.Abstract;
using SFML.Graphics;
using SFML.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Framework.Utilities
{
    class Utils
    {
        private Utils() { }

        public static void log(String s)
        {
            System.Console.Out.WriteLineAsync(s);
        }

        public static Rectangle toRectangle(FloatRect r)
        {
            return new Rectangle(r.Left, r.Top, r.Width, r.Height);
        }

        public static bool checkMouseCollision(int x, int y, Rectangle rect)
        {
            if (x > rect.x && x < rect.x + rect.width)
            {
                if (y > rect.y && y < rect.y + rect.height)
                    return true;
            }
            return false;
        }
    }
}
