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
        public override Rectangle getPositionBox()
        {
            return new Rectangle(Body.Position.X * Ppm + PositionBox.width / 2f, Body.Position.Y * Ppm - PositionBox.height / 2f, PositionBox.width, PositionBox.height);
        }
    }
}
