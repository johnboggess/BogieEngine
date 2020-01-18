using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using BogieEngineCore.Shading;
namespace BogieEngineCore.Modelling
{
    public class MeshData : IDisposable
    {
        public Shader Shader;
        public bool Visible = true;
        public bool Disposed => ((IDisposable)_Mesh).Disposed;

        internal Mesh _Mesh;

        internal MeshData(Mesh mesh)
        {
            _Mesh = mesh;
        }

        public void Draw()
        {
            _Mesh.Draw();
        }

        public void Dispose()
        {
            ((IDisposable)_Mesh).Dispose();
        }
    }
}
