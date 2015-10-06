using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFML.Graphics;
using SFML.Window;
using SFML.System;
using Framework.Utilities;
using GameAssets.Screens;
using Framework;
using Microsoft.Xna.Framework;

namespace Cube_Platformer
{
    class Game1 : Game
    {
        public static Vector2u drawResolution = new Vector2u(1920, 1080);
        public static Vector2u screenResolution = new Vector2u(1600, 900);
        
        public static uint Ppm = 100; //100 pixels per meter(simulation)
        public static Vector2 gravity = new Vector2(0, 9.81f);
        public static WorldParameters defaultWorldParameters = new WorldParameters(Game1.gravity, Game1.Ppm);

        public static Font plainFont = new Font("Content/fonts/sweetness.ttf");

        private bool fullScreen = true;

        public static Vector2i getMousePosition()
        {
            Vector2i m = Mouse.GetPosition(Window);
            return new Vector2i((int)(m.X * ((float)drawResolution.X / screenResolution.X)), (int)(m.Y * ((float)drawResolution.Y / screenResolution.Y)));
        }

        public override void createWindow(SFML.Window.VideoMode mode, string title, SFML.Window.Styles styles, SFML.Window.ContextSettings context)
        {
            if (fullScreen)
            {
                screenResolution = new Vector2u(VideoMode.DesktopMode.Width, VideoMode.DesktopMode.Height);
                styles = Styles.None;
            }
            else
            {
                styles = Styles.Close;
            }
            context.AntialiasingLevel = 4;
            title = "Platformer";
            mode = new VideoMode(Game1.screenResolution.X, Game1.screenResolution.Y);
            base.createWindow(mode, title, styles, context);
        }

        public override void intialize(RenderWindow window)
        {
            base.intialize(window);
            window.SetView(new View(new FloatRect(0, 0, Game1.drawResolution.X, Game1.drawResolution.Y)));
            window.SetFramerateLimit(60);
            window.MouseButtonPressed += window_MouseButtonPressed;
            window.KeyPressed += window_KeyPressed;
            window.Resized += window_Resized;
        }

        void window_Resized(object sender, SizeEventArgs e)
        {
            RenderWindow w = (RenderWindow)sender;
        }

        public override void loadContent()
        {
            base.loadContent();
            ScreenManager.add(new MainMenuScreen(Game1.drawResolution, Game1.screenResolution));
        }

        public override void unloadContent()
        {
            base.unloadContent();
        }

        public override void update(GameTime time)
        {
            base.update(time);
            ScreenManager.update(time);
        }

        public override void draw(GameTime time, RenderWindow window)
        {
            window.Clear(Color.Black);
            ScreenManager.draw(time, window);
            Log.draw(window);
            window.Display();
        }

        private void window_MouseButtonPressed(object sender, MouseButtonEventArgs e)
        {
            Vector2i corrected = getMousePosition();
            e.X = corrected.X;
            e.Y = corrected.Y;
            ScreenManager.handleMouseButton(e);
        }

        private void window_KeyPressed(object sender, KeyEventArgs e)
        {
            if (e.Code == Keyboard.Key.F1)
                Log.IsLogging = !Log.IsLogging;
            ScreenManager.handleKeyPress(e);
        }

        public static void closeApplication()
        {
            Window.Close();
        }
    }
}
