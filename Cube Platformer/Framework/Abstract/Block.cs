using FarseerPhysics.Collision.Shapes;
using FarseerPhysics.Common;
using Framework.Managers;
using Framework.Utilities;
using Microsoft.Xna.Framework;
using SFML.Graphics;
using SFML.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Framework.Abstract
{
    abstract class Block : Entity
    {
        public override Rectangle getPositionBox()
        {
            return new Rectangle((Body.Position.X * Ppm) - PositionBox.width / 2f, (Body.Position.Y * Ppm) - PositionBox.height / 2f, PositionBox.width, PositionBox.height);
        }
    }
}
