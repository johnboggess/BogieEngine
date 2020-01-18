using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL4;

using BogieEngineCore.Shaders;
using BogieEngineCore.Textures;
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

            _Shader = new Shader("Shaders/default.vert", "Shaders/default.frag");
            _Texture = ContentManager.LoadTexture("Textures/Brick.jpg", TextureUnit.Texture0);
            _TextureMask = ContentManager.LoadTexture("Textures/Circle.png", TextureUnit.Texture1);

            _VB = new VertexBuffer();
            _VB.SetVertices(vertices);

            _EB = new ElementBuffer();
            _EB.SetIndices(indices);

            _VA = new VertexArray();
            _VA.Setup(_VB, _EB, new List<Texture> { _Texture, _TextureMask });

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

            _VA.Bind();
            _VA.BindTextures();
            _Shader.Use(ActiveCamera.Projection, ActiveCamera.View, Matrix4.CreateRotationX(MathHelper.DegreesToRadians(-55.0f)));
            _VA.Draw();
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

