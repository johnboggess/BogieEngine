using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BogieEngineCore.Nodes
{
    public class Root : Node
    {
        public override void Draw()
        {
            foreach (Node node in _Childern)
                node._Draw();
        }

        public override void Process()
        {
            foreach (Node node in _Childern)
                node._Process();
        }
    }
}
