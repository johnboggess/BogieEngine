using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using BogieEngineCore.Modelling;

using OpenTK;
namespace BogieEngineCore.Nodes
{
    public class ModelNode : Node
    {
        public Model Model;

        public ModelNode(Model model)
        {
            Model = model;
        }

        public override void Draw(float deltaT, Transform parentWorldTransform)
        {
            Model.Draw(LocalTransform.GetMatrix4() * parentWorldTransform.GetMatrix4());
        }

        public List<MeshData> GetMeshWithName(string name)
        {
            return Model.GetMeshWithName(name);
        }
    }
}
