using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using BogieEngineCore;
using BogieEngineCore.Entities;
using BogieEngineCore.Components;
using BogieEngineCore.Vertices;

namespace BogieEngineConsoleTest.Entities
{
    class Samus : Entity
    {
        public Model Model;

        public Samus(Entity parent, Game game) : base(parent, game)
        {
        }

        public override void EntitySetup()
        {
            Model = new Model(((Game)Game).SamusInstance);
            this.ForceAddComponent(Model);
        }
    }
}
