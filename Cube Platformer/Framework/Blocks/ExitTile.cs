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
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Framework.Blocks
{
    class ExitTile : Framework.Abstract.Block , IBlocksLight
    {
        private Color Color { get; set; }
        public Texture Texture { get; private set; }

        public ExitTile(ContentManager c, float x, float y, float width, float height)
        {
            this.ContentManager = c;
            this.Ppm = c.Ppm;
            this.Body = BodyFactory.CreateRectangle(c.World, width / Ppm, height / Ppm, 1);
            Body.Position = new Vector2((x + width / 2f) / Ppm, (y + height / 2f) / Ppm);
            Body.BodyType = BodyType.Dynamic;
            Body.OnCollision += Body_OnCollision;

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

        bool Body_OnCollision(Fixture fixtureA, Fixture fixtureB, FarseerPhysics.Dynamics.Contacts.Contact contact)
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


        public Polygon getLightPolygon()
        {
            return new Polygon(this.Vertices);
        }
    }
}
