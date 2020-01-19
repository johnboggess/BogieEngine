using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using BogieEngineCore.Modelling;
namespace BogieEngineCore.Nodes
{
    class ModelNode : Node3D
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
    }
}
