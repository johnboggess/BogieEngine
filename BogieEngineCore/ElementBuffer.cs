using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using OpenTK.Graphics.OpenGL4;

namespace BogieEngineCore
{
    class ElementBuffer : GPUBuffer
    {
        uint[] _indices;
        public uint[] Indices { get { return _indices; } }

        public ElementBuffer() : base(BufferTarget.ElementArrayBuffer, BufferUsageHint.StaticDraw) { }

        public void SetIndices(uint[] indices)
        {
            _indices = indices;
            Bind();
            GL.BufferData(BufferTarget.ElementArrayBuffer, indices.Length * sizeof(uint), indices, BufferUsageHint.StaticDraw);
            UnBind();
        }
    }
}
