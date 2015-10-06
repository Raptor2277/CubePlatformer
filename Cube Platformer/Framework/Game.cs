using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFML.Graphics;
using SFML.Window;
using Framework.Utilities;
using Framework.Managers;

namespace Framework
{
    class Game 
    {
        
        #region Window
        protected VideoMode videoMode;
        protected String title;
        protected Styles style; 
        protected ContextSettings context;
        #endregion

        #region Runtime
        public static ScreenManager ScreenManager { get; set; }

        public static RenderWindow Window { get; set; }
        #endregion

        public Game()
        {
            videoMode = new VideoMode(960, 540);
            title = "SFML Game Window";
            style = Styles.Close;
            context = new ContextSettings();

            ScreenManager = new ScreenManager();
        }

        public virtual void intialize(RenderWindow window)
        {
            
        }

        public virtual void loadContent()
        {

        }

        public virtual void unloadContent()
        {
            Utils.log("unloaded contet");
        }

        public virtual void update(GameTime time)
        {

        }

        public virtual void draw(GameTime time, RenderWindow window)
        {
            
        }

        public void run()
        {
            createWindow(videoMode,title, style, context);
            intialize(Window);
            loadContent();

            GameTime time =  new GameTime();
            long timeSnap = 0;

            while (Window.IsOpen)
            {
                Window.DispatchEvents();
                time.frameTime = (time.gameTime.ElapsedTime.AsMicroseconds() - (float)timeSnap) / 1000000;
                timeSnap = time.gameTime.ElapsedTime.AsMicroseconds();
                update(time);
                draw(time, Window);
            }

            unloadContent();
        }

        public virtual void createWindow(VideoMode mode, String title, Styles styles, ContextSettings context)
        {
            Window = new RenderWindow(mode, title, styles, context);
            Window.Closed += window_Closed;
        }

        private void window_Closed(object sender, EventArgs e)
        {
            Window win = (Window)sender;
            win.Close();
        }
    }
}
