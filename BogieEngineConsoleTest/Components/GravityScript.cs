using BogieEngineCore.Components;
using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BogieEngineConsoleTest.Components
{
    class GravityScript : Component
    {
        public System.Numerics.Vector3 Gravity = System.Numerics.Vector3.Zero;
        public GravityScript() { Name = nameof(GravityScript); }

        public override void EventInvoked(string evnt, params object[] eventArgs)
        {
            if (evnt == Component.UpdateEvent)
                Update((double)eventArgs[0]);
        }

        public void Update(double deltaT)
        {
            Entity.GetComponet<RigidBox>(nameof(RigidBox)).BodyReference.Velocity.Linear += Gravity * (float)deltaT;
        }
    }
}
