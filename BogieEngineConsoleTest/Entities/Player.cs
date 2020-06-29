using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BogieEngineConsoleTest.Components;
using BogieEngineCore;
using BogieEngineCore.Components;
using BogieEngineCore.Entities;
using BogieEngineCore.Physics.Shapes;
namespace BogieEngineConsoleTest.Entities
{
    class Player : Entity
    {
        public Camera Camera;
        public RigidBox RigidBox;
        public PlayerMovmentScript PlayerMovmentScript;
        public ShootScript ShootScript;

        public Player(Entity parent, BaseGame game, Camera camera) : base(parent, game)
        {
            Camera = camera;
        }

        public override void EntitySetup()
        {
            RigidBox = new RigidBox(LocalTransform.Position.X, LocalTransform.Position.Y, LocalTransform.Position.Z, LocalTransform.Scale.X, LocalTransform.Scale.Y, LocalTransform.Scale.Z, true);
            ForceAddComponent(RigidBox);

            PlayerMovmentScript = new PlayerMovmentScript(Camera, RigidBox);
            this.ForceAddComponent(PlayerMovmentScript);

            ShootScript = new ShootScript(Camera);
            ShootScript.Name = nameof(ShootScript);
            ForceAddComponent(ShootScript);
        }
    }
}
