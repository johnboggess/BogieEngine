using BogieEngineCore.Shading;
using BogieEngineCore.Vertices;
using OpenTK.Graphics.OpenGL4;
using System;

namespace BogieEngineCore
{
    internal class BaseVertexArray
    {
        public bool Disposed { get { return _disposed; } }
        public Type VertexType { get { return vertexType; } }

        protected Type vertexType;

        bool _disposed = false;
        int _handle;
        
        public BaseVertexArray()
        {
            _handle = GL.GenVertexArray();
        }

        public void Bind()
        {
            GL.BindVertexArray(_handle);
        }

        public void UnBind()
        {
            GL.BindVertexArray(0);
        }

        public virtual void Draw() { }

        public void Dispose()
        {
            _disposed = true;
            GL.DeleteBuffer(_handle);
        }
    }

    /// <summary>
    /// GPU array representing the vertices for a mesh, attributes of each vertex, and the order the vertices should be rendered in
    /// </summary>
    internal class VertexArray<T> : BaseVertexArray, IDisposable where T : struct, Vertex
    {
        int _handle;
        VertexBuffer<T> _vbo;
        ElementBuffer _ebo;

        public VertexArray()
        {
            _handle = GL.GenVertexArray();
            vertexType = typeof(T);
        }

        public void Setup(VertexBuffer<T> vbo, ElementBuffer ebo)
        {
            _ebo = ebo;
            _vbo = vbo;
            Bind();

            vbo.Bind();

            vbo.SetUpVertexAttributePointers();

            ebo.Bind();

            UnBind();

        }

        public override void Draw()
        {
            GL.DrawElements(PrimitiveType.Triangles, _ebo.Indices.Length, DrawElementsType.UnsignedInt, 0);
        }
    }
}
