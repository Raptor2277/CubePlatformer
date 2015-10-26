using SFML.Graphics;
using Framework.Utilities;
using Framework.Managers;
using Framework.Interface;
using FarseerPhysics.Collision.Shapes;
using Microsoft.Xna.Framework;
using FarseerPhysics.Common;

namespace Framework.Abstract
{
    /// <summary>
    /// Any entity that is controlled by Player or AI
    /// </summary>
    abstract class Actor : Entity
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
