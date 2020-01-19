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
        public Matrix4 Transform = Matrix4.Identity;
        public List<MeshData> MeshData = new List<MeshData>();

        public Model(List<MeshData> meshData)
        {
            MeshData = meshData;
        }

        public Model(Model model, Game game)
        {
            foreach(MeshData meshData in model.MeshData)
            {
                MeshData newMeshData = new MeshData(meshData._Mesh);
                newMeshData.Shader = meshData.Shader;
                newMeshData.Textures = meshData.Textures;
                MeshData.Add(newMeshData);
            }
        }

        public List<MeshData> GetMeshWithName(string name)
        {
            List<MeshData> result = new List<MeshData>();
            foreach(MeshData meshData in MeshData)
            {
                if(meshData.Name == name) { result.Add(meshData); }
            }
            return result;
        }

        public void Draw()
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
