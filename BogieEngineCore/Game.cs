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
        public Color4 ClearColor = Color4.CornflowerBlue;
        public Shader DefaultShader;
        public Shader MaskCubeShader;
        public Camera ActiveCamera = new Camera();

        internal Model _Samus;
        internal Model _SamusNoVisor;
        internal Model _Cube;

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
            MaskCubeShader = new Shader("Resources/Shaders/default.vert", "Resources/Shaders/maskCube.frag");

            //downloaded from https://sketchfab.com/3d-models/varia-suit-79c802129f9a4945aba62a607892ac31
            _Samus = ContentManager.LoadModel("Resources/Models/VariaSuit/DolBarriersuit.obj");
            _SamusNoVisor = ContentManager.LoadModel("Resources/Models/VariaSuit/DolBarriersuit.obj");
            _SamusNoVisor.Transform = Matrix4.CreateScale(.1f) * Matrix4.CreateTranslation(0, -1, 0);

            Texture cube0Tex = ContentManager.LoadTexture("Resources/Textures/Brick.jpg", TextureUnit.Texture0);
            Texture cube1Tex = ContentManager.LoadTexture("Resources/Textures/Circle.png", TextureUnit.Texture1);

            _Cube = ContentManager.LoadModel("Resources/Models/Cube.obj");
            _Cube.Transform = Matrix4.CreateTranslation(0, 0, -2) * Matrix4.CreateScale(3.5f, 3.5f, 1);
            _Cube.MeshData[0].Shader = MaskCubeShader;
            _Cube.MeshData[0].Textures.Add(cube0Tex);
            _Cube.MeshData[0].Textures.Add(cube1Tex);

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

            MaskCubeShader.Projection = ActiveCamera.Projection;
            MaskCubeShader.View = ActiveCamera.View;

            rot += .01f;
            _Samus.Transform = Matrix4.CreateScale(.1f)*Matrix4.CreateRotationY(rot) * Matrix4.CreateTranslation(-.5f, -1, 0);
            _Samus.Draw();

            _SamusNoVisor.Transform = Matrix4.CreateScale(.1f) * Matrix4.CreateRotationY(rot) * Matrix4.CreateTranslation(.5f, -1, 0);
            _SamusNoVisor.Draw();

            _Cube.Draw();

            Context.SwapBuffers();
            base.OnRenderFrame(e);
        }

        protected override void OnUnload(EventArgs  e)
        {
            DefaultShader.Dispose();
        }
    }
}

