using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using BogieEngineConsoleTest;
using BogieEngineConsoleTest.Components;

using BogieEngineCore;
using BogieEngineCore.Entities;
using BogieEngineCore.Components;
namespace BogieEngineConsoleTest.Entities
{
    class Suzanne : Entity
    {
        Model model;
        RigidMesh _rigidMesh;
        GravityScript _gravityScript;

        public Suzanne(Entity entity, BaseGame game) : base(entity, game)
        {

        }

        public override void EntitySetup()
        {
            model = new Model(((Game)Game).SuzanneModel.CreateInstance());
            ForceAddComponent(model);

            _rigidMesh = new RigidMesh(model.ModelInstance.Meshes[0].MeshData, GlobalTransform, false);
            ForceAddComponent(_rigidMesh);

            _gravityScript = new GravityScript(nameof(RigidMesh));
            _gravityScript.Gravity = BogieEngineConsoleTest.Game.Gravity;
            ForceAddComponent(_gravityScript);
        }
    }
}
