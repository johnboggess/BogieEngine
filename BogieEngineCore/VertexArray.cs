using BogieEngineCore.Shading;
using BogieEngineCore.Vertices;
using OpenTK.Graphics.OpenGL4;
using System;

namespace BogieEngineCore
{
    /// <summary>
    /// GPU array representing the vertices for a mesh, attributes of each vertex, and the order the vertices should be rendered in
    /// </summary>
    internal class VertexArray : IDisposable
    {
        public bool Disposed { get { return _disposed; } }

        bool _disposed = false;
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

            vbo.SetUpVertexAttributePointers();

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
