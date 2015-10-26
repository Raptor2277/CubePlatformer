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
    /// <summary>
    /// Entity that is not controlled by Player or AI
    /// </summary>
    abstract class Block : Entity
    {
        /// <summary>
        /// Gets the position box in pixel coordinates
        /// </summary>
        public override Rectangle PositionBox
        {
            get
            {
                return new Rectangle((Body.Position.X * Ppm) - positionBox.width / 2f, (Body.Position.Y * Ppm) - positionBox.height / 2f, positionBox.width, positionBox.height);
            }
            protected set
            {
                this.positionBox = value;
            }
        }
    }
}
