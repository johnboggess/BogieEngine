using BogieEngineCore.Texturing;
using OpenTK;
using System.Collections.Generic;
namespace BogieEngineCore.Modelling
{
    public class Model
    {
        /// <summary>
        /// The meshes that make up the model.
        /// </summary>
        public List<MeshInstance> Meshes = new List<MeshInstance>();

        public Model(List<MeshInstance> meshes)
        {
            Meshes = meshes;
        }

        /// <summary>
        /// Create a new model that is a clone of the given model.
        /// </summary>
        /// <param name="model">The model to clone</param>
        public Model(Model model)
        {
            foreach (MeshInstance mesh in model.Meshes)
            {
                MeshInstance newMesh = new MeshInstance(mesh._MeshData);
                newMesh.Shader = mesh.Shader;
                newMesh.Textures = new List<Texture>(mesh.Textures);
                Meshes.Add(newMesh);
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

        /// <summary>
        /// Sets the given shader to all the meshes for this model.
        /// </summary>
        /// <param name="shader">The shader to assign to each mesh.</param>
        public void SetShader(Shading.Shader shader)
        {
            foreach (MeshInstance meshData in Meshes)
                meshData.Shader = shader;
        }

        /// <summary>
        /// Draw the model with the given transform.
        /// </summary>
        /// <param name="Transform">How to transform the model.</param>
        public void Draw(Matrix4 Transform)
        {
            foreach (MeshInstance mesh in Meshes)
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
