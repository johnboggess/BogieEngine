using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using BogieEngineCore.Shading;
using BogieEngineCore.Texturing;
namespace BogieEngineCore.Modelling
{
    public class MeshData : IDisposable
    {
        public Shader Shader;
        public bool Visible = true;
        public string Name { get { return _Mesh.Name; } }
        public bool Disposed => ((IDisposable)_Mesh).Disposed;
        public List<Texture> Textures = new List<Texture>();

        internal Mesh _Mesh;

        internal MeshData(Mesh mesh)
        {
            _Mesh = mesh;
        }

        public void Draw()
        {
            _Mesh.BindVertexArray();
            foreach (Texture texture in Textures)
                texture.Bind();
            _Mesh.Draw();
        }

        public void Dispose()
        {
            ((IDisposable)_Mesh).Dispose();
        }
    }
}
