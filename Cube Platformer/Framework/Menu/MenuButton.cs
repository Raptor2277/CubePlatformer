using Framework.Abstract;
using Framework.Utilities;
using SFML.Graphics;
using SFML.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Framework.Abstract
{
    public class MenuButton
    {
        public bool IsHighLighted { get; set; }
        public bool IsActive { get; set; }

        public Color BorderColor { get; set; }
        public uint CharacterSize { get; set; }

        public Text Text { get; set; }
        public Rectangle TextBox { get; set; }

        public MenuButton(String displayedText, Font font, Color color, uint size)
        {
            this.TextBox = new Rectangle();

            this.CharacterSize = size;
            this.Text = new Text(displayedText, font, CharacterSize);
            Text.Color = color;
            this.Text.Position = new SFML.System.Vector2f(100, 100);

            this.IsActive = true;
            this.IsHighLighted = false;
            this.BorderColor = color;
        }

        public void draw(RenderWindow window)
        {
            if (IsHighLighted)
                Draw.drawRectangle(window, TextBox, BorderColor);
            window.Draw(Text);
        }

        public float getWidth()
        {
            return Text.GetGlobalBounds().Width;
        }

        public float getHeight()
        {
            return Text.GetGlobalBounds().Height;
        }

        public void setPosition(Vector2f pos)
        {
            Text.Position = pos;

            float buggedY = Text.GetLocalBounds().Top;
            float buggedX = Text.GetLocalBounds().Left;

            Text.Position = new Vector2f(pos.X - buggedX, pos.Y - buggedY);

            float size = Text.CharacterSize / 5;
            TextBox = new Rectangle(pos.X - size, pos.Y - size, Text.GetGlobalBounds().Width + 2 * size, Text.GetGlobalBounds().Height + 2 * size);
        }
    }
}
