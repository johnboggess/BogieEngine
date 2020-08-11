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
            /*NormalMaterial normalMaterial = new NormalMaterial();
            normalMaterial.DiffuseTexture = ((Game)Game).Brick2Tex;
            normalMaterial.NormalTexture = ((Game)Game).Brick2Norm;
            normalMaterial.SpecularTexture = ((Game)Game).BlankSpecular;

            model = Model.CreateModel("Resources/Models/Cube.obj", Game.ContentManager, ((Game)Game).NormalShader, new TangetSpaceVertexDefinition());

            model.GetMesh(0).Material = normalMaterial;

            ForceAddComponent(model);*/
        }
    }
}
