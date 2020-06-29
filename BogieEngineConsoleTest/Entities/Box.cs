using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;

using BogieEngineCore;
using BogieEngineCore.Entities;
using BogieEngineCore.Components;
using BogieEngineConsoleTest.Components;

namespace BogieEngineConsoleTest.Entities
{
    class Box : Entity
    {
        public RigidBox RigidBox;

        private bool _reportContacts;

        public Box(Entity parent, bool reportContacts, OpenTK.Vector3 position, OpenTK.Vector3 scale, BaseGame game) : base(parent, game)
        {
            LocalTransform.Position = position;
            LocalTransform.Scale = scale;
            _reportContacts = reportContacts;
        }

        public override void EntitySetup()
        {
            RigidBox = new RigidBox(LocalTransform.Position.X, LocalTransform.Position.Y, LocalTransform.Position.Z, LocalTransform.Scale.X, LocalTransform.Scale.Y, LocalTransform.Scale.Z, _reportContacts);
            RigidBox.Name = nameof(RigidBox);
            ForceAddComponent(RigidBox);
            GravityScript gravityScript = new GravityScript();
            ForceAddComponent(gravityScript);

            Model blockModel = Model.CreateModel("Resources/Models/Cube.obj", ((Game)BaseGame.GlobalGame).ContentManager, ((Game)BaseGame.GlobalGame).DefaultShader);
            blockModel.GetMesh(0).Textures.Add(((Game)BaseGame.GlobalGame).CubeTex);
            ForceAddComponent(blockModel);
        }

    }
}
