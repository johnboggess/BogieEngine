using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using BogieEngineCore.Components;

using BepuPhysics;
using BepuPhysics.Collidables;

namespace BogieEngineCore.Components
{
    public class PhysicsObject : Component
    {
        internal int _BodyHandle;
        internal BodyReference _BodyReference;
        internal bool _IsBody;

        protected void registerPhysicsObject()
        {
            BaseGame.GlobalGame._GamePhysics.PhysicsObjectAdded(this);
        }

        protected void unregisterPhysicsObject()
        {
            BaseGame.GlobalGame._GamePhysics.PhysicsObjectRemoved(this);
        }
    }
}
