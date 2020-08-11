using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using BogieEngineCore.Components;
using BogieEngineCore.Entities;
using BogieEngineCore.Materials;
using BogieEngineCore.Vertices;

namespace BogieEngineConsoleTest.Entities
{
    class NormalTest : Entity
    {
        Model model;
        public NormalTest(Entity parent, Game game) : base(parent, game)
        {

        }

        public override void EntitySetup()
        {
            model = new Model(((Game)Game).NormalCubeInstance);
            ForceAddComponent(model);
        }
    }
}
