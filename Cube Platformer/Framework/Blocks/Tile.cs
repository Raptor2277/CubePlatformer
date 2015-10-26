using Framework.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFML.Graphics;
using Framework.Utilities;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Factories;
using FarseerPhysics.Collision.Shapes;
using FarseerPhysics.Common;
using Framework.Interface;
using SFML.System;
using Microsoft.Xna.Framework;
using Framework.Managers;

namespace Framework.Blocks
{
    class Tile : Block, IBlocksLight
    {
        public Color Color { get; private set; }

        public Tile(ContentManager c, float x, float y, float width, float height)
        {
            this.Ppm = c.Ppm;
            this.Body = BodyFactory.CreateRectangle(c.World, width / Ppm, height / Ppm, 1);
            Body.Position = new Vector2((x + width / 2f) / Ppm, (y + height / 2f) / Ppm);
            Body.BodyType = BodyType.Static;

            this.Shape = (PolygonShape)Body.FixtureList[0].Shape;
            this.Vertices = new List<Vector2>();
            this.updateVerts();
            this.PositionBox = new Rectangle(x, y, width, height);
            this.IsActive = true;
            this.IsDrawn = true;
            this.IsCollidable = true;

            this.Color = Color.Green;
            this.Id = 1;
        }

        public override void draw(GameTime time, RenderWindow window)
        {
            Draw.fillPolygon(window, Vertices, Color);
            Draw.drawPolygon(window, Vertices, Color.Black);
        }

        public override void update(GameTime time)
        {
            updateVerts();
        }

        public Polygon getLightPolygon()
        {
            return new Polygon(this.Vertices);
        }

    }
}
