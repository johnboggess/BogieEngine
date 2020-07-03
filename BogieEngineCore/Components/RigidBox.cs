using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

using BepuPhysics;
using BepuPhysics.Collidables;
namespace BogieEngineCore.Components
{
    public class RigidBox : Component
    {//todo: should have resize method?

        float _x;
        float _y;
        float _z;

        float _widthX;
        float _heightY;
        float _lengthZ;

        bool _recordsContacts = false;

        BodyReference _bodyReference;

        public RigidBox(float x, float y, float z, float widthX, float heightY, float lengthZ, bool recordContacts)
        {
            Name = nameof(RigidBox);
            _x = x;
            _y = y;
            _z = z;

            _widthX = widthX;
            _heightY = heightY;
            _lengthZ = lengthZ;

            _recordsContacts = recordContacts;

            _setupBox();

            if(recordContacts)
                _setUpCollisionDetection();
        }

        public override void EventInvoked(string evnt, params object[] eventArgs)
        {
            if (Destroyed)
                return;

            if (evnt == Component.UpdateEvent)
            {
                _localTransformMatchRigidBody();
            }
            else if(evnt == Component.DestroyEvent)
            {
                _bodyReference.Bodies.Remove(_bodyReference.Handle);
            }
        }

        public bool IsColliding()
        {
            if (!_recordsContacts) { return false; }
            return Entity.Game._GamePhysics._ContactDictionary._IsColliding(new Physics.ContactInfo(_bodyReference.Handle, CollidableMobility.Dynamic));
        }

        public void IsAwake(bool value)
        {
            _bodyReference.Awake = value;
        }

        #region Pose
        /// <summary>
        /// Sets the position of the rigid body. Updates attached entity.
        /// </summary>
        public Vector3 Position
        {
            get { return _bodyReference.Pose.Position; }
            set { _bodyReference.Pose.Position = value; _localTransformMatchRigidBody(); }
        }

        /// <summary>
        /// Sets the orientation of the rigid body. Updates attached entity.
        /// </summary>
        public BepuUtilities.Quaternion Orientation
        {
            get { return _bodyReference.Pose.Orientation; }
            set { _bodyReference.Pose.Orientation = value; _localTransformMatchRigidBody(); }
        }
        #endregion

        #region Physics
        public Vector3 Velocity
        {
            get { return _bodyReference.Velocity.Linear; }
            set { _bodyReference.Velocity.Linear = value; }
        }

        public Vector3 AngularVelocity
        {
            get { return _bodyReference.Velocity.Angular; }
            set { _bodyReference.Velocity.Angular = value; }
        }

        public void ApplyImpulse(Vector3 impluse, Vector3 impulseOffset)
        {
            _bodyReference.ApplyImpulse(in impluse, in impulseOffset);
        }

        public void ApplyAngularImpulse(Vector3 impluse)
        {
            _bodyReference.ApplyAngularImpulse(in impluse);
        }

        public void ApplyLinearImpulse(Vector3 impluse)
        {
            _bodyReference.ApplyLinearImpulse(in impluse);
        }
        #endregion

        private void _setupBox()
        {
            Box box = new Box(_widthX, _heightY, _lengthZ);
            BodyInertia bodyInertia;
            box.ComputeInertia(1, out bodyInertia);
            BodyDescription bodyDescription = BodyDescription.CreateDynamic(
                new System.Numerics.Vector3(_x, _y, _z),
                bodyInertia,
                new CollidableDescription(BaseGame.GlobalGame._PhysicsSimulation.Shapes.Add(box), 0.1f),
                new BodyActivityDescription(0.01f));

            //todo: get this to work. or remove
            //bodyDescription.Pose.Orientation = Utilities.ConvertQuaternionType(_Entity.LocalTransform.Quaternion);
            
            int _bodyHandle = BaseGame.GlobalGame._PhysicsSimulation.Bodies.Add(bodyDescription);
            _bodyReference = BaseGame.GlobalGame._PhysicsSimulation.Bodies.GetBodyReference(_bodyHandle);
        }

        private void _setUpCollisionDetection()
        {
            BaseGame.GlobalGame._GamePhysics._ContactDictionary._Add(new Physics.ContactInfo(_bodyReference.Handle, CollidableMobility.Dynamic));
            _bodyReference.Activity.SleepThreshold = 0;
        }

        private void _localTransformMatchRigidBody()
        {
            Entity.LocalTransform.Position = Utilities.ConvertVector3Type(_bodyReference.Pose.Position);
            Entity.LocalTransform.Quaternion = Utilities.ConvertQuaternionType(_bodyReference.Pose.Orientation);
        }
    }
}
