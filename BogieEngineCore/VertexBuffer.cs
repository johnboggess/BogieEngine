using System.Diagnostics.CodeAnalysis;
using System.Numerics;
using System;
using System.Runtime.InteropServices;

using OpenTK.Graphics.OpenGL4;

using BogieEngineCore.Vertices;

namespace BogieEngineCore
{
    /// <summary>
    /// Represents a buffer of vertices on the GPU.
    /// </summary>
    internal class VertexBuffer : GPUBuffer
    {

        /// <summary>
        /// The vertices stored in the buffer.
        /// </summary>
        public float[] Vertices { get { return _vertices; } }
        private VertexDefinition VertexDefinition { get { return _vertexDefinition; } }

        private float[] _vertices;
        private VertexDefinition _vertexDefinition;

        /// <summary>
        /// Populates the GPU buffer with a set of vertices.
        /// </summary>
        /// <param name="vertices"></param>
        public VertexBuffer(float[] vertices, VertexDefinition vertexDefinition) : base(BufferTarget.ArrayBuffer, BufferUsageHint.StaticDraw)
        {
            _vertexDefinition = vertexDefinition;
            _vertices = vertices;
            Bind();
            GL.BufferData(bufferTarget, vertices.Length * sizeof(float), _vertices, bufferUsageHint);
            UnBind();
        }

        public int GetVertexSize()
        {
            return VertexDefinition.GetVertexSizeInBytes();
        }

        public void SetUpVertexAttributePointers()
        {
            VertexDefinition.SetUpVertexAttributePointers();
        }

        internal Vector3 GetPosition(int vertexIndex)
        {
            return _vertexDefinition._GetPosition(_vertices, vertexIndex);
        }
    }
}
