using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using BogieEngineCore;
using BogieEngineCore.Nodes;
using BogieEngineCore.Modelling;
namespace BogieEngineConsoleTest
{
    class StaticBlock : StaticBody
    {
        public static Model Model;

        public StaticBlock(BaseGame game) : base(game) { }

        public override void Draw(float deltaT, Transform parentWorldTransform)
        {
            Model.Draw(LocalTransform.GetMatrix4() * parentWorldTransform.GetMatrix4());
        }
    }
}
