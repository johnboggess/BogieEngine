using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using OpenTK;
namespace BogieEngineCore.Models
{
    class Model
    {
        public Matrix4 Transform = Matrix4.Identity;
        public List<Mesh> Meshes = new List<Mesh>();

        public Model(List<Mesh> meshes)
        {
            Meshes = meshes;
        }

        public void Draw()
        {
            foreach(Mesh mesh in Meshes)
            {
                mesh.Shader.Use(Transform);
                mesh.Draw();
            }
        }
    }
}
