using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using OpenTK;

using BepuPhysics;
using BepuPhysics.Collidables;

namespace BogieEngineCore.Nodes
{
    public class RigidBody : Node
    {
        public BodyDescription BodyDescription;
        private int bodyHandle;

        public RigidBody(BaseGame game) : base(game)
        {
        }

        public void Init(Transform parentWorldTransform)
        {
            Matrix4 worldTransform = LocalTransform.GetMatrix4() * parentWorldTransform.GetMatrix4();

            Box box = new Box(LocalTransform.Scale.X, LocalTransform.Scale.Y, LocalTransform.Scale.Z);
            BodyInertia bodyInertia;
            box.ComputeInertia(1, out bodyInertia);
            BodyDescription = BodyDescription.CreateDynamic(
                new System.Numerics.Vector3(worldTransform.M41, worldTransform.M42, worldTransform.M43),
                bodyInertia,
                new CollidableDescription(Game.PhysicsSimulation.Shapes.Add(box), 0.1f),
                new BodyActivityDescription(0.01f));
            BodyDescription.Pose.Orientation = Utilities.ConvertQuaternionType(LocalTransform.Quaternion);
            bodyHandle = Game.PhysicsSimulation.Bodies.Add(BodyDescription);
        }

        internal override void _Process(float deltaT, Transform parentWorldTransform)
        {
            Game.PhysicsSimulation.Bodies.GetDescription(bodyHandle, out BodyDescription);
            localTransformMatchRigidBody(parentWorldTransform);
            base._Process(deltaT, parentWorldTransform);
        }

        private void localTransformMatchRigidBody(Transform parentWorldTransform)
        {
            Game.PhysicsSimulation.Bodies.GetDescription(bodyHandle, out BodyDescription);

            Matrix4 worldToLocalTransform = parentWorldTransform.GetMatrix4();
            worldToLocalTransform.Invert();
            LocalTransform.Position = Utilities.ConvertVector3Type(BodyDescription.Pose.Position);
            LocalTransform.Quaternion = Utilities.ConvertQuaternionType(BodyDescription.Pose.Orientation);
        }

    }
}
