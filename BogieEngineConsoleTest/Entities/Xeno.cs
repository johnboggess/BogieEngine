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
            //mesh 0: body
            //mesh 1: head
            /*Model = Model.CreateModel("Resources/Models/xeno-raven/source/XenoRaven.fbx", Game.ContentManager, ((Game)Game).NormalShader, new TangetSpaceVertexDefinition());
            this.ForceAddComponent(Model);

            Texture body = ((PhongMaterial)Model.GetMesh(0).Material).DiffuseTexture;
            Texture head = ((PhongMaterial)Model.GetMesh(1).Material).DiffuseTexture;

            Model.GetMesh(0).Material = new NormalMaterial();
            Model.GetMesh(1).Material = new NormalMaterial();


            ((NormalMaterial)Model.GetMesh(0).Material).DiffuseTexture = body;
            ((NormalMaterial)Model.GetMesh(1).Material).DiffuseTexture = head;

            ((NormalMaterial)Model.GetMesh(0).Material).SpecularTexture = ((Game)Game).XenoBodyS;
            ((NormalMaterial)Model.GetMesh(1).Material).SpecularTexture = ((Game)Game).XenoHeadS;

            ((NormalMaterial)Model.GetMesh(0).Material).NormalTexture = ((Game)Game).XenoBodyN;
            ((NormalMaterial)Model.GetMesh(1).Material).NormalTexture = ((Game)Game).XenoHeadN;

            ((NormalMaterial)Model.GetMesh(0).Material).Shininess = 32f;//1.8f;
            ((NormalMaterial)Model.GetMesh(1).Material).Shininess = 32f; //1.8f;*/
        }
    }
}
