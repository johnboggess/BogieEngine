using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace BogieEngineCore.Nodes
{
    public class Node
    {
        public BaseGame Game;
        public Node Parent;
        public Transform LocalTransform = new Transform();
        public Transform WorldTransform { get { return _worldTransform; } }

        internal List<Node> _ProcessChildern = new List<Node>();
        internal List<Node> _DrawChildern = new List<Node>();

        protected internal List<Node> nodesQueuedToAdd_ProcessChildern = new List<Node>();
        protected internal List<Node> nodesQueuedToRemove_ProcessChildern = new List<Node>();

        protected internal List<Node> nodesQueuedToAdd_DrawChildern = new List<Node>();
        protected internal List<Node> nodesQueuedToRemove_DrawChildern = new List<Node>();

        Transform _worldTransform = new Transform();
        private Mutex queueMutex = new Mutex();

        public Node(BaseGame game)
        {
            this.Game = game;
        }

        public void AddNode(Node node)
        {
            if(node.Parent != null)
            {
                node.Parent.RemoveNode(node);
            }
            node.Parent = this;

            queueMutex.WaitOne();
            nodesQueuedToAdd_ProcessChildern.Add(node);
            nodesQueuedToAdd_DrawChildern.Add(node);
            queueMutex.WaitOne();
        }

        public void RemoveNode(Node node)
        {
            node.Parent = null;

            queueMutex.WaitOne();
            nodesQueuedToRemove_ProcessChildern.Add(node);
            nodesQueuedToRemove_DrawChildern.Add(node);
            queueMutex.ReleaseMutex();
        }

        public virtual void Process(float deltaT, Transform parentWorldTransform) { }
        public virtual void Draw(float deltaT, Transform parentWorldTransform) { }

        internal virtual void _Process(float deltaT, Transform parentWorldTransform)
        {
            processAddRemoveQueue(_ProcessChildern, nodesQueuedToAdd_ProcessChildern, nodesQueuedToRemove_ProcessChildern);

            _worldTransform.FromMatrix4(LocalTransform.GetMatrix4() * parentWorldTransform.GetMatrix4());
            Process(deltaT, parentWorldTransform);
            foreach (Node node in _ProcessChildern)
                node._Process(deltaT, _worldTransform);

            processAddRemoveQueue(_ProcessChildern, nodesQueuedToAdd_ProcessChildern, nodesQueuedToRemove_ProcessChildern);
        }

        internal virtual void _Draw(float deltaT, Transform parentWorldTransform)
        {
            processAddRemoveQueue(_DrawChildern, nodesQueuedToAdd_DrawChildern, nodesQueuedToRemove_DrawChildern);

            Draw(deltaT, parentWorldTransform);
            foreach (Node node in _DrawChildern)
                node._Draw(deltaT, _worldTransform);

            processAddRemoveQueue(_DrawChildern, nodesQueuedToAdd_DrawChildern, nodesQueuedToRemove_DrawChildern);
        }

        protected internal void processAddRemoveQueue(IList<Node> nodeList, IList<Node> addQueue, IList<Node> removeQueue)
        {
            queueMutex.WaitOne();
            foreach (Node node in addQueue)
            {
                if (!nodeList.Contains(node))
                {
                    nodeList.Add(node);
                }
            }
            addQueue.Clear();

            foreach (Node node in removeQueue)
            {
                nodeList.Remove(node);
            }
            removeQueue.Clear();
            queueMutex.ReleaseMutex();
        }

    }
}
