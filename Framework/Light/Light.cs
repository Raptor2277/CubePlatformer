using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFML.Graphics;
using SFML.System;
using Framework.Utilities;

namespace Framework.Light
{
    public class Light
    {
        public Vector2f pos;
        public Color color;
        public float brightness;


        public Light(Vector2f pos, Color color, float brightness)
        {
            this.pos = pos;
            this.color = color;
            this.brightness = brightness;
        }

        public void draw(RenderWindow window)
        {
            Draw.fillCircle(window, pos, 10, 12, color);
        }
    }
}
