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
        /// <summary>
        /// Reference to the physics body of the object.
        /// </summary>
        public BodyReference BodyReference;

        /// <summary>
        /// Handle of the physics body.
        /// </summary>
        private int _bodyHandle;
        /// <summary>
        /// Does the object store contact information for the dev to check collision information. Leave false if collision information is not necessary.
        /// </summary>
        private bool _reportsContacts;

        //todo: keep documenting
        public RigidBody(BaseGame game, bool reportsContacts = false) : base(game)
        {
            this._reportsContacts = reportsContacts;
        }

        public void CreateBox(Transform parentWorldTransform)
        {
            Matrix4 worldTransform = LocalTransform.GetMatrix4() * parentWorldTransform.GetMatrix4();
            Box box = new Box(LocalTransform.Scale.X, LocalTransform.Scale.Y, LocalTransform.Scale.Z);
            BodyInertia bodyInertia;
            box.ComputeInertia(1, out bodyInertia);
            BodyDescription BodyDescription = BodyDescription.CreateDynamic(
                new System.Numerics.Vector3(worldTransform.M41, worldTransform.M42, worldTransform.M43),
                bodyInertia,
                new CollidableDescription(Game._PhysicsSimulation.Shapes.Add(box), 0.1f),
                new BodyActivityDescription(0.01f));
            BodyDescription.Pose.Orientation = Utilities.ConvertQuaternionType(LocalTransform.Quaternion);
            _bodyHandle = Game._PhysicsSimulation.Bodies.Add(BodyDescription);
            BodyReference = Game._PhysicsSimulation.Bodies.GetBodyReference(_bodyHandle);

            createShapeCommon();
        }

        public void CreateCylinder(float radius, float length, Transform parentWorldTransform)
        {
            Matrix4 worldTransform = LocalTransform.GetMatrix4() * parentWorldTransform.GetMatrix4();
            Cylinder cylinder = new Cylinder(radius, length);
            BodyInertia bodyInertia;
            cylinder.ComputeInertia(1, out bodyInertia);
            BodyDescription BodyDescription = BodyDescription.CreateDynamic(
                new System.Numerics.Vector3(worldTransform.M41, worldTransform.M42, worldTransform.M43),
                bodyInertia,
                new CollidableDescription(Game._PhysicsSimulation.Shapes.Add(cylinder), 0.1f),
                new BodyActivityDescription(0.01f));
            BodyDescription.Pose.Orientation = Utilities.ConvertQuaternionType(LocalTransform.Quaternion);
            _bodyHandle = Game._PhysicsSimulation.Bodies.Add(BodyDescription);
            BodyReference = Game._PhysicsSimulation.Bodies.GetBodyReference(_bodyHandle);

            createShapeCommon();
        }

        public bool IsColliding()
        {
            if (!_reportsContacts) { return false; }
            return Game._GamePhysics._ContactDictionary._IsColliding(BodyReference.Handle);

        }

        internal override void _Process(float deltaT, Transform parentWorldTransform)
        {
            localTransformMatchRigidBody(parentWorldTransform);
            base._Process(deltaT, parentWorldTransform);
        }

        private void createShapeCommon()
        {
            if (_reportsContacts)
            {
                Game._GamePhysics._ContactDictionary._Add(_bodyHandle);
                BodyReference.Activity.SleepThreshold = 0;
            }
        }

        private void localTransformMatchRigidBody(Transform parentWorldTransform)
        {
            Matrix4 worldToLocalTransform = parentWorldTransform.GetMatrix4();
            worldToLocalTransform.Invert();
            LocalTransform.Position = Utilities.ConvertVector3Type(BodyReference.Pose.Position);
            LocalTransform.Quaternion = Utilities.ConvertQuaternionType(BodyReference.Pose.Orientation);
        }

    }
}
