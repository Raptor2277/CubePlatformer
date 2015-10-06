using Cube_Platformer;
using Framework.Interface;
using Framework.Managers;
using Framework.Utilities;
using SFML.Graphics;
using SFML.System;
using SFML.Window;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Framework.Light
{
    class LightLayer : IDisposable
    {
        public List<Light> Lights { get; set; }
        public List<Polygon> Polygons { get; private set; }

        private RenderTexture renText;
        private RenderTexture lightMap;
        private Texture scene;
        private Shader lightShader;
        private Shader shadowShader;
        private RenderStates lightState;
        private RenderStates shadowState;

        private Color ambientLight = Color.Black;

        public LightLayer(Vector2u gameRez, Vector2u windowRez)
        {
            Lights = new List<Light>();
            Polygons = new List<Polygon>();

            renText = new RenderTexture(gameRez.X, gameRez.Y);
            lightMap = new RenderTexture(gameRez.X, gameRez.Y);
            lightMap.Smooth = true;
            scene = new Texture(windowRez.X, windowRez.Y);

            lightShader = new Shader(null, "Content/shaders/light.frag");
            lightShader.SetParameter("screenHeight", gameRez.Y);
            lightState = new RenderStates(BlendMode.Add, Transform.Identity, renText.Texture, lightShader);

            shadowShader = new Shader(null, "Content/shaders/shadows.frag");
            //shadowState = new RenderStates(shadowShader);
            shadowState = new RenderStates(BlendMode.Multiply);
        }

        public void update(ContentManager content)
        {
            this.Polygons = content.getLightPolygons();
        }

        public void draw(RenderWindow window)
        {
            lightMap.Clear(ambientLight);
            foreach (Light l in Lights)
            {
                if(l is SpotLight)
                {
                    SpotLight light = l as SpotLight;
                    renText.Clear(Color.Black);
                    Vertex[] verts = VectorMath.toVertecies(new Vector2f[] { light.pos, light.getBoundTwo(), light.getBoundOne() }, l.color);
                    renText.Draw(verts, PrimitiveType.Triangles);
                }
                else
                    renText.Clear(l.color); ;

                foreach (Polygon p in Polygons)
                {
                    List<Line> sides = p.getLines();
                    foreach (Line line in sides)
                    {
                        Vector2f normal = new Vector2f(line.y2 - line.y1, (line.x2 - line.x1) * -1.0f);
                        Vector2f lightDir = new Vector2f(line.x1 - l.pos.X, line.y1 - l.pos.Y);

                        if (VectorMath.dotProduct(normal, lightDir) > 0)
                        {
                            Vector2f v1 = new Vector2f(line.x1, line.y1);
                            Vector2f v2 = new Vector2f(line.x2, line.y2);
                            Vector2f p1 = VectorMath.multiply(lightDir, 10000.0f);
                            Vector2f p2 = VectorMath.multiply(new Vector2f(line.x2 - l.pos.X, line.y2 - l.pos.Y), 10000.0f);

                            Vertex[] verts = VectorMath.toVertecies(new Vector2f[] { v1, p1, p2, v2 }, Color.Black);
                            renText.Draw(verts, PrimitiveType.Quads);
                        }
                    }
                }
                renText.Display();
                lightShader.SetParameter("lightPos", l.pos);
                lightShader.SetParameter("brightness", l.brightness);
                lightShader.SetParameter("lightColor", l.color.R / 255.0f, l.color.G / 255.0f, l.color.B / 255.0f);
                lightMap.Draw(new Sprite(renText.Texture), lightState);
            }

            lightMap.Display();
            scene.Update(window);
            shadowShader.SetParameter("lightTexture", lightMap.Texture);
            shadowShader.SetParameter("scene", scene);
            window.Draw(new Sprite(lightMap.Texture), shadowState); //, new RenderStates(BlendMode.Add)
            foreach (Light l in Lights)
                l.draw(window);
        }

        public void add(Light l)
        {
            Lights.Add(l);
        }

        public void remove(Light l)
        {
            Lights.Remove(l);
        }

        public void setPolygons(List<Polygon> polygons)
        {
            this.Polygons = polygons;
        }

        public void setAmbientLight(Color color)
        {
            this.ambientLight = color;
        }

        public void Dispose()
        {
            this.lightMap.Dispose();
            this.lightMap = null;

            this.Lights.Clear();
            this.Lights = null;

            this.lightShader.Dispose();
            this.lightShader = null;

            this.Polygons.Clear();
            this.Polygons = null;

            this.renText.Dispose();
            this.renText = null;

            this.scene.Dispose();
            this.scene = null;

            this.shadowShader.Dispose();
            this.shadowShader = null;
        }
    }
}
