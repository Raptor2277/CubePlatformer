using SFML.Graphics;
using SFML.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Framework.Light
{
    class SpotLight : Light
    {
        public Vector2f boundOne;
        public Vector2f boundTwo;
        public float angle;
        public float sweep;

        public SpotLight(Vector2f pos, Color color, float brightness, float angle, float sweep) : base(pos,color, brightness)
        {
            this.angle = angle;
            this.sweep = sweep;
            setBounds();
        }

        public Vector2f getBoundOne()
        {
            return new Vector2f(pos.X + boundOne.X , pos.Y + boundOne.Y );
        }

        public Vector2f getBoundTwo()
        {
            return new Vector2f(pos.X + boundTwo.X , pos.Y + boundTwo.Y );
        }

        public void setAngle(float angle)
        {
            this.angle = angle;
            setBounds();
        }

        public void setSweep(float sweep)
        {
            this.sweep = sweep;
            setBounds();
        }

        public void increaseAngle(float amount)
        {
            this.angle += amount;
            setBounds();
        }

        public void increaseSweep(float amount)
        {
            this.sweep += amount;
            setBounds();
        }

        private void setBounds()
        {
            double radiansOne = (angle - sweep / 2.0f) * Math.PI / 180.0f;
            double radiansTwo = (angle + sweep / 2.0f) * Math.PI / 180.0f;

            boundOne = new Vector2f((float)Math.Cos(radiansOne) * 10000, (float)Math.Sin(radiansOne) * 10000);
            boundTwo = new Vector2f((float)Math.Cos(radiansTwo) * 10000, (float)Math.Sin(radiansTwo) * 10000);
        }
    }
}
