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
    class Game : BaseGame
    {
        public Shader DefaultShader;
        public Shader MaskCubeShader;

        public ModelNode _Samus;
        public ModelNode _SamusNoVisor;
        public ModelNode _SamusRelativeCube;
        public ModelNode _MiniSamus;
        public Block Floor;
        public Block _Cube;


        int frame = 0;

        public Game(int width, int height, string title, int updateRate = 60, int frameRate = 60) : base(width, height, title, updateRate, frameRate)
        {
        }

        protected override void Loading(EventArgs e)
        {
            ActiveCamera = new FPSCamera(this);
            ActiveCamera.LocalTransform.Position = new Vector3(0, 0, 3);


            DefaultShader = new Shader("Resources/Shaders/default.vert", "Resources/Shaders/default.frag");
            MaskCubeShader = new Shader("Resources/Shaders/default.vert", "Resources/Shaders/maskCube.frag");

            Texture cube0Tex = ContentManager.LoadTexture("Resources/Textures/Brick.jpg", TextureUnit.Texture0);
            Texture cube1Tex = ContentManager.LoadTexture("Resources/Textures/Circle.png", TextureUnit.Texture1);
            Block.Model = ContentManager.LoadModel("Resources/Models/Cube.obj", MaskCubeShader);
            Block.Model.MeshData[0].Textures.Add(cube0Tex);
            Block.Model.MeshData[0].Textures.Add(cube1Tex);


            //downloaded from https://sketchfab.com/3d-models/varia-suit-79c802129f9a4945aba62a607892ac31
            _Samus = new ModelNode(this, ContentManager.LoadModel("Resources/Models/VariaSuit/DolBarriersuit.obj", DefaultShader));
            _Samus.LocalTransform.ScaleBy(new Vector3(.1f, .1f, .1f));
            _Samus.LocalTransform.Position = new Vector3(-.5f, -1, 0);

            _SamusNoVisor = new ModelNode(this, ContentManager.LoadModel("Resources/Models/VariaSuit/DolBarriersuit.obj", DefaultShader));
            _SamusNoVisor.LocalTransform.ScaleBy(new Vector3(.1f, .1f, .1f));
            _SamusNoVisor.LocalTransform.Position = new Vector3(.5f, -1, 0);

            _MiniSamus = new ModelNode(this, ContentManager.LoadModel("Resources/Models/VariaSuit/DolBarriersuit.obj", DefaultShader));
            _MiniSamus.LocalTransform.ScaleBy(new Vector3(.1f, .1f, .1f));


            /*Floor = new Block(this);
            Floor.LocalTransform.Position = new Vector3(0, -10, 0);
            Floor.LocalTransform.Scale = new Vector3(10, 1, 10);*/

            _Cube = new Block(this);
            _Cube.LocalTransform.ScaleBy(new Vector3(3.5f, 3.5f, 1));
            _Cube.LocalTransform.Position = new Vector3(0, 0, -2);
            _Cube.Init(new Transform());

            _SamusRelativeCube = new ModelNode(this, ContentManager.LoadModel("Resources/Models/Cube.obj", DefaultShader));
            _SamusRelativeCube.Model.MeshData[0].Textures.Add(cube0Tex);
            _SamusRelativeCube.LocalTransform.ScaleBy(new Vector3(1, 1, 1));

            //World.AddNode(Floor);
            World.AddNode(_Cube);

            World.AddNode(_Samus);
            World.AddNode(_SamusNoVisor);
            World.AddNode(_SamusRelativeCube);
            _Samus.AddNode(_SamusRelativeCube);
            _SamusRelativeCube.AddNode(_MiniSamus);

            World.AddNode(ActiveCamera);

            List<MeshData> meshData = _SamusNoVisor.GetMeshWithName("polygon6");
            if (meshData.Count > 0) { meshData[0].Visible = false; }
        }

        protected override void OnResize(EventArgs e)
        {
            GL.Viewport(0, 0, Width, Height);
            base.OnResize(e);
        }

        protected override void PreRenderFrame(FrameEventArgs e)
        {
            DefaultShader.Projection = ActiveCamera.Projection;
            DefaultShader.View = ActiveCamera.View;

            MaskCubeShader.Projection = ActiveCamera.Projection;
            MaskCubeShader.View = ActiveCamera.View;

            _Samus.LocalTransform.Rotate(_SamusNoVisor.LocalTransform.Up, -.01f);
            _SamusNoVisor.LocalTransform.Rotate(_SamusNoVisor.LocalTransform.Up, -.01f);
            _SamusRelativeCube.LocalTransform.Rotate(_SamusRelativeCube.LocalTransform.Right, -.01f);
            _MiniSamus.LocalTransform.Position = new Vector3(0, (float)Math.Sin(frame / 100f), 0);

            frame += 1;
        }

        protected override void OnUnload(EventArgs e)
        {
            DefaultShader.Dispose();
        }
    }
}
