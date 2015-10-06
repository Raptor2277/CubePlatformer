using FarseerPhysics.Dynamics;
using Framework.Abstract;
using Framework.Blocks;
using Framework.Interface;
using Framework.Utilities;
using Framework.Media;
using Microsoft.Xna.Framework;
using SFML.Graphics;
using SFML.System;
using SFML.Window;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Framework.Managers
{
    class ContentManager : IDisposable
    {
        public MediaManager Media { get; private set; }

        public World World { get; private set; }
        public uint Ppm { get; private set; }

        public List<Entity> Entities { get; private set; }
        public List<Entity> EntityRemoveQueue { get; private set; }
        public List<Entity> EntityAddQueue { get; private set; }
        public List<int> PlayerIds { get; private set; }

        public bool EntitiesClear { get; private set; }

        public bool IsLevelWon { get; private set; }
        public event EventHandler LevelWon;
        public void onLevelWon()
        {
            IsLevelWon = true;
        }

        /// <summary>
        /// Initiate the ContentManger and the World
        /// </summary>
        public ContentManager(WorldParameters param)
        {
            this.Media = new MediaManager();

            this.Entities = new List<Entity>();
            this.EntityAddQueue = new List<Entity>();
            this.EntityRemoveQueue = new List<Entity>();
            this.PlayerIds = new List<int>();

            this.Ppm = param.Ppm;
            this.World = new World(param.Gravity);

            this.EntitiesClear = false;
            this.IsLevelWon = false;
        }

        /// <summary>
        /// Draw all the entities
        /// </summary>
        public void draw(GameTime time, RenderWindow window)
        {
            foreach (Entity e in Entities)
                if (e.IsDrawn)
                    e.draw(time, window);
        }

        /// <summary>
        /// Update the enties, world, events, and adds/removes entities from queue
        /// </summary>
        public void update(GameTime time)
        {
            add_removeBlocks();
            foreach (Entity e in Entities)
                if (e.IsActive)
                    e.update(time);

            World.Step(time.frameTime);

            if (IsLevelWon)
            {
                if (LevelWon != null)
                    LevelWon(this, EventArgs.Empty);
                IsLevelWon = false;
            }
        }

        /// <summary>
        /// Sends mouse input to all the entites that are IHandleMouseButton
        /// </summary>
        public void handleMouseButton(MouseButtonEventArgs e)
        {
            foreach (Entity entity in Entities)
            {
                IHandleMouseButton handles = entity as IHandleMouseButton;
                if (handles != null && entity.IsActive)
                    handles.handleMouseButton(e);
            }
        }

        /// <summary>
        /// Sebds keyboard input to all the entities that re IHandleKeyPress
        /// </summary>
        public void handleKeyPress(KeyEventArgs e)
        {
            foreach (Entity entity in Entities)
            {
                IHandleKeyPress handles = entity as IHandleKeyPress;
                if (handles != null && entity.IsActive)
                    handles.handleKeyPress(e);
            }
        }

        #region Getters
        /// <summary>
        /// Gets all the static static entities
        /// </summary>
        /// <returns></returns>
        public List<Entity> getStaticEntites()
        {
            List<Entity> staticEntities = new List<Entity>();

            foreach (Entity e in Entities)
            {
                if (e.Body.BodyType == BodyType.Static)
                    staticEntities.Add(e);
            }

            return staticEntities;
        }

        /// <summary>
        /// Gets all the static dynamic entities
        /// </summary>
        /// <returns></returns>
        public List<Entity> getDynamicEntities()
        {
            List<Entity> dynamicEntities = new List<Entity>();

            foreach (Entity e in Entities)
            {
                if (e.Body.BodyType == BodyType.Dynamic)
                    dynamicEntities.Add(e);
            }

            return dynamicEntities;
        }

        /// <summary>
        /// Gets the a list of rectanlges that represents the position of all entities
        /// </summary>
        /// <returns></returns>
        public List<Polygon> getLightPolygons()
        {
            List<Polygon> polygons = new List<Polygon>();

            foreach (Entity e in Entities)
                if (e is IBlocksLight)
                    polygons.Add(((IBlocksLight)e).getLightPolygon());

            return polygons;
        }

        public bool isPlayer(int bodyId)
        {
            return PlayerIds.Contains(bodyId);
        }

        #endregion

        #region Add/Remove
        /// <summary>
        /// Add entity to the addQueue
        /// </summary>
        /// <param name="e"></param>
        public void add(Entity e)
        {
            EntityAddQueue.Add(e);
        }

        /// <summary>
        /// Add enetity e to the removeQueue
        /// </summary>
        /// <param name="e"></param>
        public void remove(Entity e)
        {
            EntityRemoveQueue.Add(e);
        }

        /// <summary>
        /// Set the EntityClear flag to true
        /// </summary>
        public void clear()
        {
            EntitiesClear = true;
        }

        /// <summary>
        /// Force the add/remove blocks
        /// </summary>
        public void foreceBlocks()
        {
            add_removeBlocks();
        }

        private void add_removeBlocks()
        {
            if (EntitiesClear)
            {
                foreach (Entity e in Entities)
                    World.RemoveBody(e.Body);
                Entities.Clear();
                EntitiesClear = false;
                PlayerIds.Clear();
            }
            else
            {
                foreach (Entity e in EntityRemoveQueue)
                {
                    Entities.Remove(e);
                    World.RemoveBody(e.Body);
                    if(e is Player)
                        PlayerIds.Remove(e.Body.BodyId);
                }
            }

            //dont need to add body as it's added by the Entity upon the creation of the body
            foreach (Entity e in EntityAddQueue)
            {
                Entities.Add(e);
                if(e is Player)
                    PlayerIds.Add(e.Body.BodyId);
            }


            EntityRemoveQueue.Clear();
            EntityAddQueue.Clear();
        }
        #endregion

        public void Dispose()
        {
            this.Media.Dispose();
            Media = null;

            this.World.Clear();
            World = null;
        }
    }
}
