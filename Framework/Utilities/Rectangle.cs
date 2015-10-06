using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Framework.Utilities
{
    public class Rectangle
    {
        public float x;
        public float y;
        public float width;
        public float height;

        public Rectangle()
        {
            this.x = 0;
            this.y = 0;
            this.width = 0;
            this.height = 0;
        }

        public Rectangle(float x, float y, float width, float height)
        {
            this.x = x;
            this.y = y;
            this.width = width;
            this.height = height;
        }

        public List<Line> getLines()
        {
            return new List<Line> { new Line(x, y, x + width, y), 
                                    new Line(x + width, y, x + width, y + height), 
                                    new Line(x + width, y + height, x, y + height), 
                                    new Line(x, y + height, x, y), };
        }
    }
}
