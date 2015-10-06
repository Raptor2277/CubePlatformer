using Framework.Managers;
using Framework.Utilities;
using SFML.Graphics;
using SFML.System;
using System.Collections.Generic;
using FarseerPhysics;
using FarseerPhysics.Dynamics;
using Microsoft.Xna.Framework;

namespace Framework.Abstract
{
    abstract class Entity
    {
        public Body Body { get; protected set; }
        public ContentManager ContentManager { get; protected set; }
        public Vector2 InitialPosition { get; protected set; }

        public int Id { get; protected set; }
        public bool IsDrawn { get; protected set; }
        public bool IsActive { get; protected set; }
        public bool IsCollidable { get; protected set; }

        public abstract void draw(GameTime time, RenderWindow window);

        public abstract void update(GameTime time);

    }
}
