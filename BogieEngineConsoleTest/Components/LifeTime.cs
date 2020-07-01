using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using BogieEngineCore;
using BogieEngineCore.Entities;
using BogieEngineCore.Components;
using BogieEngineConsoleTest.Entities;

using OpenTK.Input;

namespace BogieEngineConsoleTest.Components
{
    class LifeTime : Component
    {
        float _timeLimit = 0;
        float _passedFrames = 0;

        public LifeTime(int timeLimit)
        {
            _timeLimit = timeLimit;
        }

        public override void EventInvoked(string evnt, params object[] eventArgs)
        {
            if (evnt == Component.UpdateEvent)
                Update((float)(double)eventArgs[0]);
        }

        public void Update(float deltaT)
        {
            if (_passedFrames >= _timeLimit)
                Entity.QueueDestroy();
            _passedFrames += 1;
        }
    }
}
