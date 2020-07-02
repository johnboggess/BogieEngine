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
using BogieEngineCore.Modelling;
using BogieEngineCore.Texturing;
using BogieEngineCore.Entities;

using BogieEngineConsoleTest.Entities;
using BogieEngineConsoleTest.Components;
using System.Threading;

namespace BogieEngineConsoleTest
{
    class Game : BaseGame
    {
        public static System.Numerics.Vector3 Gravity = new System.Numerics.Vector3(0, -10, 0);

        public Shader DefaultShader;
        public Shader MaskCubeShader;
        public Texture CubeTex;

        public Samus _Samus;
        public Samus _SamusNoVisor;
        public Entity _SamusRelativeCube;
        public Samus _MiniSamus;
        public Player Player;

        public Box FallingBlock;
        public Entity FloorEntity;

        int frame = 0;

        public Game(int width, int height, string title, int updateRate = 60, int frameRate = 60) : base(width, height, title, updateRate, frameRate)
        {
        }

        protected override void Loading(EventArgs e)
        {
            DefaultShader = new Shader("Resources/Shaders/default.vert", "Resources/Shaders/default.frag");
            MaskCubeShader = new Shader("Resources/Shaders/default.vert", "Resources/Shaders/repeatTexture.frag");

            CubeTex = ContentManager.LoadTexture("Resources/Textures/Brick.jpg", TextureUnit.Texture0);

            ActiveCamera.LocalTransform.Position = new Vector3(0, 0, 3);
            ActiveCamera.ForceAddComponent(new FPSCameraScript(ActiveCamera));

            Player = new Player(EntityWorld, this, ActiveCamera);

            //downloaded from https://sketchfab.com/3d-models/varia-suit-79c802129f9a4945aba62a607892ac31
            _Samus = new Samus(EntityWorld, this);
            _Samus.LocalTransform.ScaleBy(new Vector3(.1f, .1f, .1f));
            _Samus.LocalTransform.Position = new Vector3(-.5f, -8, 0);

            _SamusNoVisor = new Samus(EntityWorld, this);
            _SamusNoVisor.LocalTransform.ScaleBy(new Vector3(.1f, .1f, .1f));
            _SamusNoVisor.LocalTransform.Position = new Vector3(.5f, -8, 0);
            _SamusNoVisor.InstanceSetup = new Action(() =>
            {
                List<MeshInstance> meshData = _SamusNoVisor.Model.GetMeshWithName("polygon6");
                if (meshData.Count > 0) { meshData[0].Visible = false; }
            });

            FallingBlock = new Box(EntityWorld, false, new Vector3(0, 0, -5), new Vector3(1, 3.5f, 1), this);
            FallingBlock.InstanceSetup = new Action(() =>
            {
                FallingBlock.GetComponet<GravityScript>(nameof(GravityScript)).Gravity = Gravity;
                FallingBlock.RigidBox.AngularVelocity= new System.Numerics.Vector3(1);
                FallingBlock.RigidBox.Velocity = new System.Numerics.Vector3(1);
            });


            _SamusRelativeCube = new Entity(_Samus, this);
            BogieEngineCore.Components.Model model = BogieEngineCore.Components.Model.CreateModel("Resources/Models/Cube.obj", ContentManager, DefaultShader);
            model.GetMesh(0).Textures.Add(CubeTex);
            _SamusRelativeCube.QueueAddComponent(model);

            _MiniSamus = new Samus(_SamusRelativeCube, this); 
            _MiniSamus.LocalTransform.ScaleBy(new Vector3(.1f, .1f, .1f));


            FloorEntity = new Entity(EntityWorld, this);
            FloorEntity.LocalTransform.Position = new Vector3(0, -10, 0);
            FloorEntity.LocalTransform.Scale = new Vector3(20, 3, 20);
            FloorEntity.ForceAddComponent(BogieEngineCore.Components.Model.CreateModel("Resources/Models/Cube.obj", ContentManager, DefaultShader));
            FloorEntity.InstanceSetup = new Action(() =>
            {
                FloorEntity.ForceAddComponent(BogieEngineCore.Components.StaticBody.CreateStaticBody(FloorEntity, new BogieEngineCore.Physics.Shapes.Box(), false));
                ((BogieEngineCore.Components.Model)FloorEntity.GetComponent("Model")).GetMesh(0).Textures.Add(CubeTex);
            });

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

            _Samus.LocalTransform.Rotate(_Samus.LocalTransform.Up, -.01f);
            _SamusNoVisor.LocalTransform.Rotate(_SamusNoVisor.LocalTransform.Up, -.01f);
            _SamusRelativeCube.LocalTransform.Rotate(_SamusRelativeCube.LocalTransform.Right, -.01f);
            _MiniSamus.LocalTransform.Position = new Vector3(0, (float)Math.Sin(frame / 100f), 0);
            Console.WriteLine("Player y " + Player.GlobalTransform.Position.Y);
            Console.WriteLine();
            frame += 1;
        }

        protected override void OnUnload(EventArgs e)
        {
            DefaultShader.Dispose();
        }
    }
}
