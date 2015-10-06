using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Framework.Utilities;
using Framework.Interface;
using SFML.Graphics;
using SFML.System;
using Cube_Platformer;
using Framework.Managers;

namespace Framework.Abstract
{
    public class ButtonClickEventArgs : EventArgs
    {
        public int ButtonIndex { get; set; }
    }

    public class ButtonMouseEventArgs : EventArgs
    {
        public MenuButton Button { get; set; }
    }

    class MenuScreen : Screen, IHandleMouseButton
    {
        public static Font basicFont = new Font("Content/fonts/sweetness.ttf");
        public static uint basicTitleSize = 90;
        public static uint basicButtonSize = 55;

        protected List<MenuButton> buttons;
        private MenuButton currentButton;

        public string Title { get; set; }
        public Color TitleColor { get; set; }
        public Color ButtonColor { get; set; }
        public Font TitleFont { get; set; }
        public Font ButtonFont { get; set; }
        public uint TitleSize { get; set; }
        public uint ButtonSize { get; set; }
        public Text TitleText { get; set; }

        public bool isBordered;
        public bool isCenteredX;
        public bool isCenteredY;
        public Rectangle bounds;

        public event EventHandler<ButtonClickEventArgs> MouseClick;
        public event EventHandler<ButtonMouseEventArgs> MouseEnter;
        public event EventHandler<ButtonMouseEventArgs> MouseLeave;

        public MenuScreen(Screen parentScreen, ContentManager m, string title, Vector2i pos, bool centeredX, bool centeredY, bool bordered)
        {
            this.ParentScreen = parentScreen;
            this.Title = title;

            this.TitleColor = Color.Red;
            this.ButtonColor = Color.Black;
            this.TitleFont = m.Media.loadFont("Content/fonts/sweetness.ttf");
            this.ButtonFont = m.Media.loadFont("Content/fonts/sweetness.ttf");
            this.TitleSize = basicTitleSize;
            this.ButtonSize = basicButtonSize;
            this.buttons = new List<MenuButton>();

            this.TitleText = new Text(title, TitleFont, TitleSize);
            this.bounds = new Rectangle(pos.X, pos.Y, 0,0);

            this.isBordered = bordered;
            this.isCenteredX = centeredX;
            this.isCenteredY = centeredY;
            this.IsDrawn = true;
            this.IsUpdated = true;

            setButtons();
        }

        public void addButtons(MenuButton[] buttons)
        {
            foreach (MenuButton b in buttons)
                this.buttons.Add(b);
            setButtons();
        }

        public override void draw(Utilities.GameTime time, SFML.Graphics.RenderWindow window)
        {
            if (isBordered)
                Draw.drawRectangle(window, bounds, Color.Black);
            window.Draw(TitleText);
            foreach (MenuButton b in buttons)
                b.draw(window);
        }

        public override void update(Utilities.GameTime time)
        {
            Vector2i mouse = Game1.getMousePosition();

            foreach (MenuButton b in buttons)
            {
                if (currentButton != null)
                {
                    if (Utils.checkMouseCollision(mouse.X, mouse.Y, b.TextBox) && !b.Equals(currentButton))
                    {
                        onMouseLeave(currentButton);
                        onMouseEnter(b);
                        currentButton = b;
                    }
                }
                else if (Utils.checkMouseCollision(mouse.X, mouse.Y, b.TextBox))
                {
                    onMouseEnter(b);
                    currentButton = b;
                }
            }

            if (currentButton != null && !Utils.checkMouseCollision(mouse.X, mouse.Y, currentButton.TextBox))
            {
                onMouseLeave(currentButton);
                currentButton = null;
            }
        }

        private void onMouseEnter(MenuButton b)
        {
            if (MouseEnter != null)
                MouseEnter(this, new ButtonMouseEventArgs { Button = b });
        }

        private void onMouseLeave(MenuButton b)
        {
            if (MouseLeave != null)
                MouseLeave(this, new ButtonMouseEventArgs { Button = b });
        }

        public override void pause()
        {
            this.IsDrawn = !IsDrawn;
            this.IsUpdated = !IsUpdated;
        }

        private void setButtons()
        {
          //TitleSize > ButtonSize ? TitleSize / 2 : ButtonSize / 2;

            float offset = 0;
            if (Title != null && TitleSize > ButtonSize)
                offset = TitleSize / 2;
            else
                offset = ButtonSize / 2;

            float maxLength = 0;
            float totalHeight = 0;
            if(Title != null)
            {
                maxLength = TitleText.GetGlobalBounds().Width;
                totalHeight += TitleText.GetLocalBounds().Height;
            }
                
            foreach (MenuButton b in buttons)
            {
                if (b.getWidth() > maxLength)
                    maxLength = b.getWidth();
                totalHeight += b.getHeight();
            }

            this.bounds.width = maxLength + 2 * offset; // | 12px  |Button|  12px  |
            totalHeight += (buttons.Count * offset) + 2 * offset;
            this.bounds.height = totalHeight;

            if (isCenteredX)
                this.bounds.x = ParentScreen.GameResolution.X / 2 - this.bounds.width / 2;
            if(isCenteredY)
                this.bounds.y = ParentScreen.GameResolution.Y / 2 - this.bounds.height / 2;

            //setting the position of the title and buttons based on the possition of the menu
            float height = bounds.y;
            setTitlePosition(new Vector2f(bounds.x + bounds.width / 2 - TitleText.GetGlobalBounds().Width / 2, this.bounds.y + offset));
            height += 2 * offset + TitleText.GetGlobalBounds().Height;

            foreach (MenuButton b in buttons)
            {
                b.setPosition(new Vector2f(bounds.x + bounds.width / 2 - b.getWidth() / 2, height));
                height += offset + b.getHeight();
            }
        }

        private void setTitlePosition(Vector2f pos)
        {
            TitleText.Position = pos;

            float buggedY = TitleText.GetLocalBounds().Top;
            float buggedX = TitleText.GetLocalBounds().Left;

            TitleText.Position = new Vector2f(pos.X - buggedX, pos.Y - buggedY);
        }

        private void onButtonClick(int index)
        {
            if (MouseClick != null)
                MouseClick(this, new ButtonClickEventArgs() {ButtonIndex = index});
        }

        public void handleMouseButton(SFML.Window.MouseButtonEventArgs e)
        {
            if (currentButton != null)
                onButtonClick(buttons.IndexOf(currentButton));
        }

        public override void Dispose()
        {
            this.ButtonFont.Dispose();
            this.ButtonFont = null;

            this.ParentScreen = null;

            this.TitleFont.Dispose();
            this.TitleFont = null;

        }
    }
}
