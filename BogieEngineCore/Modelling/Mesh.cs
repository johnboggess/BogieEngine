using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using OpenTK;

using BogieEngineCore.Texturing;
using BogieEngineCore.Shading;

namespace BogieEngineCore.Modelling
{
    public class Mesh : IDisposable
    {
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
