using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BogieEngineCore.Nodes
{
    public class Node
    {
        public Node Parent;
        public List<Node> Childern { get { return _Childern; } }

        internal List<Node> _Childern = new List<Node>();

        public void AddNode(Node node)
        {
            if(node.Parent != null)
            {
                node.Parent._Childern.Remove(node);
            }
            node.Parent = this;
            this._Childern.Add(node);
        }

        public virtual void Process(float deltaT) { }
        public virtual void Draw(float deltaT) { }

        internal void _Process(float deltaT)
        {
            Process(deltaT);
            foreach (Node node in _Childern)
                node._Process(deltaT);
        }

        internal void _Draw(float deltaT)
        {
            Draw(deltaT);
            foreach (Node node in _Childern)
                node._Draw(deltaT);
        }

    }
}
