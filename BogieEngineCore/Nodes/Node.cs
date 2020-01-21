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
        public Transform LocalTransform = new Transform();
        public Transform WorldTransform { get { return _worldTransform; } }
        public List<Node> Childern { get { return _Childern; } }

        internal List<Node> _Childern = new List<Node>();

        Transform _worldTransform = new Transform();

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
            _worldTransform.FromMatrix4(LocalTransform.GetMatrix4() * parentWorldTransform.GetMatrix4());
            Process(deltaT, parentWorldTransform);
            foreach (Node node in _Childern)
                node._Process(deltaT, _worldTransform);
        }

        internal void _Draw(float deltaT, Transform parentWorldTransform)
        {
            Draw(deltaT, parentWorldTransform);
            foreach (Node node in _Childern)
                node._Draw(deltaT, _worldTransform);
        }

    }
}
