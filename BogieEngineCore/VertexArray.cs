using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using OpenTK.Graphics.OpenGL4;
namespace BogieEngineCore
{
    class VertexArray
    {
        int _handle;
        VertexBuffer vbo;

        public VertexArray()
        {
            _handle = GL.GenVertexArray();
        }

        public void Setup(Vector3[] vertices)
        {
            vbo = new VertexBuffer();
            vbo.SetVertices(vertices);
            Setup(vbo);
        }

        public void Setup(VertexBuffer vbo)
        {
            Bind();
            this.vbo = vbo;
            vbo.Bind();
            GL.VertexAttribPointer(Shader.VertexPositionLocation, 3, VertexAttribPointerType.Float, false, 3 * sizeof(float), 0);
            GL.EnableVertexAttribArray(Shader.VertexPositionLocation);
            UnBind();

        }

        public void Bind()
        {
            GL.BindVertexArray(_handle);
        }

        public void UnBind()
        {
            GL.BindVertexArray(0);
        }

        public void Dispose()
        {
            vbo.Dispose();
            GL.DeleteBuffer(_handle);
        }

    }
}
