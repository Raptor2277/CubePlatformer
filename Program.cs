using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cube_Platformer
{
    class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            Game1 game = new Game1();
            game.run();
        }
    }
}
