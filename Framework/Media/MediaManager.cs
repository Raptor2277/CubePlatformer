using SFML.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Framework.Media
{
    class MediaManager : IDisposable
    {
        public Dictionary<string, Texture> Textures { get; private set; }
        public Dictionary<string, Font> Fonts { get; private set; }

        public MediaManager()
        {
            this.Textures = new Dictionary<string, Texture>();
            this.Fonts = new Dictionary<string, Font>();
        }

        public Texture loadTexture(string path, bool smooth)
        {
            Texture texture;
            if (Textures.TryGetValue(path, out texture))
                return texture;
            else
            {
                texture = new Texture(path);
                texture.Smooth = smooth;
                Textures.Add(path, texture);
                return texture;
            }
        }

        public Font loadFont(string path)
        {
            Font font;
            if (Fonts.TryGetValue(path, out font))
                return font;
            else
            {
                font = new Font(path);
                Fonts.Add(path, font);
                return font;
            }
        }

        public void Dispose()
        {
            foreach (var t in Textures)
                t.Value.Dispose();

            foreach (var t in Fonts)
                t.Value.Dispose();

            Textures.Clear();
            Textures = null;

            Fonts.Clear();
            Fonts = null;
        }
    }
}
