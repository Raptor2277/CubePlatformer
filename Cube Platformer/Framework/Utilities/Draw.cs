using Microsoft.Xna.Framework;
using SFML.Graphics;
using SFML.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Framework.Utilities
{
    class Draw
    {
        public static void drawRectangle(RenderWindow w, Rectangle rect, Color c)
        {
            Vertex[] verts = new Vertex[8];

            verts[0] = new Vertex(new Vector2f(rect.x, rect.y), c);
            verts[1] = new Vertex(new Vector2f(rect.x + rect.width, rect.y), c);

            verts[2] = new Vertex(new Vector2f(rect.x + rect.width, rect.y), c);
            verts[3] = new Vertex(new Vector2f(rect.x + rect.width, rect.y + rect.height), c);

            verts[4] = new Vertex(new Vector2f(rect.x + rect.width, rect.y + rect.height), c);
            verts[5] = new Vertex(new Vector2f(rect.x, rect.y + rect.height), c);

            verts[6] = new Vertex(new Vector2f(rect.x, rect.y + rect.height), c);
            verts[7] = new Vertex(new Vector2f(rect.x, rect.y), c);

            w.Draw(verts, PrimitiveType.Lines);
        }

        public static void drawLine(RenderWindow w, Vector2f a, Vector2f b, Color c)
        {
            Vertex[] verts = new Vertex[8];

            verts[0] = new Vertex(new Vector2f(a.X, a.Y), c);
            verts[1] = new Vertex(new Vector2f(b.X, b.Y), c);

            w.Draw(verts, PrimitiveType.Lines);
        }

        public static void fillRectangle(RenderWindow w, Rectangle rect, Color c)
        {
            Vertex[] verts = new Vertex[8];
            verts[0] = new Vertex(new Vector2f(rect.x, rect.y), c);
            verts[1] = new Vertex(new Vector2f(rect.x + rect.width, rect.y), c);
            verts[2] = new Vertex(new Vector2f(rect.x + rect.width, rect.y + rect.height), c);
            verts[3] = new Vertex(new Vector2f(rect.x, rect.y + rect.height), c);

            w.Draw(verts, PrimitiveType.Quads);
        }

        public static void drawPolygon(RenderWindow w, List<Vector2> vertecies, Color c)
        {
            Vertex[] verts = new Vertex[vertecies.Count + 1];

            int i = 0;
            foreach (Vector2 v in vertecies)
            {
                verts[i] = new Vertex(new Vector2f(v.X , v.Y ), c);
                i++;
            }
            verts[i] = new Vertex(new Vector2f(vertecies[0].X , vertecies[0].Y ), c);

            w.Draw(verts, PrimitiveType.LinesStrip);
        }

        public static void drawTexRectangle(RenderWindow w, List<Vector2> vertecies, Color c, Texture t)
        {
            Vertex[] verts = new Vertex[4];

            verts[0] = new Vertex(new Vector2f(vertecies[3].X, vertecies[3].Y), c, new Vector2f(0,0));
            verts[1] = new Vertex(new Vector2f(vertecies[0].X, vertecies[0].Y), c, new Vector2f(t.Size.X, 0));
            verts[2] = new Vertex(new Vector2f(vertecies[1].X, vertecies[1].Y), c, new Vector2f(t.Size.X, t.Size.Y));
            verts[3] = new Vertex(new Vector2f(vertecies[2].X, vertecies[2].Y), c, new Vector2f(0, t.Size.Y));

            w.Draw(verts, PrimitiveType.Quads, new RenderStates(t));
        }

        public static void fillPolygon(RenderWindow w, List<Vector2> vertecies, Color c)
        {
            Vertex[] verts = new Vertex[vertecies.Count];

            int i = 0;
            foreach (Vector2 v in vertecies)
            {
                verts[i] = new Vertex(new Vector2f(v.X, v.Y), c);
                i++;
            }

            w.Draw(verts, PrimitiveType.Quads);
        }

        public static void drawCircle(RenderWindow w, Vector2 pos, float radius, int sides)
        {
            List<Vertex> verts = new List<Vertex>();

            double inc = Math.PI * 2.0f / sides;

            for (double theta = 0; theta < Math.PI * 2; theta += inc)
            {
                verts.Add(new Vertex(new Vector2f(pos.X + (float)Math.Cos(theta) * radius, pos.Y + (float)Math.Sin(theta) * radius), Color.White));
            }

            //pointing back to the first vertex
            verts.Add(new Vertex(new Vector2f(pos.X + (float)Math.Cos(0) * radius, pos.Y + (float)Math.Sin(0) * radius), Color.White));

            w.Draw(verts.ToArray(), PrimitiveType.LinesStrip);
        }

        public static void fillCircle(RenderWindow w, Vector2f pos, float radius, int sides, Color color)
        {
            List<Vertex> verts = new List<Vertex>();

            double inc = Math.PI * 2.0f / sides;

            //middle point
            verts.Add(new Vertex(new Vector2f(pos.X, pos.Y ), color));

            //the side verticies
            for (double theta = 0; theta < Math.PI * 2; theta += inc)
            {
                verts.Add(new Vertex(new Vector2f(pos.X + (float)Math.Cos(theta) * radius, pos.Y + (float)Math.Sin(theta) * radius), color));
            }

            //pointing back to the first vertex
            verts.Add(new Vertex(new Vector2f(pos.X + (float)Math.Cos(0) * radius, pos.Y + (float)Math.Sin(0) * radius), color));

            w.Draw(verts.ToArray(), PrimitiveType.TrianglesFan);
        }
    }
}
