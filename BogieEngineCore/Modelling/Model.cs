using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using OpenTK;
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
                MeshData.Add(newMeshData);
            }
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
