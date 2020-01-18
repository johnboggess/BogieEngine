using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using OpenTK;

using BogieEngineCore.Textures;
using BogieEngineCore.Shaders;
namespace BogieEngineCore.Models
{
    public class Mesh : IDisposable
    {
        public Matrix4 Transform;
        public Shader Shader;
        public bool Disposed => ((IDisposable)_VertexArray).Disposed;
        public List<Texture> Textures = new List<Texture>();

        internal VertexArray _VertexArray;

        internal Mesh(VertexArray vertexArray)
        {
            _VertexArray = vertexArray;
        }

        public void Draw()
        {
            _VertexArray.Bind();
            foreach(Texture texture in Textures)
                texture.Bind();
            _VertexArray.Draw();
        }

        public void Dispose()
        {
            ((IDisposable)_VertexArray).Dispose();
        }
    }
}
