using Cube_Platformer;
using Framework.Utilities;
using SFML.Graphics;
using SFML.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Framework.LevelEditor
{
    class SelectorButton : IDisposable
    {
        public enum Types
        {
            Player,
            Tile,
            ExitTile,
            Tip,
            Light,
            PointLight,
        }

        public Types Type { get; private set; }
        public Rectangle positionBox;
        public Rectangle blockPositionBox;
        private Vector2f offset;
        private SelectorPannel parent;
        private Text text;

        private Color outlineDark;
        private Color outlineLight; 
        private Color currentOutline;
        private Color drawColor;

        private Sprite sprite;

        public SelectorButton(SelectorPannel parent, Types type, Vector2f offset, Vector2f dimensions, string name, Color c, Texture t = null)
        {
            this.parent = parent;
            this.Type = type;
            this.offset = offset;
            this.positionBox = new Rectangle(parent.PositionBox.x + offset.X, parent.PositionBox.y + offset.Y, dimensions.X, dimensions.Y);
            updateBlockParamers();
            this.text = new Text(name, Game1.plainFont);
            text.Color = Color.Black;
            text.CharacterSize = 24;
            text.Origin = new Vector2f(text.GetLocalBounds().Left, text.GetLocalBounds().Top);
            setTextPosition();

            this.outlineLight = new Color(255, 255, 255, 0);
            this.outlineDark = new Color(parent.Color.R, parent.Color.G, parent.Color.B, 220);
            this.currentOutline = outlineDark;
            this.drawColor = c;
            if (t != null)
                sprite = new Sprite(t);
        }

        public void update()
        {
            this.positionBox.y = parent.PositionBox.y + this.offset.Y;
            this.updateBlockParamers();
            this.updateSpriteParameters();
            this.setTextPosition();
        }

        public void draw(RenderWindow window)
        {
            drawType(window);
            window.Draw(text);
            Draw.fillRectangle(window, this.positionBox, currentOutline);
        }

        public void onMouseOver()
        {
            this.currentOutline = outlineLight;
        }

        public void onMouseLeave()
        {
            this.currentOutline = outlineDark;
        }

        private void drawType(RenderWindow w)
        {
            if (sprite == null)
                Draw.fillRectangle(w, blockPositionBox, drawColor);
            else
                w.Draw(sprite);
        }

        private void setTextPosition()
        {
            float offsetX = (this.positionBox.width - text.GetGlobalBounds().Width) / 2;         
            text.Position = new Vector2f(positionBox.x + offsetX, positionBox.y);
        }

        private void updateBlockParamers()
        {
            blockPositionBox = new Rectangle(positionBox.x + 13, positionBox.y +25, positionBox.width -25, positionBox.height - 25);
        }

        private void updateSpriteParameters()
        {
            if (sprite != null)
                sprite.Position = new Vector2f(positionBox.x + 13, positionBox.y + 25);
        }

        public void Dispose()
        {
            this.parent = null;
            if (sprite != null)
                sprite.Dispose();
            sprite = null;
            this.text.Dispose();
        }
    }
}
