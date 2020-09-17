using System.Collections.Concurrent;

using BogieEngineCore.Components;

using BepuUtilities.Memory;
using BepuPhysics;
using BepuPhysics.Collidables;

namespace BogieEngineCore.Physics
{
    public class GamePhysics
    {
        internal Simulation _PhysicsSimulation;
        internal ContactDictionary _ContactDictionary = new ContactDictionary();
        internal BufferPool _BufferPool = new BufferPool();

        private ConcurrentDictionary<PhysicsObjectStorageKey, PhysicsObject> PhysicsObjects = new ConcurrentDictionary<PhysicsObjectStorageKey, PhysicsObject>();

        SimpleThreadDispatcher _threadDispatcher;

        internal GamePhysics(Simulation physicsSimulation, SimpleThreadDispatcher threadDispatcher)
        {
            _PhysicsSimulation = physicsSimulation;
            this._threadDispatcher = threadDispatcher;
        }

        internal void _Timestep(float dt)
        {
            _ContactDictionary._Clear();
            _PhysicsSimulation.Timestep(dt, _threadDispatcher);
        }

        internal void PhysicsObjectAdded(PhysicsObject physicsObject)
        {
            PhysicsObjectStorageKey key = new PhysicsObjectStorageKey()
            {
                BodyHandle = physicsObject._BodyHandle,
                IsBody = physicsObject._IsBody
            };
            PhysicsObjects.TryAdd(key, physicsObject);
        }

        internal void PhysicsObjectRemoved(PhysicsObject physicsObject)
        {
            PhysicsObjectStorageKey key = new PhysicsObjectStorageKey()
            {
                BodyHandle = physicsObject._BodyHandle,
                IsBody = physicsObject._IsBody
            };
            PhysicsObject poOut;
            PhysicsObjects.TryRemove(key, out poOut);
        }

        internal PhysicsObject GetPhysicsObject(int bodyHandle, bool isBody)
        {
            PhysicsObjectStorageKey key = new PhysicsObjectStorageKey()
            {
                BodyHandle = bodyHandle,
                IsBody = isBody
            };
            PhysicsObject poOut;
            PhysicsObjects.TryGetValue(key, out poOut);
            return poOut;
        }

        internal PhysicsObject GetPhysicsObject(CollidableReference collidableReference)
        {
            return GetPhysicsObject(collidableReference.Handle, collidableReference.Mobility == CollidableMobility.Static ? false : true);
        }

        private struct PhysicsObjectStorageKey
        {
            public int BodyHandle;
            public bool IsBody;
        }
    }
}
