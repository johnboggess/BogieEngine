using System.Collections.Generic;

using BogieEngineCore.Materials;
using BogieEngineCore.Shading;
using BogieEngineCore.Texturing;

namespace BogieEngineCore.Modelling
{
    /// <summary>
    /// A specific instance of a mesh and its properties.
    /// </summary>
    public class MeshInstance : IDisposable
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
        public string Name { get { return _MeshData.Name; } }
        /// <summary>
        /// Has the mesh been disposed?
        /// </summary>
        public bool Disposed => ((IDisposable)_MeshData).Disposed;
        /// <summary>
        /// The material of this mesh instance.
        /// </summary>
        public Material Material;

        internal MeshData _MeshData;

        /// <summary>
        /// Create a specific instancee of a mesh.
        /// </summary>
        /// <param name="meshData">The mesh defining the shape of the object.</param>
        internal MeshInstance(MeshData meshData)
        {
            _MeshData = meshData;
        }

        /// <summary>
        /// Draw this mesh instance
        /// </summary>
        public void Draw()
        {
            _MeshData._BindVertexArray();
            Material.SetMaterialUniform("material", Shader);
            /*foreach (Texture texture in Textures)
                texture.Bind();*/
            _MeshData._Draw();
        }

        /// <summary>
        /// Dispose of the object.
        /// </summary>
        public void Dispose()
        {
            ((IDisposable)_MeshData).Dispose();
        }
    }
}
