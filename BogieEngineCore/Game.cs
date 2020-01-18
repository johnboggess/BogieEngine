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

        internal VertexBuffer _VB;
        internal ElementBuffer _EB;
        internal Mesh _Mesh;
        internal Model _Model;
        internal Model _ModelMonkey;
        internal VertexArray _VA;
        internal Shader _Shader;
        internal Texture _Texture;
        internal Texture _TextureMask;

        public Game(int width, int height, string title, int updateRate = 30, int frameRate = 30) : base(width, height, OpenTK.Graphics.GraphicsMode.Default, title)
        {
            Run(updateRate, frameRate);
        }

        protected override void OnLoad(EventArgs e)
        {
            _Shader = new Shader("Resources/Shaders/default.vert", "Resources/Shaders/default.frag");
            _Texture = ContentManager.LoadTexture("Resources/Textures/Brick.jpg", TextureUnit.Texture0);
            _TextureMask = ContentManager.LoadTexture("Resources/Textures/Circle.png", TextureUnit.Texture1);

            _ModelMonkey = ModelLoader.LoadModel("Resources/Models/Monkey.obj");
            _ModelMonkey.Transform = Matrix4.Identity;
            foreach (Mesh mesh in _ModelMonkey.Meshes)
            {
                mesh.Shader = _Shader;
                mesh.Textures.Add(_Texture);
                mesh.Textures.Add(_TextureMask);
            }

            GL.ClearColor(1, 1, 1, 1);

            Vertex[] vertices =
            {
                new Vertex(new Vector3(0.5f, 0.5f, 0.0f), new Vector2(1, 1)),
                new Vertex(new Vector3(0.5f, -0.5f, 0.0f), new Vector2(1, 0)),
                new Vertex(new Vector3(-0.5f, -0.5f, 0.0f), new Vector2(0, 0)),
                new Vertex(new Vector3(-0.5f, 0.5f, 0.0f), new Vector2(0, 1))
            };

            uint[] indices =
            {
                0,1,3,1,2,3
            };


            _VB = new VertexBuffer();
            _VB.SetVertices(vertices);

            _EB = new ElementBuffer();
            _EB.SetIndices(indices);

            _VA = new VertexArray();
            _VA.Setup(_VB, _EB);

            _Mesh = new Mesh(_VA);
            _Mesh.Shader = _Shader;
            _Mesh.Textures = new List<Texture> { _Texture, _TextureMask };

            _Model = new Model(new List<Mesh> { _Mesh });
            _Model.Transform = Matrix4.CreateRotationX(MathHelper.DegreesToRadians(-55.0f));
            base.OnLoad(e);
        }

        protected override void OnResize(EventArgs e)
        {
            GL.Viewport(0, 0, Width, Height);
            base.OnResize(e);
        }

        protected override void OnRenderFrame(FrameEventArgs e)
        {
            GL.Clear(ClearBufferMask.ColorBufferBit);
            _Shader.Projection = ActiveCamera.Projection;
            _Shader.View = ActiveCamera.View;

            _Model.Draw();
            _ModelMonkey.Draw();

            Context.SwapBuffers();
            base.OnRenderFrame(e);
        }

        protected override void OnUnload(EventArgs e)
        {
            _VA.Dispose();
            _Shader.Dispose();
        }
    }
}

