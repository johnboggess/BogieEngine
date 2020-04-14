using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using BogieEngineCore.Shading;
using BogieEngineCore.Texturing;
namespace BogieEngineCore.Modelling
{
    /// <summary>
    /// A specific instance of a mesh and its properties.
    /// </summary>
    public class MeshData : IDisposable
    {
        /// <summary>
        /// The shader to apply to this mesh instance.
        /// </summary>
        public Shader Shader;
        /// <summary>
        /// Is this mesh instance visible?
        /// </summary>
        public bool Visible = true;
        /// <summary>
        /// The name of the mesh.
        /// </summary>
        public string Name { get { return _Mesh.Name; } }
        /// <summary>
        /// Has the mesh been disposed?
        /// </summary>
        public bool Disposed => ((IDisposable)_Mesh).Disposed;
        /// <summary>
        /// The textures for this mesh instance.
        /// </summary>
        public List<Texture> Textures = new List<Texture>();

        internal Mesh _Mesh;

        /// <summary>
        /// Create a specific instancee of a mesh.
        /// </summary>
        /// <param name="mesh">The mesh defining the shape of the object.</param>
        internal MeshData(Mesh mesh)
        {
            _Mesh = mesh;
        }

        /// <summary>
        /// Draw this mesh instance
        /// </summary>
        public void Draw()
        {
            _Mesh._BindVertexArray();
            foreach (Texture texture in Textures)
                texture.Bind();
            _Mesh.Draw();
        }

        /// <summary>
        /// Dispose of the object.
        /// </summary>
        public void Dispose()
        {
            ((IDisposable)_Mesh).Dispose();
        }
    }
}
