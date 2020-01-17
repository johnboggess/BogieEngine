using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL4;

namespace BogieEngineCore
{
    public class Window : GameWindow
    {
        public Color4 ClearColor = Color4.White;

        internal VertexArray va;
        internal Shader shader;
        public Window(int width, int height, string title, int updateRate = 30, int frameRate = 30) : base(width, height, OpenTK.Graphics.GraphicsMode.Default, title)
        {
            Run(updateRate, frameRate);
        }

        protected override void OnLoad(EventArgs e)
        {
            GL.ClearColor(1, 1, 1, 1);

            Vector3[] vertices =
            {
                new Vector3(-0.5f, -0.5f, 0.0f),
                new Vector3(0.5f, -0.5f, 0.0f),
                new Vector3(0.0f,  0.5f, 0.0f)
            };

            shader = new Shader("Shaders/default.vert", "Shaders/default.frag");

            va = new VertexArray();
            va.Setup(vertices);

            base.OnLoad(e);
        }

        protected override void OnResize(EventArgs e)
        {
            GL.Viewport(0, 0, Width, Height);
            base.OnResize(e);
        }

        protected override void OnRenderFrame(FrameEventArgs e)
        {
            GL.Clear(ClearBufferMask.ColorBufferBit);

            shader.Use();
            va.Bind();
            GL.DrawArrays(PrimitiveType.Triangles, 0, 3);
            Context.SwapBuffers();
            base.OnRenderFrame(e);
        }

        protected override void OnUnload(EventArgs e)
        {
            va.Dispose();
            shader.Dispose();
        }
    }
}

