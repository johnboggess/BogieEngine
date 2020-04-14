using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace BogieEngineCore.Nodes
{
    /// <summary>
    /// A game object. All game objects must inherit from this class.
    /// </summary>
    public class Node
    {
        public BaseGame Game;
        public Node Parent;

        /// <summary>
        /// Defines the local vector space of the object and its pose in that vector space.
        /// </summary>
        public Transform LocalTransform = new Transform();
        /// <summary>
        /// The transform to get the object back into the world vector space. Any changes to the local transform will be reflected here after the current tick.
        /// </summary>
        public Transform WorldTransform { get { return _worldTransform; } }

        /// <summary>
        /// Which child node of this node can be procesed each tick.
        /// </summary>
        internal List<Node> _ProcessChildern = new List<Node>();
        /// <summary>
        /// Which child node of this node can be drawn each frame.
        /// </summary>
        internal List<Node> _DrawChildern = new List<Node>();

        protected internal List<Node> _NodesQueuedToAdd_ProcessChildern = new List<Node>();
        protected internal List<Node> _NodesQueuedToRemove_ProcessChildern = new List<Node>();

        protected internal List<Node> _NodesQueuedToAdd_DrawChildern = new List<Node>();
        protected internal List<Node> _NodesQueuedToRemove_DrawChildern = new List<Node>();

        /// <summary>
        /// The transform to get the object back into the world vector space. Any changes to the local transform will be reflected here after the current tick.
        /// </summary
        Transform _worldTransform = new Transform();
        private Mutex _queueMutex = new Mutex();

        public Node(BaseGame game)
        {
            this.Game = game;
        }

        /// <summary>
        /// Adds the given node as a child to this node.
        /// </summary>
        /// <param name="node">The node to add.</param>
        public void AddNode(Node node)
        {
            if(node.Parent != null)
            {
                node.Parent.RemoveNode(node);
            }
            node.Parent = this;

            _queueMutex.WaitOne();
            _NodesQueuedToAdd_ProcessChildern.Add(node);
            _NodesQueuedToAdd_DrawChildern.Add(node);
            _queueMutex.WaitOne();
        }


        /// <summary>
        /// Removes the given child node.
        /// </summary>
        /// <param name="node">The node to remove.</param>
        public void RemoveNode(Node node)
        {
            node.Parent = null;

            _queueMutex.WaitOne();
            _NodesQueuedToRemove_ProcessChildern.Add(node);
            _NodesQueuedToRemove_DrawChildern.Add(node);
            _queueMutex.ReleaseMutex();
        }

        /// <summary>
        /// Place code here to execute each tick.
        /// </summary>
        /// <param name="deltaT">Time since the last tick.</param>
        /// <param name="parentWorldTransform">The transform of the parent node.</param>
        public virtual void Process(float deltaT, Transform parentWorldTransform) { }

        /// <summary>
        /// Place code here to draw the node each frame.
        /// </summary>
        /// <param name="deltaT">Time since the last frame.</param>
        /// <param name="parentWorldTransform">The transform of the parent node.</param>
        public virtual void Draw(float deltaT, Transform parentWorldTransform) { }

        /// <summary>
        /// Handles pre and post procesing of the node.
        /// </summary>
        /// <param name="deltaT">Time since the last tick.</param>
        /// <param name="parentWorldTransform">The transform of the parent node.</param>
        internal virtual void _Process(float deltaT, Transform parentWorldTransform)
        {
            processAddRemoveQueue(_ProcessChildern, _NodesQueuedToAdd_ProcessChildern, _NodesQueuedToRemove_ProcessChildern);

            _worldTransform.FromMatrix4(LocalTransform.GetMatrix4() * parentWorldTransform.GetMatrix4());
            Process(deltaT, parentWorldTransform);
            foreach (Node node in _ProcessChildern)
                node._Process(deltaT, _worldTransform);

            processAddRemoveQueue(_ProcessChildern, _NodesQueuedToAdd_ProcessChildern, _NodesQueuedToRemove_ProcessChildern);
        }


        /// <summary>
        /// Handles pre and post drawing of the node.
        /// </summary>
        /// <param name="deltaT">Time since the last frame.</param>
        /// <param name="parentWorldTransform">The transform of the parent node.</param>
        internal virtual void _Draw(float deltaT, Transform parentWorldTransform)
        {
            processAddRemoveQueue(_DrawChildern, _NodesQueuedToAdd_DrawChildern, _NodesQueuedToRemove_DrawChildern);

            Draw(deltaT, parentWorldTransform);
            foreach (Node node in _DrawChildern)
                node._Draw(deltaT, _worldTransform);

            processAddRemoveQueue(_DrawChildern, _NodesQueuedToAdd_DrawChildern, _NodesQueuedToRemove_DrawChildern);
        }

        protected internal void processAddRemoveQueue(IList<Node> nodeList, IList<Node> addQueue, IList<Node> removeQueue)
        {
            _queueMutex.WaitOne();
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
            _queueMutex.ReleaseMutex();
        }

    }
}
