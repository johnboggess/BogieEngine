using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using BogieEngineCore.Shading;
using BogieEngineCore.Entities;
using BogieEngineCore.Physics;

using BepuPhysics;
using BepuPhysics.Collidables;
using BepuPhysics.CollisionDetection;
using BepuPhysics.Constraints;
using BepuPhysics.Trees;

using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace BogieEngineCore.Components
{
    public class RigidBody : Component
    {
        public BodyReference BodyReference { get { return _bodyReference; } }
        public System.Numerics.Vector3 Gravity = new System.Numerics.Vector3(0, -.1f, 0);

        BodyReference _bodyReference;
        bool _reportsContacts;

        public static RigidBody CreateRigidBody(Entity entity, Physics.Shapes.Shape shapeInfo, bool reportContacts, string name = nameof(RigidBody))
        {
            RigidBody rigidBody = new RigidBody();
            rigidBody.Name = name;
            rigidBody.QueueAttachToEntity(entity);

            if (shapeInfo is Physics.Shapes.Box)
                rigidBody._createBox((Physics.Shapes.Box)shapeInfo);
            else if (shapeInfo is Physics.Shapes.Cylinder)
                rigidBody._createCylinder((Physics.Shapes.Cylinder)shapeInfo);

            rigidBody._reportsContacts = reportContacts;
            rigidBody.setUpCollisionDetection();

            return rigidBody;
        }

        private void _createBox(Physics.Shapes.Box boxInfo)
        {
            if (boxInfo.Height_Y < 0)
                boxInfo = new Physics.Shapes.Box(_Entity.LocalTransform.Scale.X, _Entity.LocalTransform.Scale.Y, _Entity.LocalTransform.Scale.Z);
            Box box = new Box(boxInfo.Width_X, boxInfo.Height_Y, boxInfo.Length_Z);
            BodyInertia bodyInertia;
            box.ComputeInertia(1, out bodyInertia);
            BodyDescription bodyDescription = BodyDescription.CreateDynamic(
                new System.Numerics.Vector3(_Entity.GlobalTransform.Position.X, _Entity.GlobalTransform.Position.Y, _Entity.GlobalTransform.Position.Z),
                bodyInertia,
                new CollidableDescription(_Entity.Game._PhysicsSimulation.Shapes.Add(box), 0.1f),
                new BodyActivityDescription(0.01f));

            bodyDescription.Pose.Orientation = Utilities.ConvertQuaternionType(_Entity.LocalTransform.Quaternion);
            int _bodyHandle = _Entity.Game._PhysicsSimulation.Bodies.Add(bodyDescription);
            _bodyReference = _Entity.Game._PhysicsSimulation.Bodies.GetBodyReference(_bodyHandle);

            setUpCollisionDetection();
        }

        public void _createCylinder(Physics.Shapes.Cylinder cylinderInfo)
        {
            Cylinder cylinder = new Cylinder(cylinderInfo.Radius, cylinderInfo.Length);
            BodyInertia bodyInertia;
            cylinder.ComputeInertia(1, out bodyInertia);
            BodyDescription BodyDescription = BodyDescription.CreateDynamic(
                new System.Numerics.Vector3(_Entity.GlobalTransform.Position.X, _Entity.GlobalTransform.Position.Y, _Entity.GlobalTransform.Position.Z),
                bodyInertia,
                new CollidableDescription(_Entity.Game._PhysicsSimulation.Shapes.Add(cylinder), 0.1f),
                new BodyActivityDescription(0.01f));
            BodyDescription.Pose.Orientation = Utilities.ConvertQuaternionType(_Entity.LocalTransform.Quaternion);
            int _bodyHandle = _Entity.Game._PhysicsSimulation.Bodies.Add(BodyDescription);
            _bodyReference = _Entity.Game._PhysicsSimulation.Bodies.GetBodyReference(_bodyHandle);

            setUpCollisionDetection();
        }

        public override void EventInvoked(string evnt, params object[] eventArgs)
        {
            if (evnt == Component.UpdateEvent)
            {
                _bodyReference.Velocity.Linear += Gravity;
                localTransformMatchRigidBody();
            }
        }

        public bool IsColliding()
        {
            if (!_reportsContacts) { return false; }
            return Entity.Game._GamePhysics._ContactDictionary._IsColliding(new Physics.ContactInfo(_bodyReference.Handle, CollidableMobility.Dynamic));
        }

        public void IsAwake(bool value)
        {
            _bodyReference.Awake = value;
        }

        private void setUpCollisionDetection()
        {
            if (_reportsContacts)
            {
                Entity.Game._GamePhysics._ContactDictionary._Add(new Physics.ContactInfo(_bodyReference.Handle, CollidableMobility.Dynamic));
                _bodyReference.Activity.SleepThreshold = 0;
            }
        }

        private void localTransformMatchRigidBody()
        {
            Entity.LocalTransform.Position = Utilities.ConvertVector3Type(_bodyReference.Pose.Position);
            Entity.LocalTransform.Quaternion = Utilities.ConvertQuaternionType(_bodyReference.Pose.Orientation);
        }
    }
}
