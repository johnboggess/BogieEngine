﻿using BogieEngineCore.Vertices;
using BogieEngineCore.Materials;
using BogieEngineCore.Shading;

namespace BogieEngineCore.Modelling
{
    /// <summary>
    /// Collections of vertices representing a shape.
    /// </summary>
    public class MeshData : IDisposable
    {
        /// <summary>
        /// Has the object been disposed?
        /// </summary>
        public bool Disposed => ((IDisposable)_VertexArray).Disposed;

        /// <summary>
        /// Name of the mesh.
        /// </summary>
        public readonly string Name;

        /// <summary>
        /// The vertices that define the mesh
        /// </summary>
        internal VertexArray _VertexArray;

        /// <summary>
        /// Creates a mesh
        /// </summary>
        /// <param name="name">Name of the mesh.</param>
        /// <param name="vertexArray">The vertices that define the mesh.</param>
        internal MeshData(string name, VertexArray vertexArray)
        {
            Name = name;
            _VertexArray = vertexArray;
        }


        /// <summary>
        /// Dispose of the mesh.
        /// </summary>
        public void Dispose()
        {
            ((IDisposable)_VertexArray).Dispose();
        }

        public MeshInstance CreateInstance(Material material, Shader shader)
        {
            MeshInstance meshInstance = new MeshInstance(this);
            meshInstance.Material = material;
            meshInstance.Shader = shader;
            return meshInstance;
        }

        /// <summary>
        /// Draw the mesh to the screen. Vertex array and textures must be bounded first.
        /// </summary>
        internal void _Draw()
        {
            _VertexArray.Draw();
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
