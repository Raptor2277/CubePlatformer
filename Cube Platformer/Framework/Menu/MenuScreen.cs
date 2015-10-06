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
        protected List<MenuButton> buttons;
        private MenuButton currentButton;

        public Color ButtonColor { get; set; }
        public Color BackGroudColor { get; set; }
        public Font TitleFont { get; set; }
        public Font ButtonFont { get; set; }
        public uint TitleSize { get; set; }
        public uint ButtonSize { get; set; }
        public Text Title { get; set; }

        public bool isBordered;
        public bool isCenteredX;
        public bool isCenteredY;
        public bool hasBackground;
        public Rectangle bounds;

        public event EventHandler<ButtonClickEventArgs> MouseClick;
        public event EventHandler<ButtonMouseEventArgs> MouseEnter;
        public event EventHandler<ButtonMouseEventArgs> MouseLeave;

        public MenuScreen(Screen parentScreen, ContentManager m, string title, Vector2i pos)
            : this(parentScreen, m, title, pos, true, true, false, false, Color.White) { }

        public MenuScreen(Screen parentScreen, ContentManager m, string title, Vector2i pos, bool centeredX, bool centeredY, bool bordered)
            : this(parentScreen, m, title, pos, centeredX, centeredY, bordered, false, Color.White) { }

        public MenuScreen(Screen parentScreen, ContentManager m, string title, Vector2i pos, bool centeredX, bool centeredY, bool bordered, bool hasBackground, Color backgroundColor)
        {
            this.ParentScreen = parentScreen;

            this.ButtonColor = Color.Black;
            this.BackGroudColor = backgroundColor;
            this.TitleFont = m.Media.loadFont("Content/fonts/sweetness.ttf");
            this.ButtonFont = m.Media.loadFont("Content/fonts/sweetness.ttf");
            this.TitleSize = 90;
            this.ButtonSize = 55;
            this.buttons = new List<MenuButton>();

            this.Title = new Text(title, TitleFont, TitleSize);
            this.bounds = new Rectangle(pos.X, pos.Y, 0,0);

            this.isBordered = bordered;
            this.isCenteredX = centeredX;
            this.isCenteredY = centeredY;
            this.IsDrawn = true;
            this.IsUpdated = true;

            this.hasBackground = hasBackground;
            this.BackGroudColor = backgroundColor;

            setButtons();
        }

        public void addButtons(string[] buttons)
        {
            foreach (string s in buttons)
                this.buttons.Add(new MenuButton(s, ButtonFont, ButtonColor, ButtonSize));
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
            window.Draw(Title);
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
            float offset = 0;
            if (!string.IsNullOrEmpty(Title.DisplayedString)  && TitleSize > ButtonSize)
                offset = TitleSize / 2;
            else
                offset = ButtonSize / 2;

            float maxLength = 0;
            float totalHeight = 0;
            if (!string.IsNullOrEmpty(Title.DisplayedString))
            {
                maxLength = Title.GetGlobalBounds().Width;
                totalHeight += Title.GetLocalBounds().Height;
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
            setTitlePosition(new Vector2f(bounds.x + bounds.width / 2 - Title.GetGlobalBounds().Width / 2, this.bounds.y + offset));
            height += 2 * offset + Title.GetGlobalBounds().Height;

            foreach (MenuButton b in buttons)
            {
                b.setPosition(new Vector2f(bounds.x + bounds.width / 2 - b.getWidth() / 2, height));
                height += offset + b.getHeight();
            }
        }

        private void setTitlePosition(Vector2f pos)
        {
            Title.Position = pos;

            float buggedY = Title.GetLocalBounds().Top;
            float buggedX = Title.GetLocalBounds().Left;

            Title.Position = new Vector2f(pos.X - buggedX, pos.Y - buggedY);
        }

        private void onButtonClick(int index)
        {
            if (MouseClick != null)
                MouseClick(this, new ButtonClickEventArgs() {ButtonIndex = index});
            Console.WriteLine(index);
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
