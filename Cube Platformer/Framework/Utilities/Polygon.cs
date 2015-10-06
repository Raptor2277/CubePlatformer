using Framework.Utilities;
using Microsoft.Xna.Framework;
using SFML.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Framework.Utilities
{
    class Polygon
    {
        public List<Vector2> Vertices { get; private set;}

        public Polygon(List<Vector2> vertices)
        {
            this.Vertices = vertices;
        }

        public List<Line> getLines()
        {
            List<Line> lines = new List<Line>();

            int count = Vertices.Count;
            for (int i = 0; i < Vertices.Count; i++ )
                if(i < count - 1 )
                    lines.Add(new Line(Vertices[i].X, Vertices[i].Y, Vertices[i+1].X, Vertices[i+1].Y));
            lines.Add(new Line(Vertices[count - 1].X, Vertices[count - 1].Y, Vertices[0].X, Vertices[0].Y)); //line pointing to the first vertex

            return lines;
        }

        public static List<Line> getLines(List<Vector2f> vertices)
        {
            List<Line> lines = new List<Line>();

            int count = vertices.Count;
            for (int i = 0; i < vertices.Count; i++ )
                if(i < count - 2 )
                    lines.Add(new Line(vertices[i].X, vertices[i].Y, vertices[i+1].X, vertices[i+1].Y));
            lines.Add(new Line(vertices[count - 1].X, vertices[count - 1].Y, vertices[0].X, vertices[0].Y)); //line pointing to the first vertex

            return lines;
        }
    }
}
