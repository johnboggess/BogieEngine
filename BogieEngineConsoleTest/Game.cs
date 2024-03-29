﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

using OpenTK;
using OpenTK.Input;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL4;

using BogieEngineCore;
using BogieEngineCore.Shading;
using BogieEngineCore.Modelling;
using BogieEngineCore.Texturing;
using BogieEngineCore.Entities;
using BogieEngineCore.Materials;
using BogieEngineCore.Lighting;
using BogieEngineCore.Vertices;
using BogieEngineCore.Components;
using BogieEngineConsoleTest.Entities;
using BogieEngineConsoleTest.Components;

using SixLabors.ImageSharp;

namespace BogieEngineConsoleTest
{
    //varia suit from https://sketchfab.com/3d-models/varia-suit-79c802129f9a4945aba62a607892ac31
    //xenomorph from https://sketchfab.com/3d-models/xeno-raven-e444a88e999549d99eacb1ea0f8e04e4
    class Game : BaseGame
    {
        public static System.Numerics.Vector3 Gravity = new System.Numerics.Vector3(0, -10, 0);

        public ModelData CubeModel;
        public ModelData XenoModel;
        public ModelData SamusModel;
        public ModelData SuzanneModel;
        public ModelInstance CubeInstance;
        public ModelInstance NormalCubeInstance;
        public ModelInstance XenoInstance;
        public ModelInstance SamusInstance;

        public DefaultShader DefaultShader;
        public BlinnPhongShader BlinnPhongShader;
        public NormalShader NormalShader;

        public Texture CubeTex;
        public Texture ContainerTex;
        public Texture WhiteTex;
        public Texture BlackTex;
        public Texture ContainerSpecularTex;
        public Texture WhiteSpecular;
        public Texture BlackSpecular;

        public Texture XenoHeadS;
        public Texture XenoBodyS;
        public Texture XenoHeadN;
        public Texture XenoBodyN;

        public Texture Brick2Tex;
        public Texture Brick2Norm;

        public Samus _Samus;
        public Samus _SamusNoVisor;
        public Entity _SamusRelativeCube;
        public Samus _MiniSamus;
        public Xeno _Xeno;
        public Player Player;
        public Suzanne Suzanne;

        public Box FallingBlock;
        public Entity FloorEntity;

        int frame = 0;

        public Game(int width, int height, string title, int updateRate = 60, int frameRate = 60) : base(width, height, title, updateRate, frameRate)
        {
        }

        protected override void Loading(EventArgs e)
        {
            ClearColor = System.Drawing.Color.Black;

            DefaultShader = new DefaultShader(this);
            BlinnPhongShader = new BlinnPhongShader(this);
            NormalShader = new NormalShader(this);

            CubeTex = ContentManager.LoadTexture("Resources/Textures/Brick.jpg", TextureUnit.Texture0);
            ContainerTex = ContentManager.LoadTexture("Resources/Textures/container.png", TextureUnit.Texture0); 
            WhiteTex = ContentManager.LoadTexture("Resources/Textures/BlankWhite.png", TextureUnit.Texture0);
            BlackTex = ContentManager.LoadTexture("Resources/Textures/BlankBlack.png", TextureUnit.Texture0);

            ContainerSpecularTex = ContentManager.LoadTexture("Resources/Textures/container_specular.png", TextureUnit.Texture1);
            WhiteSpecular = ContentManager.LoadTexture("Resources/Textures/BlankWhite.png", TextureUnit.Texture1);
            BlackSpecular = ContentManager.LoadTexture("Resources/Textures/BlankBlack.png", TextureUnit.Texture1);

            XenoHeadS = ContentManager.LoadTexture("Resources/Models/Xeno-raven/Maps/XenoRaven_Head_S.tga", TextureUnit.Texture1);
            XenoBodyS = ContentManager.LoadTexture("Resources/Models/Xeno-raven/Maps/XenoRaven_Body_S.tga", TextureUnit.Texture1);

            XenoHeadN = ContentManager.LoadTexture("Resources/Models/Xeno-raven/Maps/XenoRaven_head_N.tga", TextureUnit.Texture2);
            XenoBodyN = ContentManager.LoadTexture("Resources/Models/Xeno-raven/Maps/XenoRaven_body_N.tga", TextureUnit.Texture2);

            Brick2Tex = ContentManager.LoadTexture("Resources/Textures/brick2.jpg", TextureUnit.Texture0);
            Brick2Norm = ContentManager.LoadTexture("Resources/Textures/brick2N.jpg", TextureUnit.Texture2);
            
            CubeModel = ContentManager.LoadModel<PhongMaterial>("Resources/Models/Cube.obj", BlinnPhongShader, new TangetSpaceVertexDefinition());
            CubeModel.Meshes[0].DefaultMaterial = new PhongMaterial() { DiffuseTexture = ContainerTex, SpecularTexture = ContainerSpecularTex, Shininess = 128f };
            CubeModel.Meshes[0].DefaultShader = BlinnPhongShader;

            XenoModel = ContentManager.LoadModel<NormalMaterial>("Resources/Models/xeno-raven/source/XenoRaven.fbx", NormalShader, new TangetSpaceVertexDefinition());
            //mesh 0: body
            //mesh 1: head
            ((NormalMaterial)XenoModel.Meshes[0].DefaultMaterial).NormalTexture = XenoBodyN;
            ((NormalMaterial)XenoModel.Meshes[0].DefaultMaterial).SpecularTexture = XenoBodyS;
            ((NormalMaterial)XenoModel.Meshes[0].DefaultMaterial).Shininess = 32f;
            ((NormalMaterial)XenoModel.Meshes[1].DefaultMaterial).NormalTexture = XenoHeadN;
            ((NormalMaterial)XenoModel.Meshes[1].DefaultMaterial).SpecularTexture = XenoHeadS;
            ((NormalMaterial)XenoModel.Meshes[1].DefaultMaterial).Shininess = 32f;

            SamusModel = ContentManager.LoadModel<PhongMaterial>("Resources/Models/VariaSuit/DolBarriersuit.obj", BlinnPhongShader, new DefaultVertexDefinition());
            foreach (MeshData meshData in SamusModel.Meshes)
            {
                ((PhongMaterial)meshData.DefaultMaterial).SpecularTexture = WhiteSpecular;
                ((PhongMaterial)meshData.DefaultMaterial).Shininess = 256f;
            }

            SuzanneModel = ContentManager.LoadModel<PhongMaterial>("Resources/Models/Monkey.obj", BlinnPhongShader, new DefaultVertexDefinition());
            ((PhongMaterial)SuzanneModel.Meshes[0].DefaultMaterial).DiffuseTexture = WhiteTex;
            ((PhongMaterial)SuzanneModel.Meshes[0].DefaultMaterial).SpecularTexture = WhiteSpecular;
            ((PhongMaterial)SuzanneModel.Meshes[0].DefaultMaterial).Shininess = 128;

            SamusInstance = SamusModel.CreateInstance();
            CubeInstance = CubeModel.CreateInstance();
            NormalCubeInstance = CubeModel.CreateInstance(new NormalMaterial() { DiffuseTexture = Brick2Tex, NormalTexture = Brick2Norm, SpecularTexture = WhiteSpecular, Shininess = 16f }, NormalShader);
            XenoInstance = XenoModel.CreateInstance();

            ActiveCamera.LocalTransform.Position = new Vector3(0, 0, 3);
            ActiveCamera.ForceAddComponent(new FPSCameraScript(ActiveCamera));

            Player = new Player(EntityWorld, this, ActiveCamera);

            _Xeno = new Xeno(EntityWorld, this);
            _Xeno.LocalTransform.ScaleBy(new Vector3(.01f, .01f, .01f));
            _Xeno.LocalTransform.Position = new Vector3(5, -9, 0);
            _Xeno.LocalTransform.Rotate(_Xeno.LocalTransform.Right, -1.57f);

            _Samus = new Samus(EntityWorld, this);
            _Samus.LocalTransform.ScaleBy(new Vector3(.1f, .1f, .1f));
            _Samus.LocalTransform.Position = new Vector3(-.5f, -8, 0);

            _SamusNoVisor = new Samus(EntityWorld, this);
            _SamusNoVisor.LocalTransform.ScaleBy(new Vector3(.1f, .1f, .1f));
            _SamusNoVisor.LocalTransform.Position = new Vector3(.5f, -8, 0);
            
            _SamusNoVisor.InstanceSetup = new Action(() =>
            {
                _SamusNoVisor.ForceRemoveComponent(_SamusNoVisor.Model);
                _SamusNoVisor.Model = new BogieEngineCore.Components.Model(SamusModel.CreateInstance());
                _SamusNoVisor.ForceAddComponent(_SamusNoVisor.Model);

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
            BogieEngineCore.Components.Model model = new BogieEngineCore.Components.Model(CubeInstance);
            _SamusRelativeCube.QueueAddComponent(model);

            _MiniSamus = new Samus(_SamusRelativeCube, this); 
            _MiniSamus.LocalTransform.ScaleBy(new Vector3(.1f, .1f, .1f));


            FloorEntity = new Entity(EntityWorld, this);
            FloorEntity.LocalTransform.Position = new Vector3(0, -10, 0);
            FloorEntity.LocalTransform.Scale = new Vector3(20, 3, 20);
            FloorEntity.ForceAddComponent(new BogieEngineCore.Components.Model(CubeInstance));
            FloorEntity.InstanceSetup = new Action(() =>
            {
                FloorEntity.ForceAddComponent(new StaticBox(FloorEntity.GlobalTransform));
            });

            NormalTest normalTest = new NormalTest(EntityWorld, this);
            normalTest.LocalTransform.Position = new Vector3(-5, -8, 0);
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

            BlinnPhongShader.Projection = ActiveCamera.Projection;
            BlinnPhongShader.View = ActiveCamera.View;

            NormalShader.Projection = ActiveCamera.Projection;
            NormalShader.View = ActiveCamera.View;//todo: track all shaders and have the cameras auto these values

            _Samus.LocalTransform.Rotate(_Samus.LocalTransform.Up, -.01f);
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
