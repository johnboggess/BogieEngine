using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BogieEngineCore.Nodes
{
    public class Root : Node
    {
        public override void Draw(float deltaT, Transform parentWorldTransform)
        {
            foreach (Node node in _Childern)
                node._Draw(deltaT, parentWorldTransform);
        }

        public override void Process(float deltaT, Transform parentWorldTransform)
        {
            foreach (Node node in _Childern)
                node._Process(deltaT, parentWorldTransform);
        }
    }
}
