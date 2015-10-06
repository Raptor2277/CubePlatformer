using Framework.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFML.Graphics;
using SFML.Window;
using Framework.Utilities;
using Framework.Managers;
using Framework.Blocks;
using SFML.System;
using Framework.Interface;
using System.IO;
using Framework.Light;
using Framework;
using Cube_Platformer;
using FarseerPhysics.Dynamics;
using Microsoft.Xna.Framework;

namespace GameAssets.Screens
{
    class GameScreen : Screen, IHandleMouseButton, IHandleKeyPress
    {
        private ContentManager contentManager;
        private LightLayer lightLayer;
        private int currentLevel;

        private MenuScreen winScreen;
        private MenuScreen pauseScreen;

        private Sprite sp;

        private byte lol = 0;
        private bool enabled = true;

        public GameScreen(Screen parentScreen)
        {
            this.GameResolution = parentScreen.GameResolution;
            this.WindowResolution = parentScreen.WindowResolution;
            this.ParentScreen = parentScreen;

            this.IsDrawn = true;
            this.IsUpdated = true;

            this.contentManager = new ContentManager(Game1.defaultWorldParameters);
            contentManager.LevelWon += contentManager_LevelWon;

            IO.loadLevel(this.contentManager, "Content/levels/level1.xml");
            this.currentLevel = 1;

            this.lightLayer = new LightLayer(this.GameResolution, this.WindowResolution);
            lightLayer.Lights.Add(new Light(new Vector2f(100, 100), Color.White, .4f));

            this.sp = new Sprite(contentManager.Media.loadTexture("Content/images/bg.jpg", true));
            sp.Scale = new Vector2f(this.GameResolution.X / (float)sp.TextureRect.Width, this.GameResolution.Y / (float)sp.TextureRect.Height);

            loadMenuScreens();
        }

        void contentManager_LevelWon(object sender, EventArgs e)
        {
            this.winScreen.pause();
            this.IsUpdated = false;
        }

        public override void update(Framework.Utilities.GameTime time)
        {
            contentManager.update(time);
            lightLayer.update(contentManager);
        }

        public override void draw(Framework.Utilities.GameTime time, SFML.Graphics.RenderWindow window)
        {
            window.Draw(sp);
            contentManager.draw(time, window);
            if (enabled)
                lightLayer.draw(window);
        }

        public override void pause()
        {
            this.IsDrawn = !IsDrawn;
            this.IsUpdated = !IsUpdated;
        }

        public void handleMouseButton(MouseButtonEventArgs e)
        {
            if (e.Button == Mouse.Button.Right)
                lightLayer.add(new Light(new Vector2f(e.X, e.Y), Color.White, .4f));
            if (e.Button == Mouse.Button.Left)
                lightLayer.add(new SpotLight(new Vector2f(e.X, e.Y), new Color(100,100,255,255), .4f, 90, 75));
            contentManager.handleMouseButton(e);
        }

        public void handleKeyPress(KeyEventArgs e)
        {
            if (Keyboard.IsKeyPressed(Keyboard.Key.Escape))
            {
                pauseScreen.pause();
                this.IsUpdated = false;
            }
            if (Keyboard.IsKeyPressed(Keyboard.Key.L))
            {
                lightLayer.Lights.Clear();
            }

            if (Keyboard.IsKeyPressed(Keyboard.Key.Insert))
            {
                lol += 5;
                lightLayer.setAmbientLight(new Color(lol, lol, lol));
            }

            if (Keyboard.IsKeyPressed(Keyboard.Key.Delete))
            {
                lol -= 5;
                lightLayer.setAmbientLight(new Color(lol, lol, lol));
            }

            if (Keyboard.IsKeyPressed(Keyboard.Key.E))
            {
                enabled = !enabled;
            }

            if (Keyboard.IsKeyPressed(Keyboard.Key.Num1))
                foreach (Light l in lightLayer.Lights)
                    if (l is SpotLight)
                    {
                        SpotLight li = l as SpotLight;
                        li.increaseAngle(5f);
                        Console.WriteLine("derp");
                    }

            if (Keyboard.IsKeyPressed(Keyboard.Key.Num2))
                foreach (Light l in lightLayer.Lights)
                    if (l is SpotLight)
                    {
                        SpotLight li = l as SpotLight;
                        li.increaseAngle(5f);
                    }

            if (Keyboard.IsKeyPressed(Keyboard.Key.Num3))
                foreach (Light l in lightLayer.Lights)
                    if (l is SpotLight)
                    {
                        SpotLight li = l as SpotLight;
                        li.increaseSweep(5f);
                    }

            if (Keyboard.IsKeyPressed(Keyboard.Key.Num4))
                foreach (Light l in lightLayer.Lights)
                    if (l is SpotLight)
                    {
                        SpotLight li = l as SpotLight;
                        li.increaseSweep(5f);
                    }

            contentManager.handleKeyPress(e);
        }

        public void loadMenuScreens()
        {
            this.ChildScreens = new List<Screen>();

            this.winScreen = new MenuScreen(this, this.contentManager, "Level Won", new Vector2i(0, 0), true, true, false);
            winScreen.addButtons(new string[] { "Next Level",  "Restart",  "Quit"});
            winScreen.IsDrawn = false;
            winScreen.IsUpdated = false;
            winScreen.MouseEnter += menuScreen_MouseEnter;
            winScreen.MouseLeave += menuScreen_MouseLeave;
            winScreen.MouseClick += menuScreen_MouseClick;
            ChildScreens.Add(winScreen);

            this.pauseScreen = new MenuScreen(this, this.contentManager, "Game Paused", new Vector2i(0, 0), true, true, false);
            pauseScreen.addButtons(new string[] {  "Continue",  "Restart",  "Quit" });
            pauseScreen.IsDrawn = false;
            pauseScreen.IsUpdated = false;
            pauseScreen.MouseEnter += menuScreen_MouseEnter;
            pauseScreen.MouseLeave += menuScreen_MouseLeave;
            pauseScreen.MouseClick += menuScreen_MouseClick;
            ChildScreens.Add(pauseScreen);
        }

        private void menuScreen_MouseClick(object sender, ButtonClickEventArgs e)
        {
            MenuScreen m = ((MenuScreen)sender);
            if (m.Title.DisplayedString.Equals("Level Won"))
            {
                if (e.ButtonIndex == 0)
                {
                    m.pause();
                    currentLevel++;
                    IO.loadLevel(contentManager, "Content/levels/level" + currentLevel + ".xml");
                    this.IsUpdated = true;
                    Console.WriteLine("this fuckig thing ran");
                }
            }
            else if (m.Title.DisplayedString.Equals("Game Paused"))
            {
                if (e.ButtonIndex == 0)
                {
                    m.pause();
                    this.IsUpdated = true;
                }
            }

            if (e.ButtonIndex == 1)
            {
                foreach (Entity entity in contentManager.Entities)
                    if (entity is Player)
                        ((Player)entity).respawn();
                m.pause();
                this.IsUpdated = true;
            }
            if (e.ButtonIndex == 2)
            {
                ParentScreen.pause();
                Game1.ScreenManager.remove(this);
            }

        }

        private void menuScreen_MouseLeave(object sender, ButtonMouseEventArgs e)
        {
            e.Button.IsHighLighted = false;
        }

        private void menuScreen_MouseEnter(object sender, ButtonMouseEventArgs e)
        {
            e.Button.IsHighLighted = true;
        }

        public override void Dispose()
        {
            ParentScreen = null;

            foreach (Screen s in ChildScreens)
                s.Dispose();
            ChildScreens.Clear();
            ChildScreens = null;

            this.contentManager.Dispose();
            this.contentManager = null;

            this.lightLayer.Dispose();
            this.lightLayer = null;
        }
    }
}
