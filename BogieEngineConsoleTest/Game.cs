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
        public ModelNode _SamusRelativeCube;
        public ModelNode _MiniSamus;
        public ModelNode _Cube;

        int frame = 0;

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

            DefaultShader = new Shader("Resources/Shaders/default.vert", "Resources/Shaders/default.frag");
            MaskCubeShader = new Shader("Resources/Shaders/default.vert", "Resources/Shaders/maskCube.frag");

            //downloaded from https://sketchfab.com/3d-models/varia-suit-79c802129f9a4945aba62a607892ac31
            _Samus = new ModelNode(ContentManager.LoadModel("Resources/Models/VariaSuit/DolBarriersuit.obj", DefaultShader));
            _Samus.Transform.Scale(new Vector3(.1f, .1f, .1f));
            _Samus.Transform.Position = new Vector3(-.5f, -1, 0);

            _SamusNoVisor = new ModelNode(ContentManager.LoadModel("Resources/Models/VariaSuit/DolBarriersuit.obj", DefaultShader));
            _SamusNoVisor.Transform.Scale(new Vector3(.1f, .1f, .1f));
            _SamusNoVisor.Transform.Position = new Vector3(.5f, -1, 0);

            _MiniSamus = new ModelNode(ContentManager.LoadModel("Resources/Models/VariaSuit/DolBarriersuit.obj", DefaultShader));
            _MiniSamus.Transform.Scale(new Vector3(.1f, .1f, .1f));

            World.AddNode(_Samus);
            World.AddNode(_SamusNoVisor);

            Texture cube0Tex = ContentManager.LoadTexture("Resources/Textures/Brick.jpg", TextureUnit.Texture0);
            Texture cube1Tex = ContentManager.LoadTexture("Resources/Textures/Circle.png", TextureUnit.Texture1);

            _Cube = new ModelNode(ContentManager.LoadModel("Resources/Models/Cube.obj", MaskCubeShader));
            _Cube.Transform.Scale(new Vector3(3.5f, 3.5f, 1));
            _Cube.Transform.Position = new Vector3(0, 0, -2);
            _Cube.Model.MeshData[0].Textures.Add(cube0Tex);
            _Cube.Model.MeshData[0].Textures.Add(cube1Tex);

            _SamusRelativeCube = new ModelNode(ContentManager.LoadModel("Resources/Models/Cube.obj", DefaultShader));
            _SamusRelativeCube.Model.MeshData[0].Textures.Add(cube0Tex);
            _SamusRelativeCube.Transform.Scale(new Vector3(1, 1, 1));

            World.AddNode(_Cube);
            World.AddNode(_SamusRelativeCube);
            _Samus.AddNode(_SamusRelativeCube);
            _SamusRelativeCube.AddNode(_MiniSamus);

            World.AddNode(ActiveCamera);
            //_Samus.AddNode(ActiveCamera);

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
            World.Process((float)e.Time, new Transform());
        }

        protected override void OnRenderFrame(FrameEventArgs e)
        {
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            DefaultShader.Projection = ActiveCamera.Projection;
            DefaultShader.View = ActiveCamera.View;

            MaskCubeShader.Projection = ActiveCamera.Projection;
            MaskCubeShader.View = ActiveCamera.View;

            World.Draw((float)e.Time, new Transform());

            _Samus.Transform.Rotate(_SamusNoVisor.Transform.Up, -.01f);
            _SamusNoVisor.Transform.Rotate(_SamusNoVisor.Transform.Up, -.01f);
            _SamusRelativeCube.Transform.Rotate(_SamusRelativeCube.Transform.Right, -.01f);
            _MiniSamus.Transform.Position = new Vector3(0, (float)Math.Sin(frame/100f), 0);

            frame += 1;

            Context.SwapBuffers();
            base.OnRenderFrame(e);
            Console.WriteLine("Time (s):" + e.Time);
            Console.WriteLine("FPS: " + 1d/e.Time);
        }

        protected override void OnUnload(EventArgs e)
        {
            DefaultShader.Dispose();
        }
    }
}
