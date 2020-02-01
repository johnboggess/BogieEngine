using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using BogieEngineCore.Nodes;
using BogieEngineCore.Modelling;
using BogieEngineCore;

namespace BogieEngineConsoleTest
{
    class Block : RigidBody
    {
        public static Model Model;

        public override void Draw(float deltaT, Transform parentWorldTransform)
        {
            Model.Draw(LocalTransform.GetMatrix4() * parentWorldTransform.GetMatrix4());
        }
    }
}
