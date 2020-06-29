using System;
using System.Collections.Concurrent;

using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL4;

using BepuPhysics;
using BepuUtilities.Memory;

using BogieEngineCore.Components;
using BogieEngineCore.Entities;
using BogieEngineCore.Physics;
using System.Collections.Generic;

namespace BogieEngineCore
{
    public class BaseGame : GameWindow
    {
        public static BaseGame GlobalGame;

        public ContentManager ContentManager = new ContentManager();
        public Color4 ClearColor = Color4.CornflowerBlue;
        public Camera ActiveCamera;
        public Entity EntityWorld;

        internal Simulation _PhysicsSimulation { get { return _GamePhysics._PhysicsSimulation; } }
        internal GamePhysics _GamePhysics;

        internal ConcurrentDictionary<Entity, Entity> _EntitiesToMove = new ConcurrentDictionary<Entity, Entity>();
        internal ConcurrentDictionary<Component, Entity> _ComponentsToMove = new ConcurrentDictionary<Component, Entity>();



        public BaseGame(int width, int height, string title, int updateRate = 60, int frameRate = 60) : base(width, height, GraphicsMode.Default, title)
        {
            GlobalGame = this;
            EntityWorld = new Entity(null, this);
            ActiveCamera = new Camera(EntityWorld, this);

            Simulation simulation = Simulation.Create(new BufferPool(), new NarrowPhaseCallbacks() { Game = this }, new PoseIntegratorCallbacks() { Game = this });
            SimpleThreadDispatcher threadDispatcher = new SimpleThreadDispatcher(Environment.ProcessorCount);
            _GamePhysics = new GamePhysics(simulation, threadDispatcher);
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
            foreach (KeyValuePair<Entity, Entity> keyValue in _EntitiesToMove)
            {
                if (keyValue.Value != null)
                    keyValue.Value.ForceAddChild(keyValue.Key);
                else
                    keyValue.Key.Parent = null;
            }

            foreach (KeyValuePair<Component, Entity> keyValue in _ComponentsToMove)
            {
                if (keyValue.Value != null)
                    keyValue.Value.ForceAddComponent(keyValue.Key);
                else
                    keyValue.Key.ForceDetachFromEntity();
            }

            foreach (KeyValuePair<Entity, Entity> keyValue in _EntitiesToMove)
            {
                if (keyValue.Value != null)
                {
                    keyValue.Key.ParentChanged();
                    if (!keyValue.Key._FirstTimeSetup)
                    {
                        keyValue.Key._FirstTimeSetup = true;
                        keyValue.Key.EntitySetup();
                        keyValue.Key.InstanceSetup.DynamicInvoke();
                    }
                }
            }


            _EntitiesToMove.Clear();
            _ComponentsToMove.Clear();

            PreUpdateFrame(e);

            _GamePhysics._Timestep((float)e.Time);
            EntityWorld.InvokeEvent(Component.UpdateEvent, true, e.Time);

            PostUpdateFrame(e);
            base.OnUpdateFrame(e);
        }
        protected virtual void PreUpdateFrame(FrameEventArgs e) { }
        protected virtual void PostUpdateFrame(FrameEventArgs e) { }

        protected override void OnRenderFrame(FrameEventArgs e)
        {
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            PreRenderFrame(e);
            EntityWorld.InvokeEvent(Component.RenderEvent, true, null);
            PostRenderFrame(e);

            Context.SwapBuffers();
            base.OnRenderFrame(e);
        }
        protected virtual void PreRenderFrame(FrameEventArgs e) { }
        protected virtual void PostRenderFrame(FrameEventArgs e) { }
    }
}
