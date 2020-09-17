using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

using BogieEngineCore.Physics;

using BepuPhysics;
using BepuPhysics.Collidables;

namespace BogieEngineCore.Components
{
    public class RigidBox : RigidBody
    {//todo: add resize method
        public RigidBox(float x, float y, float z, float widthX, float heightY, float lengthZ, bool recordContacts) : base(x,y,z,widthX,heightY,lengthZ, recordContacts, nameof(RigidBox), null)
        {
        }
        public RigidBox(Vector3 position, Vector3 scale, bool recordContacts) : base(position, scale, recordContacts, nameof(RigidBox), null)
        {
        }
        public RigidBox(Transform transform, bool recordContacts) : base(transform, recordContacts, nameof(RigidBox), null)
        {
        }

        protected override void _SetupCollider(float x, float y, float z, float scaleX, float scaleY, float scaleZ, object otherData)
        {
            Box box = new Box(scaleX, scaleY, scaleZ);
            BodyInertia bodyInertia;
            box.ComputeInertia(1, out bodyInertia);
            BodyDescription bodyDescription = BodyDescription.CreateDynamic(
                new System.Numerics.Vector3(x, y, z),
                bodyInertia,
                new CollidableDescription(BaseGame.GlobalGame._PhysicsSimulation.Shapes.Add(box), 0.1f),
                new BodyActivityDescription(0.01f));

            //todo: get this to work. or remove
            //bodyDescription.Pose.Orientation = Utilities.ConvertQuaternionType(_Entity.LocalTransform.Quaternion);
            
            _BodyHandle = BaseGame.GlobalGame._PhysicsSimulation.Bodies.Add(bodyDescription);
            _BodyReference = BaseGame.GlobalGame._PhysicsSimulation.Bodies.GetBodyReference(_BodyHandle);
            _IsBody = true;
        }
    }
}
