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
        ElementBuffer ebo;

        public VertexArray()
        {
            _handle = GL.GenVertexArray();
        }

        public void Setup(Vector3[] vertices, uint[] indices)
        {
            vbo = new VertexBuffer();
            vbo.SetVertices(vertices);
            ebo = new ElementBuffer();
            ebo.SetIndices(indices);

            Setup(vbo, ebo);
        }

        public void Setup(VertexBuffer vbo, ElementBuffer ebo)
        {
            this.ebo = ebo;
            this.vbo = vbo;
            Bind();

            vbo.Bind();
            GL.VertexAttribPointer(Shader.VertexPositionLocation, 3, VertexAttribPointerType.Float, false, 3 * sizeof(float), 0);
            GL.EnableVertexAttribArray(Shader.VertexPositionLocation);

            ebo.Bind();

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

        public void Draw()
        {
            GL.DrawElements(PrimitiveType.Triangles, ebo.Indices.Length, DrawElementsType.UnsignedInt, 0);
        }

        public void Dispose()
        {
            vbo.Dispose();
            GL.DeleteBuffer(_handle);
        }

    }
}
