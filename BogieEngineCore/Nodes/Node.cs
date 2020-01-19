using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BogieEngineCore.Nodes
{
    public class Node
    {
        public List<Node> Childern = new List<Node>();

        internal void _Process()
        {
            Process();
        }

        internal void _Draw()
        {
            Draw();
        }

        public virtual void Process() { }
        public virtual void Draw() { }
    }
}
