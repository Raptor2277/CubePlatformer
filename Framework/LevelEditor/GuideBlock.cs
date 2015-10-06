using SFML.Graphics;
using SFML.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Framework.Interface;
using Framework.Abstract;
using Framework.Managers;
using Framework.Blocks;
using Framework.Utilities;

namespace Framework.LevelEditor
{
    class GuideBlock : IHandleMouseButton
    {
        private ContentManager contentManager;

        private Rectangle blockOutline;
        private Vector2i blockStart;
        public bool IsActive { get; set; }

        public enum States
        {
            placingScaleableBlock,
            scalingBlock,
            placingBlock,
        }
        public States state;
        public SelectorButton.Types type;

        public GuideBlock(ContentManager content)
        {
            this.state = States.placingScaleableBlock;
            this.type = SelectorButton.Types.Tile;
            this.contentManager = content;
        }

        public void update(Vector2i mPos)
        {
            if(IsActive)
            {
                Vector2i gridPos = new Vector2i(mPos.X - (mPos.X % 32) + 16, mPos.Y - (mPos.Y % 32) + 16);
            
                int lowX,  lowY,  highX,  highY;

                if(gridPos.Y < blockStart.Y)
                {
                    highY = blockStart.Y + 16;
                    lowY = gridPos.Y - 16;
                }
                else
                {
                    lowY = blockStart.Y - 16;
                    highY = gridPos.Y + 16;
                }

                if (gridPos.X < blockStart.X)
                {
                    highX = blockStart.X + 16;
                    lowX = gridPos.X - 16;
                }
                else
                {
                    lowX = blockStart.X - 16;
                    highX = gridPos.X + 16;
                }

                this.blockOutline = new Rectangle(lowX, lowY, highX - lowX, highY - lowY);
            }
        }

        public void draw(RenderWindow window)
        {
            if(IsActive)
            {
                if(state == States.scalingBlock)
                    Draw.drawRectangle(window,blockOutline, Color.Yellow);
                else if (state == States.placingBlock)
                    Draw.drawRectangle(window,blockOutline, Color.Yellow);
            }
	    }
        
        public void newOutline(Vector2i startPos)
        {
            this.blockOutline = new Rectangle(startPos.X, startPos.Y, 32, 32);  
            this.blockStart = new Vector2i(startPos.X + 16, startPos.Y + 16);
        }

        public void handleMouseButton(SFML.Window.MouseButtonEventArgs e)
        {
            if(IsActive)
            {
                int x = e.X - (e.X % 32);
                int y = e.Y - (e.Y % 32);

                if (e.Button == SFML.Window.Mouse.Button.Left)
                {
                    if (state == States.placingScaleableBlock)
                    {
                        this.newOutline(new Vector2i(x, y));
                        this.state = States.scalingBlock;
                    }
                    else if (state == States.scalingBlock)
                    {
                        placeBlock();
                        this.state = States.placingScaleableBlock;
                    }
                    
                }
                else if (e.Button == SFML.Window.Mouse.Button.Right)
                {
                    if(state == States.scalingBlock)
                    {
                        state = States.placingScaleableBlock;
                    }
                    else
                    {
                        Entity remove = null;
                        foreach (Entity entity in contentManager.Entities)
                        {
                            //if (Utils.checkMouseCollision(e.X, e.Y, e.InitialPosition))
                                remove = entity;
                        }

                        if (remove != null)
                            contentManager.remove(remove);
                    }
                }
            }
        }

        private void placeBlock()
        {
            switch(type)
            {
                case SelectorButton.Types.Tile:
                    contentManager.add(new Tile(contentManager, this.blockOutline.x, this.blockOutline.y, this.blockOutline.width, this.blockOutline.height));
                    break;
                case SelectorButton.Types.Player:
                    contentManager.add(new Player(contentManager, this.blockOutline.x, this.blockOutline.y, this.blockOutline.width, this.blockOutline.height));
                    break;
                case SelectorButton.Types.ExitTile:
                    contentManager.add(new ExitTile(contentManager, this.blockOutline.x, this.blockOutline.y, this.blockOutline.width, this.blockOutline.height));
                    break;
                case SelectorButton.Types.Tip:
                    contentManager.add(new Tip(contentManager, this.blockOutline.x, this.blockOutline.y, this.blockOutline.width, this.blockOutline.height));
                    break;
            }
        }
    }
}
