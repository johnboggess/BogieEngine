using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using OpenTK.Graphics.OpenGL4;

namespace BogieEngineCore
{
    class ElementBuffer
    {
        int _handle;
        uint[] _indices;
        public uint[] Indices { get { return _indices; } }

        public ElementBuffer()
        {
            _handle = GL.GenBuffer();
        }

        public void SetIndices(uint[] indices)
        {
            _indices = indices;
            Bind();
            GL.BufferData(BufferTarget.ElementArrayBuffer, indices.Length * sizeof(uint), indices, BufferUsageHint.StaticDraw);
            UnBind();
        }

        public void Bind()
        {
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, _handle);
        }

        public void UnBind()
        {
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, 0);
        }

        public void Dispose()
        {
            GL.DeleteBuffer(_handle);
        }
    }
}
