using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using OpenTK;

using Jitter;
using Jitter.Collision.Shapes;
using Jitter.LinearMath;
namespace BogieEngineCore.Nodes
{
    public class RigidBody : Node
    {
        public Jitter.Dynamics.RigidBody JitterRigidBody;
        public RigidBody()
        {
            JitterRigidBody = new Jitter.Dynamics.RigidBody(new BoxShape(1, 1, 1));
        }

        public override void Process(float deltaT, Transform parentWorldTransform)
        {
            LocalTransformMatchRigidBody(parentWorldTransform);
        }

        public void RigidBodyMatchLocalTransform(Transform parentWorldTransform)
        {
            Matrix4 worldTransform = LocalTransform.GetMatrix4() * parentWorldTransform.GetMatrix4();
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

            JitterRigidBody.Orientation = jMatrix;
        }

        public void LocalTransformMatchRigidBody(Transform parentWorldTransform)
        {
            Matrix4 worldToLocalTransform = parentWorldTransform.GetMatrix4();
            worldToLocalTransform.Invert();
            Vector3 scale = LocalTransform.Scale;
            LocalTransform.Position = new Vector3((worldToLocalTransform * new Vector4(JitterRigidBody.Position.X, JitterRigidBody.Position.Y, JitterRigidBody.Position.Z, 0)));
            LocalTransform.XAxis = new Vector3(JitterRigidBody.Orientation.M11, JitterRigidBody.Orientation.M12, JitterRigidBody.Orientation.M13);
            LocalTransform.YAxis = new Vector3(JitterRigidBody.Orientation.M21, JitterRigidBody.Orientation.M22, JitterRigidBody.Orientation.M23);
            LocalTransform.ZAxis = new Vector3(JitterRigidBody.Orientation.M31, JitterRigidBody.Orientation.M32, JitterRigidBody.Orientation.M33);

            LocalTransform.Scale = scale;
        }
    }
}
