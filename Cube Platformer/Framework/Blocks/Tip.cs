using FarseerPhysics.Collision.Shapes;
using FarseerPhysics.Common;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Factories;
using Framework.Abstract;
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
    class Tip : Framework.Abstract.Block
    {
        public Color Color { get; private set; }

        public Tip(ContentManager c, float x, float y, float width, float height)
        {
            this.ContentManager = c;
            this.InitialPosition = new Vector2(x / Ppm, y / Ppm);

            this.Ppm = c.Ppm;
            this.Body = BodyFactory.CreateRectangle(c.World, width / Ppm, height / Ppm, 1);
            Body.Position = new Vector2((x + width / 2f) / Ppm, (y + height / 2f) / Ppm);
            Body.BodyType = BodyType.Static;
            Body.CollidesWith = Category.None;
            Body.OnCollision += Body_OnCollision;

            this.PositionBox = new Rectangle(x, y, width, height);
            this.Shape = (PolygonShape)Body.FixtureList[0].Shape;
            this.Vertices = new List<Vector2>();
            this.updateVerts();

            this.Color = Color.Yellow;
            this.Id = 4;
            this.IsActive = true;
            this.IsDrawn = true;
            this.IsCollidable = false;
        }

        bool Body_OnCollision(Fixture fixtureA, Fixture fixtureB, FarseerPhysics.Dynamics.Contacts.Contact contact)
        {
            Console.WriteLine("this");
            return false;
        }

        public override void draw(Utilities.GameTime time, SFML.Graphics.RenderWindow window)
        {
            Draw.fillPolygon(window, Vertices, Color);
        }

        public override void update(Utilities.GameTime time)
        {
            updateVerts();
        }

    }
}
