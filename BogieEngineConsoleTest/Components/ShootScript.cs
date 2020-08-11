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
    class ShootScript : Component
    {
        public Entity Origin;

        float timer = 0f;
        float timerMax = 1f;

        public ShootScript(Entity origin)
        {
            Origin = origin;
        }

        public override void EventInvoked(string evnt, params object[] eventArgs)
        {
            if (evnt == Component.UpdateEvent)
                Update((float)(double)eventArgs[0]);
        }

        public void Update(float deltaT)
        {
            MouseState ms = Mouse.GetCursorState();

            timer -= deltaT;
            if (timer <= 0)
            {
                timer = 0;
                if (ms.LeftButton == ButtonState.Pressed)
                {
                    timer = timerMax;
                    Box box = new Box(Entity.Game.EntityWorld, false, Origin.LocalTransform.Position, new OpenTK.Vector3(1, 1, 1), (Game)Entity.Game);
                    
                    LifeTime lifeTime = new LifeTime(60);
                    lifeTime.Name = nameof(LifeTime);

                    box.ForceAddComponent(lifeTime);
                    
                    box.InstanceSetup = new Action(() =>
                    {
                        box.GetComponet<GravityScript>(nameof(GravityScript)).Gravity = Game.Gravity;
                        box.RigidBox.Velocity = Utilities.ConvertVector3Type(Origin.LocalTransform.Forwards * -50);
                    });
                }
                else if (ms.RightButton == ButtonState.Pressed)
                {
                    timer = timerMax;
                    Box box = new Box(Entity.Game.EntityWorld, false, Origin.LocalTransform.Position, new OpenTK.Vector3(1, 1, 1), (Game)Entity.Game);
                    box.InstanceSetup = new Action(() =>
                    {
                        box.GetComponet<GravityScript>(nameof(GravityScript)).Gravity = Game.Gravity;
                    });
                }
            }
        }
    }
}
