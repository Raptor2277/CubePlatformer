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
            int id = 0;
            float[] blockInfo = new float[4];
            foreach(XmlNode node in reader.DocumentElement)
            {
                foreach(XmlNode child in node.ChildNodes)
                {
                    id = int.Parse(child.Attributes[0].Value);
                    blockInfo[0] = float.Parse(child.Attributes[1].Value);
                    blockInfo[1] = float.Parse(child.Attributes[2].Value);
                    blockInfo[2] = float.Parse(child.Attributes[3].Value);
                    blockInfo[3] = float.Parse(child.Attributes[4].Value);
                    loadBlock(id, blockInfo, c);
                }
            }
            reader.Clone();
        }

        private static void loadBlock(int id, float[] info, ContentManager c)
        {
            switch (id)
            {
                case 0:
                    c.add(new Player(c, info[0], info[1], info[2], info[3]));
                    break;
                case 1:
                    c.add(new Tile(c, info[0], info[1], info[2], info[3]));
                    break;
                case 2:
                    c.add(new ExitTile(c, info[0], info[1], info[2], info[3]));
                    break;
                case 3:
                    c.add(new Tip(c, info[0], info[1], info[2], info[3]));
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

                Rectangle pos = e.PositionBox;

                at = writer.CreateAttribute("x");
                at.InnerText = pos.x.ToString();
                block.Attributes.Append(at);

                at = writer.CreateAttribute("y");
                at.InnerText = pos.y.ToString();
                block.Attributes.Append(at);

                at = writer.CreateAttribute("w");
                at.InnerText = pos.width.ToString();
                block.Attributes.Append(at);

                at = writer.CreateAttribute("h");
                at.InnerText = pos.height.ToString();
                block.Attributes.Append(at);

                blocksNode.AppendChild(block);
            }

            rootNode.AppendChild(blocksNode);
            writer.AppendChild(rootNode);

            writer.Save(path);
        }
    }
}
