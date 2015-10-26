using System;
using System.Collections.Generic;
using Framework.Managers;
using SFML.Graphics;
using Framework.Utilities;
using SFML.System;

namespace Framework.Abstract
{
    /// <summary>
    /// A representation of a game screen like a Main Menu the Game Screen.
    /// </summary>
    abstract class Screen : IDisposable
    {
        /// <summary>
        /// The screen that created this screen, sualy a Game Screen
        /// </summary>
        public Screen ParentScreen {  get;  protected set; }

        /// <summary>
        /// Any screns that this screen is parent to, intended to be used with MenuScreen
        /// </summary>
        public List<Screen> ChildScreens { get; protected set; }
        
        /// <summary>
        /// Game draw resolution
        /// </summary>
        public Vector2u GameResolution { get;  protected set; }

        /// <summary>
        /// Game window resolution
        /// </summary>
        public Vector2u WindowResolution { get; protected set; }


        /// <summary>
        /// Indicates if this screen is drawn
        /// </summary>
        public bool IsDrawn { get; set; }

        /// <summary>
        /// Indicates if this screen is updated
        /// </summary>
        public bool IsUpdated { get; set; }


        /// <summary>
        /// Draw the screen to the window
        /// </summary>
        /// <param name="time"></param>
        /// <param name="window"></param>
        public abstract void draw(GameTime time, RenderWindow window);


        /// <summary>
        /// Update the screen
        /// </summary>
        /// <param name="time"></param>
        public abstract void update(GameTime time);

        /// <summary>
        /// Pause the screen
        /// </summary>
        public abstract void pause();

        /// <summary>
        /// Dispose resources
        /// </summary>
        public abstract void Dispose();
    }
}
