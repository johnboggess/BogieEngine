using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using OpenTK.Graphics.OpenGL4;

namespace BogieEngineCore
{
    /// <summary>
    /// Represents a buffer of vertices on the GPU
    /// </summary>
    internal class VertexBuffer : GPUBuffer
    {
        private Vertex[] _vertices;
        public Vertex[] Vertice { get { return _vertices; } }

        public VertexBuffer() : base(BufferTarget.ArrayBuffer, BufferUsageHint.StaticDraw) { }

        public void SetVertices(Vertex[] vertices)
        {
            _vertices = vertices;
            Bind();
            GL.BufferData(bufferTarget, vertices.Length * Vertex.Size, vertices, bufferUsageHint);
            UnBind();
        }
    }
}
