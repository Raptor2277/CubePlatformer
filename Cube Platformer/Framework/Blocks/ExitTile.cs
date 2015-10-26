using FarseerPhysics.Collision.Shapes;
using FarseerPhysics.Common;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Factories;
using Framework.Abstract;
using Framework.Interface;
using Framework.Managers;
using Framework.Utilities;
using Microsoft.Xna.Framework;
using SFML.Graphics;
using System;
using System.Collections.Generic;

namespace Framework.Blocks
{

    /// <summary>
    /// Tile that the player must reach to beat the level
    /// </summary>
    class ExitTile : Framework.Abstract.Block , IBlocksLight
    {
        /// <summary>
        /// Color of the tile
        /// </summary>
        private Color Color { get; set; }

        /// <summary>
        /// Texture for the tile
        /// </summary>
        public Texture Texture { get; private set; }

        public ExitTile(ContentManager c, float x, float y, float width, float height)
        {
            this.ContentManager = c;
            this.Ppm = c.Ppm;
            this.Body = BodyFactory.CreateRectangle(c.World, width / Ppm, height / Ppm, 1);
            Body.Position = new Vector2((x + width / 2f) / Ppm, (y + height / 2f) / Ppm);
            Body.BodyType = BodyType.Dynamic;
            Body.OnCollision += Body_OnCollision;

            this.PositionBox = new Rectangle(x, y, width, height);
            this.Shape = (PolygonShape)Body.FixtureList[0].Shape;
            this.Vertices = new List<Vector2>();
            this.updateVerts();

            this.IsActive = true;
            this.IsDrawn = true;
            this.IsCollidable = true;

            this.Color = new Color(0, 0, 255);
            this.Texture = ContentManager.Media.loadTexture(@"Content\images\surge.png", true);
            this.Id = 2;
        }

        /// <summary>
        /// Detects the collision with the player and calls the content manager
        /// </summary>
        /// <param name="fixtureA"></param>
        /// <param name="fixtureB"></param>
        /// <param name="contact"></param>
        /// <returns></returns>
        private bool Body_OnCollision(Fixture fixtureA, Fixture fixtureB, FarseerPhysics.Dynamics.Contacts.Contact contact)
        {
            if (ContentManager.isPlayer(fixtureB.Body.BodyId))
                ContentManager.onLevelWon();
            return true;
        }

        public override void draw(GameTime time, RenderWindow window)
        {
            //Draw.fillPolygon(window, Vertices, Color);
            Draw.drawTexRectangle(window, Vertices, Color.White, Texture);
        }

        public override void update(GameTime time)
        {
            updateVerts();
        }

        /// <summary>
        /// Returns verticies to calculate shadows
        /// </summary>
        /// <returns></returns>
        public Polygon getLightPolygon()
        {
            return new Polygon(this.Vertices);
        }
    }
}
