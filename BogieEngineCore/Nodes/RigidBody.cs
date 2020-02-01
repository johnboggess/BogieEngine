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
            bodyHandle = Game.PhysicsSimulation.Bodies.Add(BodyDescription);
        }

        public override void Process(float deltaT, Transform parentWorldTransform)
        {
            LocalTransformMatchRigidBody(parentWorldTransform);
        }

        public void RigidBodyMatchLocalTransform(Transform parentWorldTransform)
        {
            /*Matrix4 worldTransform = LocalTransform.GetMatrix4() * parentWorldTransform.GetMatrix4();
            JVector pos = new JVector(worldTransform.M41, worldTransform.M42, worldTransform.M43);
            JitterRigidBody.Position = pos;

            JMatrix jMatrix = new JMatrix();

            jMatrix.M11 = worldTransform.M11;
            jMatrix.M12 = worldTransform.M12;
            jMatrix.M13 = worldTransform.M13;

            jMatrix.M21 = worldTransform.M21;
            jMatrix.M22 = worldTransform.M22;
            jMatrix.M23 = worldTransform.M23;

            jMatrix.M31 = worldTransform.M31;
            jMatrix.M32 = worldTransform.M32;
            jMatrix.M33 = worldTransform.M33;

            JitterRigidBody.Orientation = jMatrix;*/
        }

        public void LocalTransformMatchRigidBody(Transform parentWorldTransform)
        {
            Game.PhysicsSimulation.Bodies.GetDescription(bodyHandle, out BodyDescription);

            Matrix4 worldToLocalTransform = parentWorldTransform.GetMatrix4();
            worldToLocalTransform.Invert();
            //LocalTransform.Position = Utilities.SystemVector3ToTKVector3(BodyDescription.Pose.Position);
            /*Matrix4 worldToLocalTransform = parentWorldTransform.GetMatrix4();
            worldToLocalTransform.Invert();
            Vector3 scale = LocalTransform.Scale;
            LocalTransform.Position = new Vector3((worldToLocalTransform * new Vector4(JitterRigidBody.Position.X, JitterRigidBody.Position.Y, JitterRigidBody.Position.Z, 0)));
            LocalTransform.XAxis = new Vector3(JitterRigidBody.Orientation.M11, JitterRigidBody.Orientation.M12, JitterRigidBody.Orientation.M13);
            LocalTransform.YAxis = new Vector3(JitterRigidBody.Orientation.M21, JitterRigidBody.Orientation.M22, JitterRigidBody.Orientation.M23);
            LocalTransform.ZAxis = new Vector3(JitterRigidBody.Orientation.M31, JitterRigidBody.Orientation.M32, JitterRigidBody.Orientation.M33);

            LocalTransform.Scale = scale;*/
        }

        internal override void _Process(float deltaT, Transform parentWorldTransform)
        {
            Game.PhysicsSimulation.Bodies.GetDescription(bodyHandle, out BodyDescription);
            base._Process(deltaT, parentWorldTransform);
        }
    }
}
