using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using OpenTK;

using BogieEngineCore.Texturing;
namespace BogieEngineCore.Modelling
{
    public class Model
    {
        /// <summary>
        /// The meshes that make up the model.
        /// </summary>
        public List<MeshData> MeshData = new List<MeshData>();

        public Model(List<MeshData> meshData)
        {
            MeshData = meshData;
        }

        /// <summary>
        /// Create a new model that is a clone of the given model.
        /// </summary>
        /// <param name="model">The model to clone</param>
        public Model(Model model)
        {
            foreach(MeshData meshData in model.MeshData)
            {
                MeshData newMeshData = new MeshData(meshData._Mesh);
                newMeshData.Shader = meshData.Shader;
                newMeshData.Textures = new List<Texture>(meshData.Textures);
                MeshData.Add(newMeshData);
            }
        }

        /// <summary>
        /// Find meshes with the given name.
        /// </summary>
        /// <param name="name">The name of the meshes to search for.</param>
        /// <returns>All the meshes matching the given name.</returns>
        public List<MeshData> GetMeshWithName(string name)
        {
            List<MeshData> result = new List<MeshData>();
            foreach(MeshData meshData in MeshData)
            {
                if(meshData.Name == name) { result.Add(meshData); }
            }
            return result;
        }
        
        /// <summary>
        /// Sets the given shader to all the meshes for this model.
        /// </summary>
        /// <param name="shader">The shader to assign to each mesh.</param>
        public void SetShader(Shading.Shader shader)
        {
            foreach (MeshData meshData in MeshData)
                meshData.Shader = shader;
        }

        /// <summary>
        /// Draw the model with the given transform.
        /// </summary>
        /// <param name="Transform">How to transform the model.</param>
        public void Draw(Matrix4 Transform)
        {
            foreach(MeshData mesh in MeshData)
            {
                if (mesh.Visible)
                {
                    mesh.Shader.Use(Transform);
                    mesh.Draw();
                }
            }
        }
    }
}
