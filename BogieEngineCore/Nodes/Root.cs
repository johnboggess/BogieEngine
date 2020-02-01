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
            processAddRemoveQueue(_ProcessChildern, nodesQueuedToAdd_ProcessChildern, nodesQueuedToRemove_ProcessChildern);

            foreach (Node node in _ProcessChildern)
                node._Draw(deltaT, parentWorldTransform);

            processAddRemoveQueue(_ProcessChildern, nodesQueuedToAdd_ProcessChildern, nodesQueuedToRemove_ProcessChildern);
        }

        public override void Process(float deltaT, Transform parentWorldTransform)
        {
            processAddRemoveQueue(_DrawChildern, nodesQueuedToAdd_DrawChildern, nodesQueuedToRemove_DrawChildern);

            foreach (Node node in _ProcessChildern)
                node._Process(deltaT, parentWorldTransform);

            processAddRemoveQueue(_DrawChildern, nodesQueuedToAdd_DrawChildern, nodesQueuedToRemove_DrawChildern);
        }
    }
}
