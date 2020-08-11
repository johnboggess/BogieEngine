using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using OpenTK;
namespace BogieEngineCore.Modelling
{
    public class ModelInstance
    {
        /// <summary>
        /// The meshes that make up the model.
        /// </summary>
        public List<MeshInstance> Meshes = new List<MeshInstance>();

        public ModelInstance(List<MeshInstance> meshes)
        {
            Meshes = meshes;
        }

        public void Draw(Matrix4 matrix)
        {
            foreach (MeshInstance meshInstance in Meshes)
            {
                meshInstance.Shader.Use(matrix);
                meshInstance.Draw();
            }
        }

        /// <summary>
        /// Find meshes with the given name.
        /// </summary>
        /// <param name="name">The name of the meshes to search for.</param>
        /// <returns>All the meshes matching the given name.</returns>
        public List<MeshInstance> GetMeshWithName(string name)
        {
            List<MeshInstance> result = new List<MeshInstance>();
            foreach (MeshInstance mesh in Meshes)
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
        public MeshInstance GetMesh(int index)
        {
            return Meshes[index];
        }
    }
}
