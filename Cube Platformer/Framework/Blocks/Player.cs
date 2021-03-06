﻿using Framework.Abstract;
using Framework.Managers;
using Framework.Utilities;
using SFML.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFML.Window;
using Framework.Interface;
using Microsoft.Xna.Framework;
using FarseerPhysics.Common;
using SFML.System;
using FarseerPhysics.Factories;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Collision.Shapes;
using Cube_Platformer;

namespace Framework.Blocks
{
    /// <summary>
    /// This entity is controlled by the player
    /// </summary>
    class Player : Actor , IHandleKeyPress, IBlocksLight
    {
        /// <summary>
        /// Player texture
        /// </summary>
        public Texture Texture { get; private set; }

        /// <summary>
        /// Player color, if no texture available
        /// </summary>
        public Color Color { get; private set; }

        /// <summary>
        /// Representation of the stamina - used for drawing
        /// </summary>
        private Rectangle staminaBox;

        /// <summary>
        /// 0-1 value of the stamina
        /// </summary>
        private float stamina;
        public float Stamina
        {
            get { return stamina; }

            private set
            {
                stamina = value;
                if (stamina > 1) stamina = 1;
            }
        }

        //Action inputs
        private Input.InputAction jump = new Input.InputAction(new List<Keyboard.Key>() {Keyboard.Key.W, Keyboard.Key.Space, Keyboard.Key.Up});
        private Input.InputAction left = new Input.InputAction(new List<Keyboard.Key>() {Keyboard.Key.A, Keyboard.Key.Left});
        private Input.InputAction right = new Input.InputAction(new List<Keyboard.Key>() {Keyboard.Key.D, Keyboard.Key.Right});

        public Player(ContentManager content, float x, float y, float width, float height)
        {
            this.ContentManager = content;
            this.Ppm = content.Ppm;
            this.InitialPosition = new Vector2((x + ( width / 2)) / Ppm, (y + (width /2)) / Ppm);

            this.Body = BodyFactory.CreateRectangle(content.World, width / Ppm, height / Ppm, 1);
            Body.Position = new Vector2((x + width / 2f) / Ppm, (y + height / 2f) / Ppm);
            Body.BodyType = BodyType.Dynamic;

            this.staminaBox = new Rectangle(x, y - 20, width, 10);
            this.Stamina = 1;

            this.PositionBox = new Rectangle(x, y, width, height);

            this.Shape = (PolygonShape)Body.FixtureList[0].Shape;
            this.Vertices = new List<Vector2>();
            this.updateVerts();

            this.IsDrawn = true;
            this.IsActive = true;
            this.IsCollidable = true;
            this.Id = 0;
            this.Color = Color.Red;
            this.Texture = ContentManager.Media.loadTexture(@"Content\images\player.png", true);
            Texture.Smooth = false;
        }

        public override void update(GameTime time)
        {
            updateVerts();

            if(Body.ContactList != null)
                Stamina += .8f * time.frameTime;
            else
                Stamina += .3f * time.frameTime;

            staminaBox.x = Body.Position.X * Ppm - PositionBox.width / 2;
            staminaBox.y = Body.Position.Y * Ppm - PositionBox.height / 2 - 30;
            staminaBox.width = PositionBox.width * Stamina;

            if (right.evaluate() && Body.LinearVelocity.X < 1.7)
                this.Body.ApplyForce(new Vector2(20 * Body.Mass, 0));
            if (left.evaluate() && Body.LinearVelocity.X > -1.7)
                this.Body.ApplyForce(new Vector2(-20 * Body.Mass, 0));
        }

        public override void draw(GameTime time, RenderWindow window)
        {
            //Draw.drawTexRectangle(window, Vertices, Color.White, Texture);
            Draw.fillPolygon(window, Vertices, Color.Red);
            Draw.fillRectangle(window, staminaBox, Color.Yellow);
        }

        /// <summary>
        /// Places the player at the starting positon and state
        /// </summary>
        public void respawn()
        {
            this.Body.LinearVelocity = new Vector2(0, 0);
            this.Body.AngularVelocity = 0;
            this.Body.SetTransform(InitialPosition, 0);

            stamina = 1;
        }

        public void handleKeyPress(KeyEventArgs e)
        {
            if (jump.evaluate(e.Code) && stamina > .5f)
            {
                Stamina -= .5f;
                this.Body.ApplyLinearImpulse(new Vector2(0, -5 * Body.Mass));
            }
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
