using System.Collections.Concurrent;

using BepuPhysics;

namespace BogieEngineCore.Physics
{
    public class GamePhysics
    {
        internal Simulation _PhysicsSimulation;
        internal ContactDictionary _ContactDictionary = new ContactDictionary();

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
    }
}
