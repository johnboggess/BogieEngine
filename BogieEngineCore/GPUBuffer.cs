using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using OpenTK.Graphics.OpenGL4;
namespace BogieEngineCore
{
    class GPUBuffer : IDisposable
    {
        public bool Disposed { get { return _disposed; } }
        private bool _disposed = false;

        protected int _handle;
        protected BufferTarget bufferTarget;
        protected BufferUsageHint bufferUsageHint;

        public GPUBuffer(BufferTarget bufferTarget, BufferUsageHint bufferUsageHint)
        {
            _handle = GL.GenBuffer();
            this.bufferTarget = bufferTarget;
            this.bufferUsageHint = bufferUsageHint;
        }

        public void Bind()
        {
            GL.BindBuffer(bufferTarget, _handle);
        }

        public void UnBind()
        {
            GL.BindBuffer(bufferTarget, 0);
        }

        public void Dispose()
        {
            _disposed = true;
            GL.DeleteBuffer(_handle);
        }
    }
}
