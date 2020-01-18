using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL4;

using BogieEngineCore.Modelling;
using BogieEngineCore.Shading;
using BogieEngineCore.Texturing;
namespace BogieEngineCore
{
    public class Game : GameWindow
    {
        public ContentManager ContentManager = new ContentManager();
        public Color4 ClearColor = Color4.White;
        public Camera ActiveCamera = new Camera();
        public Random rng = new Random();

        internal Model _Samus;
        internal Shader _Shader;
        internal Texture _Texture;

        float rot = 0;

        public Game(int width, int height, string title, int updateRate = 30, int frameRate = 30) : base(width, height, OpenTK.Graphics.GraphicsMode.Default, title)
        {
            Run(updateRate, frameRate);
        }

        protected override void OnLoad(EventArgs e)
        {
            GL.Enable(EnableCap.DepthTest);
            GL.ClearColor(ClearColor);

            _Shader = new Shader("Resources/Shaders/default.vert", "Resources/Shaders/default.frag");
            _Texture = ContentManager.LoadTexture("Resources/Textures/Brick.jpg", TextureUnit.Texture0);

            //downloaded from https://sketchfab.com/3d-models/varia-suit-79c802129f9a4945aba62a607892ac31
            _Samus = ModelLoader.LoadModel("Resources/Models/VariaSuit/DolBarriersuit.obj", this);
            foreach (Mesh mesh in _Samus.Meshes)
            {
                mesh.Shader = _Shader;
            }

            base.OnLoad(e);
        }

        protected override void OnResize(EventArgs e)
        {
            GL.Viewport(0, 0, Width, Height);
            base.OnResize(e);
        }

        protected override void OnRenderFrame(FrameEventArgs e)
        {
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            _Shader.Projection = ActiveCamera.Projection;
            _Shader.View = ActiveCamera.View;

            rot += .01f;
            _Samus.Transform = Matrix4.CreateScale(.1f)*Matrix4.CreateRotationY(rot) * Matrix4.CreateTranslation(0, -1, 0); ;

            //_Model.Draw();
            //_ModelMonkey.Draw();
            _Samus.Draw();

            Context.SwapBuffers();
            base.OnRenderFrame(e);
        }

        protected override void OnUnload(EventArgs e)
        {
            _Shader.Dispose();
        }
    }
}

