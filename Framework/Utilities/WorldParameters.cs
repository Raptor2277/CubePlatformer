using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Framework.Utilities
{
    class WorldParameters
    {
        public Vector2 Gravity { get; private set; }
        public uint Ppm { get; private set; }

        public WorldParameters(Vector2 g, uint ppm)
        {
            this.Gravity = g;
            this.Ppm = ppm;
        }
    }
}
