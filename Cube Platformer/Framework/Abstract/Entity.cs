using Framework.Managers;
using Framework.Utilities;
using SFML.Graphics;
using SFML.System;
using System.Collections.Generic;
using FarseerPhysics;
using FarseerPhysics.Dynamics;
using Microsoft.Xna.Framework;
using FarseerPhysics.Collision.Shapes;
using FarseerPhysics.Common;

namespace Framework.Abstract
{
    abstract class Entity
    {
        /// <summary>
        /// PositionBox in pixel coords
        /// </summary>
        public Rectangle PositionBox { get; protected set; }

        /// <summary>
        /// Starting position in pixel coords
        /// </summary>
        public Vector2 InitialPosition { get; protected set; }

        /// <summary>
        /// The body used in physics simulation
        /// </summary>
        public Body Body { get; protected set; }

        /// <summary>
        /// Polygon shape representing body
        /// </summary>
        public PolygonShape Shape { get; protected set; }

        /// <summary>
        /// The list of Vertecies that are used to draw the shape
        /// </summary>
        public List<Vector2> Vertices { get; protected set; }

        /// <summary>
        /// Pixels per meter, number of pixel per meter 
        /// </summary>
        public uint Ppm { get; protected set; }

        /// <summary>
        /// Reference to contentManger
        /// </summary>
        public ContentManager ContentManager { get; protected set; }

        /// <summary>
        /// Unique ID
        /// </summary>
        public int Id { get; protected set; }

        /// <summary>
        /// Indicates if the entity is drawn
        /// </summary>
        public bool IsDrawn { get; protected set; }

        /// <summary>
        /// Indicates if the entity is active
        /// </summary>
        public bool IsActive { get; protected set; }

        /// <summary>
        /// Indicates if the entity is collidable
        /// </summary>
        public bool IsCollidable { get; protected set; }

        /// <summary>
        /// Draw the entity
        /// </summary>
        /// <param name="time"></param>
        /// <param name="window"></param>
        public abstract void draw(GameTime time, RenderWindow window);

        /// <summary>
        /// Update the entity
        /// </summary>
        /// <param name="time"></param>
        public abstract void update(GameTime time);

        /// <summary>
        /// Update the vertices after each World update
        /// </summary>
        protected virtual void updateVerts()
        {
            Vertices.Clear();
            FarseerPhysics.Common.Transform t;
            Body.GetTransform(out t);
            foreach (Vector2 vertex in Shape.Vertices)
                Vertices.Add(VectorMath.multiply(MathUtils.Mul(ref t, vertex), this.Ppm));
        }

        /// <summary>
        /// The positionBox in pixel coords
        /// </summary>
        /// <returns></returns>
        public virtual Rectangle getPositionBox()
        {
            return this.PositionBox;
        }
    }
}
