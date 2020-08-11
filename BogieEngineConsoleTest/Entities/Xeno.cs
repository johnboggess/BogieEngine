using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using BogieEngineCore;
using BogieEngineCore.Entities;
using BogieEngineCore.Components;
using BogieEngineCore.Materials;
using BogieEngineCore.Texturing;
using BogieEngineCore.Vertices;

namespace BogieEngineConsoleTest.Entities
{
    class Xeno : Entity
    {
        public Model Model;

        public Xeno(Entity parent, Game game) : base(parent, game)
        {
        }

        public override void EntitySetup()
        {
            Model = new Model(((Game)(Game)).XenoInstance);
            ForceAddComponent(Model);
        }
    }
}
