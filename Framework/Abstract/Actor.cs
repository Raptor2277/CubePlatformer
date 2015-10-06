using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFML.Graphics;
using Framework.Utilities;
using Framework.Managers;
using Framework.Interface;
using FarseerPhysics.Collision.Shapes;
using Microsoft.Xna.Framework;
using FarseerPhysics.Common;

namespace Framework.Abstract
{
    abstract class Actor : Entity
    {
        /// <summary>
        /// Polygon shape representing the actor at 0,0 postition
        /// </summary>
        public PolygonShape Shape { get; protected set; }

        /// <summary>
        /// The list of Vertecies that are used to draw the shape
        /// </summary>
        public List<Vector2> Vertices { get; protected set; }

        /// <summary>
        /// Pixels per meter, the width in pixel for each meter 
        /// </summary>
        public uint Ppm { get; protected set; }

        /// <summary>
        /// Updates the vertices after each World update
        /// </summary>
        protected void updateVerts()
        {
            Vertices.Clear();
            FarseerPhysics.Common.Transform t;
            Body.GetTransform(out t);
            foreach (Vector2 vertex in Shape.Vertices)
                Vertices.Add(VectorMath.multiply(MathUtils.Mul(ref t, vertex), this.Ppm));
        }
    }
}
