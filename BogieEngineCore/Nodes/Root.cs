using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BogieEngineCore.Nodes
{
    public class Root : Node
    {
        public override void Draw(float deltaT)
        {
            foreach (Node node in _Childern)
                node._Draw(deltaT);
        }

        public override void Process(float deltaT)
        {
            foreach (Node node in _Childern)
                node._Process(deltaT);
        }
    }
}
