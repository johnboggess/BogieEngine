using System.Collections.Generic;
using OpenTK;

using BogieEngineCore.Shading;
using BogieEngineCore.Materials;
namespace BogieEngineCore.Modelling
{
    public class ModelData
    {
        /// <summary>
        /// The meshes that make up the model.
        /// </summary>
        public List<MeshData> Meshes = new List<MeshData>();

        public ModelData(List<MeshData> meshes)
        {
            Meshes = meshes;
        }
        
        public ModelInstance CreateInstance(Material material, Shader shader)
        {
            List<MeshInstance> meshInstances = new List<MeshInstance>();
            foreach(MeshData mesh in Meshes)
            {
                meshInstances.Add(mesh.CreateInstance(material, shader));
            }
            return new ModelInstance(meshInstances);
        }

        /// <summary>
        /// Find meshes with the given name.
        /// </summary>
        /// <param name="name">The name of the meshes to search for.</param>
        /// <returns>All the meshes matching the given name.</returns>
        public List<MeshData> GetMeshWithName(string name)
        {
            List<MeshData> result = new List<MeshData>();
            foreach (MeshData mesh in Meshes)
            {
                if (mesh.Name == name) { result.Add(mesh); }
            }
            return result;
        }

        /// <summary>
        /// Find the mesh with the given index.
        /// </summary>
        /// <param name="index">Index of the mesh</param>
        /// <returns>Mesh at the index</returns>
        public MeshData GetMesh(int index)
        {
            return Meshes[index];
        }
    }
}
