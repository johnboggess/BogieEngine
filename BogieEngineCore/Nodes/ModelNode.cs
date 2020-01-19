using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using BogieEngineCore.Modelling;
namespace BogieEngineCore.Nodes
{
    public class ModelNode : Node3D
    {
        public Model Model;

        public ModelNode(Model model)
        {
            Model = model;
        }

        public override void Draw()
        {
            Model.Draw(Transform.GetMatrix4());
        }

        public List<MeshData> GetMeshWithName(string name)
        {
            return Model.GetMeshWithName(name);
        }
    }
}
