using Framework.Abstract;
using Framework.Blocks;
using Framework.Managers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Framework.Utilities
{
    class IO
    {
        public static void loadLevel(ContentManager c, String path)
        {
            c.clear();
            XmlDocument reader = new XmlDocument();
            reader.Load(path);
            int[] blockInfo = new int[5];
            foreach(XmlNode node in reader.DocumentElement)
            {
                foreach(XmlNode child in node.ChildNodes)
                {
                    blockInfo[0] = int.Parse(child.Attributes[0].Value);
                    blockInfo[1] = int.Parse(child.Attributes[1].Value);
                    blockInfo[2] = int.Parse(child.Attributes[2].Value);
                    blockInfo[3] = int.Parse(child.Attributes[3].Value);
                    blockInfo[4] = int.Parse(child.Attributes[4].Value);
                    loadBlock(blockInfo, c);
                }
            }
            reader.Clone();
        }

        private static void loadBlock(int[] info, ContentManager c)
        {
            switch (info[0])
            {
                case 0:
                    c.add(new Player(c, info[1], info[2], info[3], info[4]));
                    break;
                case 1:
                    c.add(new Tile(c, info[1], info[2], info[3], info[4]));
                    break;
                case 2:
                    c.add(new ExitTile(c,info[1], info[2], info[3], info[4]));
                    break;
            }
        }

        public static void saveLevel(String path, ContentManager c)
        {
            XmlDocument writer = new XmlDocument();
            XmlNode rootNode = writer.CreateElement("Level");
            XmlNode blocksNode = writer.CreateElement("Blocks");

            foreach(Entity e in c.Entities)
            {
                XmlNode block = writer.CreateElement("Entity");
                
                XmlAttribute at = writer.CreateAttribute("id");
                at.InnerText = e.Id.ToString();
                block.Attributes.Append(at);

                //at = writer.CreateAttribute("x");
                //at.InnerText = e.Body.Position.X;
                //block.Attributes.Append(at);

                //at = writer.CreateAttribute("y");
                //at.InnerText = e.PositionBox.y.ToString();
                //block.Attributes.Append(at);

                //at = writer.CreateAttribute("w");
                //at.InnerText = e.PositionBox.width.ToString();
                //block.Attributes.Append(at);

                //at = writer.CreateAttribute("h");
                //at.InnerText = e.PositionBox.height.ToString();
                //block.Attributes.Append(at);

                blocksNode.AppendChild(block);
            }

            rootNode.AppendChild(blocksNode);
            writer.AppendChild(rootNode);

            writer.Save(path);
        }
    }
}
