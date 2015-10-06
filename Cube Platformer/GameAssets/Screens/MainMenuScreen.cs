using Framework.Abstract;
using SFML.System;
using SFML.Graphics;
using SFML.Audio;
using Framework.Managers;
using Framework.Interface;
using Framework.Utilities;
using Framework.Blocks;
using Framework.Light;
using Framework.Audio;
using SFML.Window;
using Cube_Platformer;
using Microsoft.Xna.Framework;
using FarseerPhysics.Dynamics;

namespace GameAssets.Screens
{
    class MainMenuScreen : Screen, IHandleMouseButton
    {
        private MenuScreen mainMenu;
        private ContentManager contentManager;
        private LightLayer lightLayer;
        private MusicPlayer mPlayer;
        private Sprite backGround;

        public MainMenuScreen(Vector2u gameResolution, Vector2u windowResolution)
        {
            this.GameResolution = gameResolution;
            this.WindowResolution = windowResolution;
            this.IsDrawn = true;
            this.IsUpdated = true;
            this.ParentScreen = null;

            //mPlayer = new MusicPlayer();
            //mPlayer.add("bg", new Music(@"Content/music/never_short.ogg"));
            //mPlayer.currentMusic.Volume = 50;
            //mPlayer.currentMusic.Loop = true;
            //mPlayer.play();

            contentManager = new ContentManager(Game1.defaultWorldParameters);
            IO.loadLevel(contentManager, @"Content/levels/menuLevel.xml");

            backGround = new Sprite(contentManager.Media.loadTexture("Content/images/bg.jpg", true));
            backGround.Scale = new Vector2f(gameResolution.X / (float)backGround.Texture.Size.X, gameResolution.Y / (float)backGround.Texture.Size.Y);

            lightLayer = new LightLayer(this.GameResolution, this.WindowResolution);
            lightLayer.add(new Light(new Vector2f(700, 350), new Color(255, 255, 200), .55f));
            lightLayer.add(new Light(new Vector2f(1920 - 700, 350), new Color(255, 255, 200), .55f));

            contentManager.foreceBlocks();
            lightLayer.setPolygons(contentManager.getLightPolygons());

            mainMenu = new MenuScreen(this, this.contentManager, null, new Vector2i(0, 0), true, true, false);
            mainMenu.Title.Color = Color.Black;
            mainMenu.ButtonColor = Color.Black;
            mainMenu.addButtons(new string[] { "Play",  "Level Editor", "Options",  "Help", "Credits", "Quit"});
            mainMenu.MouseClick += mainMenu_ButtonClick;
            mainMenu.MouseEnter += mainMenu_MouseEnter;
            mainMenu.MouseLeave += mainMenu_MouseLeave;
        }

        void mainMenu_MouseLeave(object sender, ButtonMouseEventArgs e)
        {
            e.Button.IsHighLighted = false;
        }

        void mainMenu_MouseEnter(object sender, ButtonMouseEventArgs e)
        {
            e.Button.IsHighLighted = true;
        }

        void mainMenu_ButtonClick(object sender, ButtonClickEventArgs e)
        {
            switch (e.ButtonIndex)
            {
                case 0:
                    this.pause();
                    Game1.ScreenManager.add(new GameScreen(this));
                    break;
                case 1:
                    this.pause();
                    Game1.ScreenManager.add(new LevelEditorScreen(this));
                    break;
                case 5:
                    Game1.closeApplication();
                    break;
            }
        }

        public override void update(Framework.Utilities.GameTime time)
        {
            mainMenu.update(time);

            int pos = Game1.getMousePosition().Y;
            if (pos < 200)
                pos = 200;
            if (pos > 750)
                pos = 750;
            foreach (Light l in lightLayer.Lights)
                l.pos.Y = pos; ;
        }

        public override void draw(Framework.Utilities.GameTime time, SFML.Graphics.RenderWindow window)
        {
            window.Draw(backGround);
            contentManager.draw(time, window);
            lightLayer.draw(window);
            mainMenu.draw(time, window);
        }

        public override void pause()
        {
            this.IsDrawn = !IsDrawn;
            this.IsUpdated = !IsUpdated;
        }

        public void handleMouseButton(SFML.Window.MouseButtonEventArgs e)
        {
            mainMenu.handleMouseButton(e);
        }

        public override void Dispose()
        {

            this.contentManager.Dispose();
            this.contentManager = null;

            this.lightLayer.Dispose();
            this.lightLayer = null;

            this.mainMenu.Dispose();
            this.mainMenu = null;

            this.mPlayer = null;
            this.ParentScreen = null;
        }
    }
}
