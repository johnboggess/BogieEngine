using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BogieEngineCore.Nodes
{
    public class Root : Node
    {
        public Root(BaseGame game) : base(game) { }

        public override void Draw(float deltaT, Transform parentWorldTransform)
        {
            processAddRemoveQueue(_ProcessChildern, _NodesQueuedToAdd_ProcessChildern, _NodesQueuedToRemove_ProcessChildern);

            foreach (Node node in _ProcessChildern)
                node._Draw(deltaT, parentWorldTransform);

            processAddRemoveQueue(_ProcessChildern, _NodesQueuedToAdd_ProcessChildern, _NodesQueuedToRemove_ProcessChildern);
        }

        public override void Process(float deltaT, Transform parentWorldTransform)
        {
            processAddRemoveQueue(_DrawChildern, _NodesQueuedToAdd_DrawChildern, _NodesQueuedToRemove_DrawChildern);

            foreach (Node node in _ProcessChildern)
                node._Process(deltaT, parentWorldTransform);

            processAddRemoveQueue(_DrawChildern, _NodesQueuedToAdd_DrawChildern, _NodesQueuedToRemove_DrawChildern);
        }
    }
}
