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
        public ContentManager ContentManager;
        public Color4 ClearColor = Color4.White;
        public Shader DefaultShader;
        public Camera ActiveCamera = new Camera();

        internal Model _Samus;
        internal Model _SamusNoVisor;

        float rot = 0;

        public Game(int width, int height, string title, int updateRate = 30, int frameRate = 30) : base(width, height, OpenTK.Graphics.GraphicsMode.Default, title)
        {
            ContentManager = new ContentManager(this);
            Run(updateRate, frameRate);
        }

        protected override void OnLoad(EventArgs e)
        {
            GL.Enable(EnableCap.DepthTest);
            GL.ClearColor(ClearColor);
            DefaultShader = new Shader("Resources/Shaders/default.vert", "Resources/Shaders/default.frag");

            //downloaded from https://sketchfab.com/3d-models/varia-suit-79c802129f9a4945aba62a607892ac31
            _Samus = ContentManager.LoadModel("Resources/Models/VariaSuit/DolBarriersuit.obj");
            _SamusNoVisor = ContentManager.LoadModel("Resources/Models/VariaSuit/DolBarriersuit.obj");
            _SamusNoVisor.Transform = Matrix4.CreateScale(.1f) * Matrix4.CreateTranslation(0, -1, 0);

            List<MeshData> meshData = _SamusNoVisor.GetMeshWithName("polygon6");
            if(meshData.Count > 0) { meshData[0].Visible = false; }
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
            DefaultShader.Projection = ActiveCamera.Projection;
            DefaultShader.View = ActiveCamera.View;

            rot += .01f;
            _Samus.Transform = Matrix4.CreateScale(.1f)*Matrix4.CreateRotationY(rot) * Matrix4.CreateTranslation(-.5f, -1, 0);
            _Samus.Draw();

            _SamusNoVisor.Transform = Matrix4.CreateScale(.1f) * Matrix4.CreateRotationY(rot) * Matrix4.CreateTranslation(.5f, -1, 0);
            _SamusNoVisor.Draw();

            Context.SwapBuffers();
            base.OnRenderFrame(e);
        }

        protected override void OnUnload(EventArgs  e)
        {
            DefaultShader.Dispose();
        }
    }
}

