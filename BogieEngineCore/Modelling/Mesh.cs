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
    /// <summary>
    /// Collections of vertices representing a shape.
    /// </summary>
    internal class Mesh : IDisposable
    {
        /// <summary>
        /// Has the object been disposed?
        /// </summary>
        public bool Disposed => ((IDisposable)_VertexArray).Disposed;

        /// <summary>
        /// Name of the mesh.
        /// </summary>
        public string Name { get { return _name; } }

        /// <summary>
        /// The vertices that define the mesh
        /// </summary>
        internal VertexArray _VertexArray;

        /// <summary>
        /// Name of the mesh.
        /// </summary>
        private string _name;

        /// <summary>
        /// Creates a mesh
        /// </summary>
        /// <param name="name">Name of the mesh.</param>
        /// <param name="vertexArray">The vertices that define the mesh.</param>
        internal Mesh(string name, VertexArray vertexArray)
        {
            _name = name;
            _VertexArray = vertexArray;
        }

        /// <summary>
        /// Draw the meh to the screen.
        /// </summary>
        public void Draw()
        {
            _VertexArray.Draw();
        }

        /// <summary>
        /// Dispose of the mesh.
        /// </summary>
        public void Dispose()
        {
            ((IDisposable)_VertexArray).Dispose();
        }

        /// <summary>
        /// Binds the vertex array.
        /// </summary>
        internal void _BindVertexArray()
        {
            _VertexArray.Bind();
        }
    }
}
