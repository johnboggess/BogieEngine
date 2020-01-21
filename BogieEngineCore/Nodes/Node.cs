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
        public Transform Transform = new Transform();
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

        public virtual void Process(float deltaT, Transform parentWorldTransform) { }
        public virtual void Draw(float deltaT, Transform parentWorldTransform) { }

        internal void _Process(float deltaT, Transform parentWorldTransform)
        {
            Process(deltaT, parentWorldTransform);
            Transform t = new Transform();
            t.FromMatrix4(Transform.GetMatrix4() * parentWorldTransform.GetMatrix4());
            foreach (Node node in _Childern)
                node._Process(deltaT, t);
        }

        internal void _Draw(float deltaT, Transform parentWorldTransform)
        {
            Draw(deltaT, parentWorldTransform);
            Transform t = new Transform();
            t.FromMatrix4(Transform.GetMatrix4() * parentWorldTransform.GetMatrix4());
            foreach (Node node in _Childern)
                node._Draw(deltaT, t);
        }

    }
}
