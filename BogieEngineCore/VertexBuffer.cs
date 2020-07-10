using System.Diagnostics.CodeAnalysis;

using OpenTK.Graphics.OpenGL4;

using BogieEngineCore.Vertices;
using System;
using System.Runtime.InteropServices;

namespace BogieEngineCore
{
    /// <summary>
    /// Represents a buffer of vertices on the GPU.
    /// </summary>
    internal class VertexBuffer<T> : GPUBuffer where T : struct, Vertex
    {
        /// <summary>
        /// The vertices stored in the buffer.
        /// </summary>
        public T[] Vertices { get { return _vertices; } }

        private T[] _vertices;

        public VertexBuffer() : base(BufferTarget.ArrayBuffer, BufferUsageHint.StaticDraw) { }

        /// <summary>
        /// Populates the GPU buffer with a set of vertices.
        /// </summary>
        /// <param name="vertices"></param>
        public void SetVertices(T[] vertices)
        {
            _vertices = vertices;
            Bind();
            GL.BufferData(bufferTarget, vertices.Length * vertices[0].GetSize(), _vertices, bufferUsageHint);
            UnBind();
        }

        public int GetVertexSize()
        {
            return Vertices[0].GetSize();
        }

        public void SetUpVertexAttributePointers()
        {
            Vertices[0].SetUpVertexAttributePointers();
        }
    }
}
