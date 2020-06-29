using BogieEngineCore.Shading;
using OpenTK.Graphics.OpenGL4;

namespace BogieEngineCore
{
    /// <summary>
    /// GPU array representing the vertices for a mesh, attributes of each vertex, and the order the vertices should be rendered in
    /// </summary>
    class VertexArray : IDisposable
    {
        public bool Disposed { get { return _disposed; } }
        private bool _disposed = false;

        int _handle;
        VertexBuffer _vbo;
        ElementBuffer _ebo;

        public VertexArray()
        {
            _handle = GL.GenVertexArray();
        }

        public void Setup(VertexBuffer vbo, ElementBuffer ebo)
        {
            _ebo = ebo;
            _vbo = vbo;
            Bind();

            vbo.Bind();
            GL.VertexAttribPointer(Shader.VertexPositionLocation, 3, VertexAttribPointerType.Float, false, Vertex.Size, 0);
            GL.EnableVertexAttribArray(Shader.VertexPositionLocation);
            GL.VertexAttribPointer(Shader.VertexUVLocation, 2, VertexAttribPointerType.Float, false, Vertex.Size, 3 * sizeof(float));
            GL.EnableVertexAttribArray(Shader.VertexUVLocation);

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
            GL.DrawElements(PrimitiveType.Triangles, _ebo.Indices.Length, DrawElementsType.UnsignedInt, 0);
        }

        public void Dispose()
        {
            _disposed = true;
            GL.DeleteBuffer(_handle);
        }

    }
}
