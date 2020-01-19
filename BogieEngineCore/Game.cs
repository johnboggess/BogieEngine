using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using OpenTK.Input;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL4;

using BogieEngineCore.Nodes;
using BogieEngineCore.Modelling;
using BogieEngineCore.Shading;
using BogieEngineCore.Texturing;
namespace BogieEngineCore
{
    public class Game : GameWindow
    {
        public ContentManager ContentManager;
        public Color4 ClearColor = Color4.CornflowerBlue;
        public Shader DefaultShader;
        public Shader MaskCubeShader;
        public Camera ActiveCamera = new Camera();
        public Node World = new Node();

        internal ModelNode _Samus;
        internal ModelNode _SamusNoVisor;
        internal ModelNode _Cube;

        public Game(int width, int height, string title, int updateRate = 30, int frameRate = 30) : base(width, height, OpenTK.Graphics.GraphicsMode.Default, title)
        {
            ContentManager = new ContentManager(this);
            Run(updateRate, frameRate);
        }

        protected override void OnLoad(EventArgs e)
        {
            GL.Enable(EnableCap.DepthTest);
            GL.ClearColor(ClearColor);

            ActiveCamera.Transform.Position = new Vector3(0, 0, 3);
            ActiveCamera.Transform.Scale(new Vector3(1, 1, 1));

            DefaultShader = new Shader("Resources/Shaders/default.vert", "Resources/Shaders/default.frag");
            MaskCubeShader = new Shader("Resources/Shaders/default.vert", "Resources/Shaders/maskCube.frag");

            //downloaded from https://sketchfab.com/3d-models/varia-suit-79c802129f9a4945aba62a607892ac31
            _Samus = new ModelNode(ContentManager.LoadModel("Resources/Models/VariaSuit/DolBarriersuit.obj"));
            _Samus.Transform.Scale(new Vector3(.1f, .1f, .1f));
            _Samus.Transform.Position = new Vector3(-.5f, -1, 0);

            _SamusNoVisor = new ModelNode(ContentManager.LoadModel("Resources/Models/VariaSuit/DolBarriersuit.obj"));
            _SamusNoVisor.Transform.Scale(new Vector3(.1f, .1f, .1f));
            _SamusNoVisor.Transform.Position = new Vector3(.5f, -1, 0);

            World.AddNode(_Samus);
            World.AddNode(_SamusNoVisor);

            Texture cube0Tex = ContentManager.LoadTexture("Resources/Textures/Brick.jpg", TextureUnit.Texture0);
            Texture cube1Tex = ContentManager.LoadTexture("Resources/Textures/Circle.png", TextureUnit.Texture1);

            _Cube = new ModelNode(ContentManager.LoadModel("Resources/Models/Cube.obj"));
            _Cube.Transform.Scale(new Vector3(3.5f, 3.5f, 1));
            _Cube.Transform.Position = new Vector3(0, 0, -2);
            _Cube.Model.MeshData[0].Shader = MaskCubeShader;
            _Cube.Model.MeshData[0].Textures.Add(cube0Tex);
            _Cube.Model.MeshData[0].Textures.Add(cube1Tex);

            World.AddNode(_Cube);

            List<MeshData> meshData = _SamusNoVisor.GetMeshWithName("polygon6");
            if(meshData.Count > 0) { meshData[0].Visible = false; }
            base.OnLoad(e);
        }

        protected override void OnResize(EventArgs e)
        {
            GL.Viewport(0, 0, Width, Height);
            base.OnResize(e);
        }


        protected override void OnUpdateFrame(FrameEventArgs e)
        {
            KeyboardState ks = Keyboard.GetState();
            if (ks.IsKeyDown(Key.A))
            {
                ActiveCamera.Transform.Rotate(ActiveCamera.Transform.YAxis, -.1f);
            }
            if (ks.IsKeyDown(Key.D))
            {
                ActiveCamera.Transform.Rotate(ActiveCamera.Transform.YAxis, .1f);
            }
            if (ks.IsKeyDown(Key.W))
            {
                ActiveCamera.Transform.Rotate(ActiveCamera.Transform.Right, .1f);
            }
            if (ks.IsKeyDown(Key.S))
            {
                ActiveCamera.Transform.Rotate(ActiveCamera.Transform.Right, -.1f);
            }
            if (ks.IsKeyDown(Key.Q))
            {
                ActiveCamera.Transform.Rotate(ActiveCamera.Transform.Forwards, .1f);
            }
            if (ks.IsKeyDown(Key.E))
            {
                ActiveCamera.Transform.Rotate(ActiveCamera.Transform.Forwards, -.1f);
            }


        }

        protected override void OnRenderFrame(FrameEventArgs e)
        {
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            DefaultShader.Projection = ActiveCamera.Projection;
            DefaultShader.View = ActiveCamera.View;

            MaskCubeShader.Projection = ActiveCamera.Projection;
            MaskCubeShader.View = ActiveCamera.View;

            World._Draw();

            _Samus.Transform.Rotate(_SamusNoVisor.Transform.Up, -.01f);
            _SamusNoVisor.Transform.Rotate(_SamusNoVisor.Transform.Up, -.01f);

            Context.SwapBuffers();
            base.OnRenderFrame(e);
        }

        protected override void OnUnload(EventArgs  e)
        {
            DefaultShader.Dispose();
        }
    }
}

