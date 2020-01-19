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
        public string Name { get { return _name; } }

        internal VertexArray _VertexArray;

        private string _name;

        internal Mesh(string name, VertexArray vertexArray)
        {
            _name = name;
            _VertexArray = vertexArray;
        }

        public void Draw()
        {
            _VertexArray.Draw();
        }

        public void Dispose()
        {
            ((IDisposable)_VertexArray).Dispose();
        }

        internal void BindVertexArray()
        {
            _VertexArray.Bind();
        }
    }
}
