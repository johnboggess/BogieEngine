using OpenTK.Graphics.OpenGL4;

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
        public Vertex[] Vertices { get { return _vertices; } }

        private Vertex[] _vertices;

        public VertexBuffer() : base(BufferTarget.ArrayBuffer, BufferUsageHint.StaticDraw) { }


        /// <summary>
        /// Populates the GPU buffer with a set of vertices.
        /// </summary>
        /// <param name="vertices"></param>
        public void SetVertices(Vertex[] vertices)
        {
            _vertices = vertices;
            Bind();
            GL.BufferData(bufferTarget, vertices.Length * Vertex.Size, vertices, bufferUsageHint);
            UnBind();
        }
    }
}
