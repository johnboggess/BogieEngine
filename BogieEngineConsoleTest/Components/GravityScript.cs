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

        private string _rigidBodyName;

        public GravityScript(string rigidBodyName) 
        { 
            Name = nameof(GravityScript);
            _rigidBodyName = rigidBodyName;
        }

        public override void EventInvoked(string evnt, params object[] eventArgs)
        {
            if (evnt == Component.UpdateEvent)
                Update((double)eventArgs[0]);
        }

        public void Update(double deltaT)
        {
            Entity.GetComponet<RigidBody>(_rigidBodyName).IsAwake(true);
            Entity.GetComponet<RigidBody>(_rigidBodyName).Velocity += Gravity * (float)deltaT;
        }
    }
}
