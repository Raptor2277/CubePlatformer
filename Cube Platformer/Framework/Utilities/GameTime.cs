using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFML.System;

namespace Framework.Utilities
{
    class GameTime
    {
        public Clock gameTime { get; set; }
        public float frameTime { get; set; }

        public GameTime()
        {
            gameTime = new Clock();
            frameTime = 0;
        }

    }
}
