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
    class VectorMath
    {
        public static void log(String x)
        {
            System.Console.WriteLine(x);
        }

        public static float dotProduct(Vector2f vec1, Vector2f vec2)
        {
            return vec1.X * vec2.X + vec1.Y * vec2.Y;
        }

        public static Vector2f multiply(Vector2f vec1, float amount)
        {
            return new Vector2f(vec1.X * amount, vec1.Y * amount);
        }

        public static Vector2 multiply(Vector2 vec1, float amount)
        {
            return new Vector2(vec1.X * amount, vec1.Y * amount);
        }

        public static Vector2f minus(Vector2f vec1, Vector2f vec2)
        {
            return new Vector2f(vec2.X - vec1.X, vec2.Y - vec1.Y);
        }

        public static Vector2f normalize(Vector2f vec)
        {
            float len = (float)Math.Sqrt(vec.X * vec.X + vec.Y + vec.Y);
            return new Vector2f(vec.X * len, vec.Y * len);
        }

        public static Vertex[] toVertecies(Vector2f[] vectors, Color color)
        {
            List<Vertex> verts = new List<Vertex>();
            
            foreach (Vector2f v in vectors)
                verts.Add(new Vertex(v, color));

            return verts.ToArray();
        }
    }
}
