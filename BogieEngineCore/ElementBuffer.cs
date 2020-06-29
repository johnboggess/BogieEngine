using OpenTK.Graphics.OpenGL4;

namespace BogieEngineCore
{
    /// <summary>
    /// GPU bufffer containing the indices of vertices indicating which vertices to use to form triangles. 
    /// </summary>
    class ElementBuffer : GPUBuffer
    {
        uint[] _indices;
        /// <summary>
        /// Indices stored in the buffer.
        /// </summary>
        public uint[] Indices { get { return _indices; } }

        public ElementBuffer() : base(BufferTarget.ElementArrayBuffer, BufferUsageHint.StaticDraw) { }

        /// <summary>
        /// Populates the GPU buffer with a set of indices.
        /// </summary>
        /// <param name="indices"></param>
        public void SetIndices(uint[] indices)
        {
            _indices = indices;
            Bind();
            GL.BufferData(BufferTarget.ElementArrayBuffer, indices.Length * sizeof(uint), indices, BufferUsageHint.StaticDraw);
            UnBind();
        }
    }
}
