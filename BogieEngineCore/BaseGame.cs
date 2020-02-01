using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;

using OpenTK;
using OpenTK.Input;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL4;

using BepuPhysics;
using BepuUtilities.Memory;

using BogieEngineCore.Nodes;
using BogieEngineCore.Physics;

namespace BogieEngineCore
{
    public class BaseGame : GameWindow
    {
        public ContentManager ContentManager = new ContentManager();
        public Color4 ClearColor = Color4.CornflowerBlue;
        public Camera ActiveCamera;
        public Root World;
        public Simulation PhysicsSimulation;


        private SimpleThreadDispatcher threadDispatcher = new SimpleThreadDispatcher(Environment.ProcessorCount);

        public BaseGame(int width, int height, string title, int updateRate = 60, int frameRate = 60) : base(width, height, GraphicsMode.Default, title)
        {
            ActiveCamera = new Camera(this);
            World = new Root(this);

            PhysicsSimulation = Simulation.Create(new BufferPool(), new NarrowPhaseCallbacks(), new PoseIntegratorCallbacks(new System.Numerics.Vector3(0, -10, 0)));
            Run(updateRate, frameRate);
        }

        protected override void OnLoad(EventArgs e)
        {
            GL.Enable(EnableCap.DepthTest);
            GL.Enable(EnableCap.CullFace);
            GL.ClearColor(ClearColor);

            Loading(e);
            base.OnLoad(e);
        }
        protected virtual void Loading(EventArgs e) { }

        protected override void OnUpdateFrame(FrameEventArgs e)
        {
            PreUpdateFrame(e);

            PhysicsSimulation.Timestep((float)e.Time, threadDispatcher);
            World.Process((float)e.Time, new Transform());

            PostUpdateFrame(e);
            base.OnUpdateFrame(e);
        }
        protected virtual void PreUpdateFrame(FrameEventArgs e) { }
        protected virtual void PostUpdateFrame(FrameEventArgs e) { }

        protected override void OnRenderFrame(FrameEventArgs e)
        {
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            PreRenderFrame(e);
            World.Draw((float)e.Time, new Transform());
            PostRenderFrame(e);

            Context.SwapBuffers();
            base.OnRenderFrame(e);
        }
        protected virtual void PreRenderFrame(FrameEventArgs e) { }
        protected virtual void PostRenderFrame(FrameEventArgs e) { }
    }
}
