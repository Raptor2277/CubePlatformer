using Cube_Platformer;
using Framework;
using Framework.Audio;
using Framework.Abstract;
using Framework.Blocks;
using Framework.Interface;
using Framework.Light;
using Framework.Managers;
using Framework.Utilities;
using SFML.Graphics;
using SFML.System;
using SFML.Window;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Framework.LevelEditor;
using FarseerPhysics.Dynamics;

namespace GameAssets.Screens
{
    class LevelEditorScreen : Screen, IHandleKeyPress, IHandleMouseButton
    {
        private ContentManager contentManager;
        private LightLayer lightLayer;
        //private MusicPlayer mPlayer;
        //private String level;

        private bool showGrid;
        private bool lightLayerEnabled;

        private GuideBlock guide;

        private MenuScreen pauseMenu;
        private SelectorPannel pannel;

        private System.Windows.Forms.OpenFileDialog ofd = new System.Windows.Forms.OpenFileDialog();
        private System.Windows.Forms.SaveFileDialog sfd = new System.Windows.Forms.SaveFileDialog();

        private Color gridColor = new Color(255, 255, 255, 40);
        private Color gridColorDark = new Color(255, 255, 255, 80);

        private View view;

        public LevelEditorScreen(Screen screen)
        {
            this.ParentScreen = screen;
            this.GameResolution = ParentScreen.GameResolution;
            this.WindowResolution = ParentScreen.WindowResolution;

            this.ChildScreens = new List<Screen>();

            this.IsDrawn = true;
            this.IsUpdated = true;

            //this.level = null;
            this.showGrid = true;
            //this.blocksPaused = true;

            this.lightLayer = new LightLayer(this.GameResolution, this.WindowResolution);
            this.contentManager = new ContentManager(Game1.defaultWorldParameters);

            this.guide = new GuideBlock(this.contentManager);
            this.view = new View(new FloatRect(0, 0, this.GameResolution.X, this.GameResolution.Y));

            loadMenus();
        }

        public void loadMenus()
        {
            this.pauseMenu = new MenuScreen(this, this.contentManager, "Settings", new Vector2i(0, 0), true, true, false);
            pauseMenu.addButtons(new MenuButton[]{
                new MenuButton("Continue"),
                new MenuButton("New Level"),
                new MenuButton("Load Level"),
                new MenuButton("Save Level"),
                new MenuButton("Exit"),
            });
            pauseMenu.IsDrawn = false;
            pauseMenu.IsUpdated = false;
            pauseMenu.MouseClick += menu_MouseClick;
            pauseMenu.MouseEnter += menu_MouseEnter;
            pauseMenu.MouseLeave += menu_MouseLeave;
            ChildScreens.Add(pauseMenu);

            this.pannel = new SelectorPannel(this, this.contentManager, this.guide);
            pannel.StateChanged += pannel_StateChanged;
            ChildScreens.Add(pannel);
        }

        void pannel_StateChanged(object sender, StateChangedEventArgs e)
        {
            if (e.state == SelectorPannel.States.showing)
                guide.IsActive = false;
            if (e.state == SelectorPannel.States.hidden)
                guide.IsActive = true;
        }

        private void menu_MouseClick(object sender, ButtonClickEventArgs e)
        {
            switch (e.ButtonIndex)
            {
                case 0:
                    pauseMenu.pause();
                    this.IsUpdated = true;
                    break;
                case 1:
                    contentManager.clear();
                    pauseMenu.pause();
                    this.IsUpdated = true;
                    break;
                case 2:
                    ofd.Filter = "XML|*.xml|All files|*.*";
                    ofd.InitialDirectory = @"C:\Users\Vasile\Desktop\c#\Cube Platformer\Cube Platformer\Content\levels";
                    Game1.Window.SetVisible(false);
                    if (ofd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                    {
                        IO.loadLevel(contentManager, ofd.FileName);
                        ofd.FileName = "";
                    }
                    pauseMenu.pause();
                    Game1.Window.SetVisible(true);
                    this.IsUpdated = true;
                    break;
                case 3:
                    sfd.Filter = "XML|*.xml|All files|*.*";
                    sfd.InitialDirectory = @"C:\Users\Vasile\Desktop\c#\Cube Platformer\Cube Platformer\Content\levels";
                    Game1.Window.SetVisible(false);
                    if (sfd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                    {
                        IO.saveLevel(sfd.FileName, contentManager);
                        sfd.FileName = "";
                    }
                    Game1.Window.SetVisible(true);
                    pauseMenu.pause();
                    this.IsUpdated = true;
                    break;
                case 4:
                    this.ParentScreen.pause();
                    Game1.ScreenManager.remove(this);
                    break;
            }
        }

        private void menu_MouseEnter(object sender, ButtonMouseEventArgs e)
        {
            e.Button.IsHighLighted = true;
        }

        private void menu_MouseLeave(object sender, ButtonMouseEventArgs e)
        {
            e.Button.IsHighLighted = false;
        }

        public override void update(GameTime time)
        {
            contentManager.update(time);
            lightLayer.update(contentManager);

            #region DeleteBlocks
            if (Mouse.IsButtonPressed(Mouse.Button.Right))
            {
                Vector2i pos = Game1.getMousePosition();
                MouseButtonEvent e = new MouseButtonEvent();
                e.Button = Mouse.Button.Right;
                e.X = pos.X;
                e.Y = pos.Y;
                this.handleMouseButton(new MouseButtonEventArgs(e));
            }
            #endregion

            this.guide.update(Game1.getMousePosition());
        }

        public override void draw(GameTime time, RenderWindow window)
        {
            window.SetView(view);
            if (showGrid)
                drawGrid(window);

            contentManager.draw(time, window);
            if (lightLayerEnabled)
                lightLayer.draw(window);

            guide.draw(window);
        }

        public void handleKeyPress(SFML.Window.KeyEventArgs e)
        {
            if (e.Code == Keyboard.Key.Escape)
            {
                this.IsUpdated = false;
                pauseMenu.pause();
            }

            if (e.Code == Keyboard.Key.Z)
            {
                Vector2i m = Game1.getMousePosition();
                lightLayer.add(new Light((Vector2f)m, Color.Yellow, .2f));
            }

            if (e.Code == Keyboard.Key.L)
                this.lightLayerEnabled = !lightLayerEnabled;

            if (e.Code == Keyboard.Key.R)
                this.contentManager.add(new Player(contentManager, Game1.getMousePosition().X, Game1.getMousePosition().Y, 32, 32));

            contentManager.handleKeyPress(e);
        }

        public void handleMouseButton(SFML.Window.MouseButtonEventArgs e)
        {
            guide.handleMouseButton(e);
        }

        public override void pause()
        {
            this.IsDrawn = !IsDrawn;
            this.IsUpdated = !IsUpdated;
        }

        private void clearLevel()
        {
            contentManager.clear();
        }

        private void drawGrid(RenderWindow window)
        {

            uint hLines = this.GameResolution.X / 32;
            uint vLines = this.GameResolution.Y / 32;

            int counter = 32;
            for (int i = 1; i < hLines; i++)
            {
                if (i % 5 == 0)
                    Draw.drawLine(window, new Vector2f(counter, 0), new Vector2f(counter, this.GameResolution.Y), gridColorDark);
                else
                    Draw.drawLine(window, new Vector2f(counter, 0), new Vector2f(counter, this.GameResolution.Y), gridColor);
                counter += 32;
            }

            counter = 32;
            for (int i = 1; i < vLines + 1; i++)
            {
                if (i % 5 == 0)
                    Draw.drawLine(window, new Vector2f(0, counter), new Vector2f(this.GameResolution.X, counter), gridColorDark);
                else
                    Draw.drawLine(window, new Vector2f(0, counter), new Vector2f(this.GameResolution.X, counter), gridColor);
                counter += 32;
            }
        }

        public override void Dispose()
        {
            this.ChildScreens.Clear();
            this.ChildScreens = null;

            this.contentManager.Dispose();
            this.contentManager = null;

            this.guide = null;

            this.lightLayer.Dispose();
            this.lightLayer = null;

            this.ofd.Dispose();
            this.ofd = null;

            this.pannel.Dispose();
            this.pannel = null;

            this.ParentScreen = null;

            this.pauseMenu.Dispose();
            this.pauseMenu = null;

            this.sfd.Dispose();
            this.sfd = null;

            this.view.Dispose();
            this.view = null;
        }
    }
}
