using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Framework.Abstract;
using SFML.Graphics;
using Framework.Utilities;
using Framework.Interface;

namespace Framework.Managers
{
    class ScreenManager : IHandleMouseButton
    {
        private List<Screen> screens { get; set; }
        private List<Screen> removeQueue;
        private List<Screen> addQueue;

        public ScreenManager()
        {
            screens = new List<Screen>();
            removeQueue = new List<Screen>();
            addQueue = new List<Screen>();
        }

        public void draw(GameTime time, RenderWindow window)
        {
            foreach (Screen s in screens)
            {
                if (s.IsDrawn)
                    s.draw(time, window);

                if (s.ChildScreens != null)
                {
                    foreach (Screen cs in s.ChildScreens)
                        if (cs.IsDrawn)
                            cs.draw(time, window);
                }
            }

        }

        public void update(GameTime time)
        {
            foreach (Screen s in removeQueue)
            {
                s.Dispose();
                screens.Remove(s);
            }
               
            removeQueue.Clear();

            foreach (Screen s in addQueue)
                screens.Add(s);
            addQueue.Clear();

            foreach (Screen s in screens)
            {
                if (s.IsUpdated)
                    s.update(time);

                if (s.ChildScreens != null)
                {
                    foreach (Screen cs in s.ChildScreens)
                        if (cs.IsUpdated)
                            cs.update(time);
                }
            }
        }

        public void handleMouseButton(SFML.Window.MouseButtonEventArgs e)
        {
            foreach (Screen s in screens)
            {
                IHandleMouseButton screen = s as IHandleMouseButton;
                if (screen != null && s.IsUpdated)
                    screen.handleMouseButton(e);

                if (s.ChildScreens != null)
                {
                    foreach (Screen cs in s.ChildScreens)
                    {
                        IHandleMouseButton childScreen = cs as IHandleMouseButton;
                        if (childScreen != null && cs.IsUpdated)
                            childScreen.handleMouseButton(e);
                    }
                }
            }
        }

        public void handleKeyPress(SFML.Window.KeyEventArgs e)
        {
            foreach (Screen s in screens)
            {
                IHandleKeyPress screen = s as IHandleKeyPress;
                if (screen != null && s.IsUpdated)
                    screen.handleKeyPress(e);

                if (s.ChildScreens != null)
                {
                    foreach (Screen cs in s.ChildScreens)
                    {
                        IHandleKeyPress childScreen = cs as IHandleKeyPress;
                        if (childScreen != null && cs.IsUpdated)
                            childScreen.handleKeyPress(e);
                    }
                }
            }
        }

        public void add(Screen s)
        {
            addQueue.Add(s);
        }

        public void remove(Screen s)
        {
            removeQueue.Add(s);
        }

    }
}
