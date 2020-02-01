using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using BepuPhysics;
using BepuPhysics.Collidables;

using OpenTK;
namespace BogieEngineCore.Nodes
{
    public class StaticBody : Node
    {
        private int bodyHandle;

        public StaticBody(BaseGame game) : base(game)
        {

        }

        public void CreateBox(Transform parentWorldTransform)
        {
            Matrix4 worldTransform = LocalTransform.GetMatrix4() * parentWorldTransform.GetMatrix4();

            Box box = new Box(LocalTransform.Scale.X, LocalTransform.Scale.Y, LocalTransform.Scale.Z);

            StaticDescription staticDescription = new StaticDescription(
                new System.Numerics.Vector3(worldTransform.M41, worldTransform.M42, worldTransform.M43),
                new CollidableDescription(Game.PhysicsSimulation.Shapes.Add(box), 0.1f));

            bodyHandle = Game.PhysicsSimulation.Statics.Add(staticDescription);
        }

        internal override void _Process(float deltaT, Transform parentWorldTransform)
        {
            base._Process(deltaT, parentWorldTransform);
        }
    }
}
