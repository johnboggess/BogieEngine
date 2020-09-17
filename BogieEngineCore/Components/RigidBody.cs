using System;
using System.Numerics;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using BepuPhysics.Collidables;
namespace BogieEngineCore.Components
{
    public class RigidBody : PhysicsObject
    {
        bool _recordsContacts = false;

        public RigidBody(float x, float y, float z, float scaleX, float scaleY, float scaleZ, bool recordContacts, string name, object otherData)
        {
            Name = name;
            _recordsContacts = recordContacts;

            _SetupCollider(x, y, z, scaleX, scaleY, scaleZ, otherData);
            if (recordContacts)
                _SetUpCollisionDetection();
            registerPhysicsObject();
        }

        public RigidBody(Vector3 position, Vector3 scale, bool recordsContacts, string name, object otherData) : this(position.X, position.Y, position.Z, scale.X, scale.Y, scale.Z, recordsContacts, name, otherData)
        {
        }

        public RigidBody(Transform transform, bool recordsContacts, string name, object otherData) : this(transform.Position.X, transform.Position.Y, transform.Position.Z, transform.Scale.X, transform.Scale.Y, transform.Scale.Z , recordsContacts, name, otherData)
        {
        }


        public override void EventInvoked(string evnt, params object[] eventArgs)
        {
            if (Destroyed)
                return;

            if (evnt == Component.UpdateEvent)
            {
                _LocalTransformMatchRigidBody();
            }
            else if (evnt == Component.DestroyEvent)
            {
                _BodyReference.Bodies.Remove(_BodyReference.Handle);
                unregisterPhysicsObject();
            }
        }

        public bool IsColliding()
        {
            if (!_recordsContacts) { return false; }
            return Entity.Game._GamePhysics._ContactDictionary._IsColliding(new Physics.ContactInfo(_BodyReference.Handle, CollidableMobility.Dynamic));
        }

        public void IsAwake(bool value)
        {
            _BodyReference.Awake = value;
        }

        #region Pose
        /// <summary>
        /// Sets the position of the rigid body. Updates attached entity.
        /// </summary>
        public Vector3 Position
        {
            get { return _BodyReference.Pose.Position; }
            set { _BodyReference.Pose.Position = value; _LocalTransformMatchRigidBody(); }
        }

        /// <summary>
        /// Sets the orientation of the rigid body. Updates attached entity.
        /// </summary>
        public BepuUtilities.Quaternion Orientation
        {
            get { return _BodyReference.Pose.Orientation; }
            set { _BodyReference.Pose.Orientation = value; _LocalTransformMatchRigidBody(); }
        }
        #endregion

        #region Physics
        public Vector3 Velocity
        {
            get { return _BodyReference.Velocity.Linear; }
            set { _BodyReference.Velocity.Linear = value; }
        }

        public Vector3 AngularVelocity
        {
            get { return _BodyReference.Velocity.Angular; }
            set { _BodyReference.Velocity.Angular = value; }
        }

        public void ApplyImpulse(Vector3 impluse, Vector3 impulseOffset)
        {
            _BodyReference.ApplyImpulse(in impluse, in impulseOffset);
        }

        public void ApplyAngularImpulse(Vector3 impluse)
        {
            _BodyReference.ApplyAngularImpulse(in impluse);
        }

        public void ApplyLinearImpulse(Vector3 impluse)
        {
            _BodyReference.ApplyLinearImpulse(in impluse);
        }
        #endregion

        protected virtual void _SetupCollider(float x, float y, float z, float scaleX, float scaleY, float scaleZ, object otherData) { throw new NotImplementedException(); }
        
        protected void _SetUpCollisionDetection()
        {
            BaseGame.GlobalGame._GamePhysics._ContactDictionary._Add(new Physics.ContactInfo(_BodyReference.Handle, CollidableMobility.Dynamic));
            _BodyReference.Activity.SleepThreshold = 0;
        }

        protected void _LocalTransformMatchRigidBody()
        {
            Entity.LocalTransform.Position = Utilities.ConvertVector3Type(_BodyReference.Pose.Position);
            Entity.LocalTransform.Quaternion = Utilities.ConvertQuaternionType(_BodyReference.Pose.Orientation);
        }
    }
}
