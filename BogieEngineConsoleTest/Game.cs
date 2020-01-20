using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using OpenTK;
using OpenTK.Input;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL4;

using BogieEngineCore;
using BogieEngineCore.Shading;
using BogieEngineCore.Nodes;
using BogieEngineCore.Modelling;
using BogieEngineCore.Texturing;
namespace BogieEngineConsoleTest
{
    class Game : GameWindow
    {
        public ContentManager ContentManager;
        public Color4 ClearColor = Color4.CornflowerBlue;
        public Shader DefaultShader;
        public Shader MaskCubeShader;

        public Camera ActiveCamera = new FPSCamera();
        public Root World = new Root();

        public ModelNode _Samus;
        public ModelNode _SamusNoVisor;
        public ModelNode _Cube;


        public Game(int width, int height, string title, int updateRate = 60, int frameRate = 60) : base(width, height, OpenTK.Graphics.GraphicsMode.Default, title)
        {
            ContentManager = new ContentManager();
            Run(updateRate, frameRate);
        }

        protected override void OnLoad(EventArgs e)
        {
            GL.Enable(EnableCap.DepthTest);
            GL.Enable(EnableCap.CullFace);
            GL.ClearColor(ClearColor);

            ActiveCamera.Transform.Position = new Vector3(0, 0, 3);
            ActiveCamera.Transform.Scale(new Vector3(1, 1, 1));

            DefaultShader = new Shader("Resources/Shaders/default.vert", "Resources/Shaders/default.frag");
            MaskCubeShader = new Shader("Resources/Shaders/default.vert", "Resources/Shaders/maskCube.frag");

            //downloaded from https://sketchfab.com/3d-models/varia-suit-79c802129f9a4945aba62a607892ac31
            _Samus = new ModelNode(ContentManager.LoadModel("Resources/Models/VariaSuit/DolBarriersuit.obj", DefaultShader));
            _Samus.Transform.Scale(new Vector3(.1f, .1f, .1f));
            _Samus.Transform.Position = new Vector3(-.5f, -1, 0);

            _SamusNoVisor = new ModelNode(ContentManager.LoadModel("Resources/Models/VariaSuit/DolBarriersuit.obj", DefaultShader));
            _SamusNoVisor.Transform.Scale(new Vector3(.1f, .1f, .1f));
            _SamusNoVisor.Transform.Position = new Vector3(.5f, -1, 0);

            World.AddNode(_Samus);
            World.AddNode(_SamusNoVisor);

            Texture cube0Tex = ContentManager.LoadTexture("Resources/Textures/Brick.jpg", TextureUnit.Texture0);
            Texture cube1Tex = ContentManager.LoadTexture("Resources/Textures/Circle.png", TextureUnit.Texture1);

            _Cube = new ModelNode(ContentManager.LoadModel("Resources/Models/Cube.obj", MaskCubeShader));
            _Cube.Transform.Scale(new Vector3(3.5f, 3.5f, 1));
            _Cube.Transform.Position = new Vector3(0, 0, -2);
            _Cube.Model.MeshData[0].Textures.Add(cube0Tex);
            _Cube.Model.MeshData[0].Textures.Add(cube1Tex);

            World.AddNode(_Cube);
            World.AddNode(ActiveCamera);

            List<MeshData> meshData = _SamusNoVisor.GetMeshWithName("polygon6");
            if (meshData.Count > 0) { meshData[0].Visible = false; }
            base.OnLoad(e);
        }

        protected override void OnResize(EventArgs e)
        {
            GL.Viewport(0, 0, Width, Height);
            base.OnResize(e);
        }

        protected override void OnUpdateFrame(FrameEventArgs e)
        {
            World.Process((float)e.Time);
        }

        protected override void OnRenderFrame(FrameEventArgs e)
        {
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            DefaultShader.Projection = ActiveCamera.Projection;
            DefaultShader.View = ActiveCamera.View;

            MaskCubeShader.Projection = ActiveCamera.Projection;
            MaskCubeShader.View = ActiveCamera.View;

            World.Draw((float)e.Time);

            _Samus.Transform.Rotate(_SamusNoVisor.Transform.Up, -.01f);
            _SamusNoVisor.Transform.Rotate(_SamusNoVisor.Transform.Up, -.01f);

            Context.SwapBuffers();
            base.OnRenderFrame(e);
            Console.WriteLine("Time (s):" + e.Time);
            Console.WriteLine("FPS (s):" + 1d/e.Time);
        }

        protected override void OnUnload(EventArgs e)
        {
            DefaultShader.Dispose();
        }
    }
}
