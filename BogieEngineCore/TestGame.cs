using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using OpenTK;
using OpenTK.Input;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL4;

using Jitter;
using Jitter.Collision;
using Jitter.Collision.Shapes;

using BogieEngineCore.Nodes;
using BogieEngineCore.Modelling;
using BogieEngineCore.Shading;
using BogieEngineCore.Texturing;
namespace BogieEngineCore
{
    /// <summary>
    /// Just used for some testing.
    /// </summary>
    public class TestGame : BaseGame
    {
        private Shader DefaultShader;
        public Shader MaskCubeShader;

        internal ModelNode _Samus;
        internal ModelNode _SamusNoVisor;
        internal ModelNode _Cube;
        internal TestPhysicsObject _TestRigid;
        internal TestPhysicsObject _TestRigidB;

        public TestGame(int width, int height, string title, int updateRate = 30, int frameRate = 30) : base(width, height, title, updateRate, frameRate)
        {
        }

        protected override void Loading(EventArgs e)
        {
            Texture cube0Tex = ContentManager.LoadTexture("Resources/Textures/Brick.jpg", TextureUnit.Texture0);
            Texture cube1Tex = ContentManager.LoadTexture("Resources/Textures/Circle.png", TextureUnit.Texture1);

            ActiveCamera.LocalTransform.Position = new Vector3(0, 0, 3);
            ActiveCamera.LocalTransform.ScaleBy(new Vector3(1, 1, 1));

            DefaultShader = new Shader("Resources/Shaders/default.vert", "Resources/Shaders/default.frag");
            MaskCubeShader = new Shader("Resources/Shaders/default.vert", "Resources/Shaders/maskCube.frag");

            //downloaded from https://sketchfab.com/3d-models/varia-suit-79c802129f9a4945aba62a607892ac31
            _Samus = new ModelNode(ContentManager.LoadModel("Resources/Models/VariaSuit/DolBarriersuit.obj", DefaultShader));
            _Samus.LocalTransform.ScaleBy(new Vector3(.1f, .1f, .1f));
            _Samus.LocalTransform.Position = new Vector3(-.5f, -1, 0);

            _SamusNoVisor = new ModelNode(ContentManager.LoadModel("Resources/Models/VariaSuit/DolBarriersuit.obj", DefaultShader));
            _SamusNoVisor.LocalTransform.ScaleBy(new Vector3(.1f, .1f, .1f));
            _SamusNoVisor.LocalTransform.Position = new Vector3(.5f, -1, 0);

            _TestRigid = new TestPhysicsObject(ContentManager.LoadModel("Resources/Models/Cube.obj", MaskCubeShader));
            _TestRigid.LocalTransform.Position = new Vector3(0, 1, 0);
            _TestRigid.LocalTransform.Scale = new Vector3(.5f, .5f, .5f);
            _TestRigid.JitterRigidBody.Shape = new BoxShape(.5f, .5f, .5f);
            _TestRigid.LocalTransform.Rotate(Vector3.UnitZ, .71f);
            _TestRigid.LocalTransform.Rotate(Vector3.UnitX, .71f);
            _TestRigid.RigidBodyMatchLocalTransform(new Transform());
            _TestRigid.Model.MeshData[0].Textures.Add(cube0Tex);
            _TestRigid.Model.MeshData[0].Textures.Add(cube1Tex);
            JitterWorld.AddBody(_TestRigid.JitterRigidBody);

            _TestRigidB = new TestPhysicsObject(ContentManager.LoadModel("Resources/Models/Cube.obj", MaskCubeShader));
            _TestRigidB.LocalTransform.Position = new Vector3(0, -1, 0);
            _TestRigidB.LocalTransform.Scale = new Vector3(1, 1, 1);
            _TestRigidB.RigidBodyMatchLocalTransform(new Transform());
            _TestRigid.Model.MeshData[0].Textures.Add(cube0Tex);
            _TestRigid.Model.MeshData[0].Textures.Add(cube1Tex);
            JitterWorld.AddBody(_TestRigidB.JitterRigidBody);
            _TestRigidB.JitterRigidBody.IsStatic = true;


            _Cube = new ModelNode(ContentManager.LoadModel("Resources/Models/Cube.obj", MaskCubeShader));
            _Cube.LocalTransform.ScaleBy(new Vector3(3.5f, 3.5f, 1));
            _Cube.LocalTransform.Position = new Vector3(0, 0, -2);
            _Cube.Model.MeshData[0].Textures.Add(cube0Tex);
            _Cube.Model.MeshData[0].Textures.Add(cube1Tex);


            World.AddNode(_TestRigid);
            World.AddNode(_TestRigidB);
            //World.AddNode(_Samus);
            //World.AddNode(_SamusNoVisor);
            //World.AddNode(_Cube);
            World.AddNode(ActiveCamera);

            List<MeshData> meshData = _SamusNoVisor.GetMeshWithName("polygon6");
            if (meshData.Count > 0) { meshData[0].Visible = false; }
        }

        protected override void OnResize(EventArgs e)
        {
            GL.Viewport(0, 0, Width, Height);
            base.OnResize(e);
        }

        protected override void PreUpdateFrame(FrameEventArgs e)
        {
            KeyboardState ks = Keyboard.GetState();
            if (ks.IsKeyDown(Key.A))
            {
                ActiveCamera.LocalTransform.Rotate(ActiveCamera.LocalTransform.YAxis, -.1f);
            }
            if (ks.IsKeyDown(Key.D))
            {
                ActiveCamera.LocalTransform.Rotate(ActiveCamera.LocalTransform.YAxis, .1f);
            }
            if (ks.IsKeyDown(Key.W))
            {
                ActiveCamera.LocalTransform.Rotate(ActiveCamera.LocalTransform.Right, .1f);
            }
            if (ks.IsKeyDown(Key.S))
            {
                ActiveCamera.LocalTransform.Rotate(ActiveCamera.LocalTransform.Right, -.1f);
            }
            if (ks.IsKeyDown(Key.Q))
            {
                ActiveCamera.LocalTransform.Rotate(ActiveCamera.LocalTransform.Forwards, .1f);
            }
            if (ks.IsKeyDown(Key.E))
            {
                ActiveCamera.LocalTransform.Rotate(ActiveCamera.LocalTransform.Forwards, -.1f);
            }
        }

        protected override void PreRenderFrame(FrameEventArgs e)
        {
            DefaultShader.Projection = ActiveCamera.Projection;
            DefaultShader.View = ActiveCamera.View;

            MaskCubeShader.Projection = ActiveCamera.Projection;
            MaskCubeShader.View = ActiveCamera.View;

            _Samus.LocalTransform.Rotate(_SamusNoVisor.LocalTransform.Up, -.01f);
            _SamusNoVisor.LocalTransform.Rotate(_SamusNoVisor.LocalTransform.Up, -.01f);
        }

        protected override void OnUnload(EventArgs  e)
        {
            DefaultShader.Dispose();
        }
    }
}

