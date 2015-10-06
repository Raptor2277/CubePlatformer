using Framework.Abstract;
using Framework.Utilities;
using SFML.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Framework.Interface;
using SFML.System;
using Cube_Platformer;
using Framework.Managers;

namespace Framework.LevelEditor
{
    class StateChangedEventArgs : EventArgs
    {
        public SelectorPannel.States state { get; set; }
    }

    class SelectorPannel : Screen, IHandleMouseButton, IHandleKeyPress
    {
        public enum States
        {
            shown,
            hidden,
            hiding,
            showing

        }

        private States state;
        private GuideBlock guide;
        public Rectangle PositionBox { get; private set; }
        private List<SelectorButton> buttons;
        private SelectorButton currentButton;
        public Color Color { get; private set; }
        private ContentManager contentManager;

        public event EventHandler<StateChangedEventArgs> StateChanged;

        public SelectorPannel(Screen parent, ContentManager c,GuideBlock g)
        {
            this.ParentScreen = parent;
            this.GameResolution = parent.GameResolution;
            this.WindowResolution = parent.WindowResolution;
            this.contentManager = c;
            this.guide = g;
            this.IsDrawn = true;
            this.IsUpdated = true;
            this.ChildScreens = null;
            this.state = States.showing;
            this.currentButton = null;
            this.Color = new Color(200, 200, 200, 255);

            this.PositionBox = new Rectangle(0, -300, 1920, 400);
            addButtons();
        }

        public void addButtons()
        {
            Texture lightTexture = contentManager.Media.loadTexture("Content/images/levelEditor/light.png", true);
            Texture spotLightTexture = contentManager.Media.loadTexture("Content/images/levelEditor/spotlight.png", true);
            Texture tipTexture = contentManager.Media.loadTexture("Content/images/levelEditor/tip.png", true);

            this.buttons = new List<SelectorButton>();
            int x = 20;
            int y = 40;

            buttons.Add(new SelectorButton(this, SelectorButton.Types.Player, new SFML.System.Vector2f(x, y), new SFML.System.Vector2f(75, 75),"Player",Color.Red));
            y += 85;
            buttons.Add(new SelectorButton(this, SelectorButton.Types.Tile, new SFML.System.Vector2f(x, y), new SFML.System.Vector2f(75, 75), "Tile", Color.Green));
            y += 85;
            buttons.Add(new SelectorButton(this, SelectorButton.Types.ExitTile, new SFML.System.Vector2f(x, y), new SFML.System.Vector2f(75, 75), "ExitTile", Color.Blue));
            y += 85;
            buttons.Add(new SelectorButton(this, SelectorButton.Types.Tip, new SFML.System.Vector2f(x, y), new SFML.System.Vector2f(75, 75), "Tip", Color.White, tipTexture));

            y = 40;
            x += 85;
            buttons.Add(new SelectorButton(this, SelectorButton.Types.Light, new SFML.System.Vector2f(x, y), new SFML.System.Vector2f(75, 75), "Light", Color.White, lightTexture));
            y += 85;
            buttons.Add(new SelectorButton(this, SelectorButton.Types.Light, new SFML.System.Vector2f(x, y), new SFML.System.Vector2f(75, 75), "SpotLight", Color.White, spotLightTexture));
        }

        public override void update(Utilities.GameTime time)
        {
            Vector2i mouse = Game1.getMousePosition();
            switch (state)
            {
                case States.showing:
                    this.PositionBox.y += 25;
                    if (PositionBox.y > 0)
                    {
                        PositionBox.y = 0;
                        state = States.shown;
                        onStateChanged();
                    }
                    break;

                case States.hiding:
                    this.PositionBox.y -= 25;
                    if (PositionBox.y < -400)
                    {
                        PositionBox.y = -400;
                        state = States.hidden;
                        onStateChanged();
                        this.IsDrawn = false;
                    }
                    break;
            }

            foreach (SelectorButton b in buttons)
            {
                b.update();

                if (currentButton != null)
                {
                    if (Utils.checkMouseCollision(mouse.X, mouse.Y, b.positionBox) && !b.Equals(currentButton))
                    {
                        currentButton.onMouseLeave();
                        b.onMouseOver();
                        currentButton = b;
                    }
                }
                else if (Utils.checkMouseCollision(mouse.X, mouse.Y, b.positionBox))
                {
                    b.onMouseOver();
                    currentButton = b;
                }
            }

            if (currentButton != null && !Utils.checkMouseCollision(mouse.X, mouse.Y, currentButton.positionBox))
            {
                currentButton.onMouseLeave();
                currentButton = null;
            }
        }

        public override void draw(Utilities.GameTime time, SFML.Graphics.RenderWindow window)
        {
            if (IsDrawn)
            {
                Draw.fillRectangle(window, this.PositionBox, this.Color);
                foreach (SelectorButton b in buttons)
                    b.draw(window);
            }
        }

        public void toggle()
        {
            if (state == States.hidden || state == States.hiding)
            {
                if (state == States.hidden)
                    this.IsDrawn = true;
                state = States.showing;
                onStateChanged();
            }
            else if (state == States.showing || state == States.shown)
            {
                state = States.hiding;
                onStateChanged();
            }
        }

        public override void pause()
        {
            this.IsDrawn = !IsDrawn;
            this.IsUpdated = !IsUpdated;
        }

        public void handleMouseButton(SFML.Window.MouseButtonEventArgs e)
        {
            if(state == States.shown && currentButton != null)
            {
                guide.type = currentButton.Type;
                this.state = States.hiding;
            }
        }

        public void handleKeyPress(SFML.Window.KeyEventArgs e)
        {
            if (e.Code == SFML.Window.Keyboard.Key.Q)
                toggle();
        }

        public void onStateChanged()
        {
            if (StateChanged != null)
                StateChanged(this, new StateChangedEventArgs { state = this.state });
        }

        public override void Dispose()
        {
            foreach (SelectorButton b in buttons)
                b.Dispose();
            this.buttons.Clear();
            this.buttons = null;

            this.currentButton = null;

            this.guide = null;

            this.ParentScreen = null;
        }
    }
}
