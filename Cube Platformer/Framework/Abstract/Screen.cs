using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFML.Graphics;
using Framework.Utilities;
using SFML.System;
using Framework.Managers;

namespace Framework.Abstract
{
    abstract class Screen : IDisposable
    {
        public Screen ParentScreen {  get;  protected set; }
        public List<Screen> ChildScreens { get; protected set; }
        
        public Vector2u GameResolution { get;  protected set; }
        public Vector2u WindowResolution { get; protected set; }

        public bool IsDrawn { get; set; }
        public bool IsUpdated { get; set; }

        public abstract void draw(GameTime time, RenderWindow window);

        public abstract void update(GameTime time);

        public abstract void pause();

        public abstract void Dispose();
    }
}
