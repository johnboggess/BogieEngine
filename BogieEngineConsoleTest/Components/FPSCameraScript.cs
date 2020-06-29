using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using OpenTK.Input;
using OpenTK;

using BogieEngineCore;
using BogieEngineCore.Entities;
using BogieEngineCore.Components;

namespace BogieEngineConsoleTest.Components
{
    class FPSCameraScript : Component
    {
        float moveScale = 1f;

        bool initMouse = true;
        float lastX = 0f;
        float lastY = 0f;

        float yawScale = 0.01f;
        float pitchScale = 0.01f;

        float upDownLimit = 1;

        Camera camera;
        public FPSCameraScript(Camera cameraEntity)
        {
            camera = cameraEntity;
            camera.QueueAddComponent(this);
        }

        public override void EventInvoked(string evnt, params object[] eventArgs)
        {
            if (evnt == Component.UpdateEvent)
                Update();
        }

        public void Update()
        {
            MouseState ms = Mouse.GetCursorState();

            if (initMouse)
            {
                initMouse = false;
                lastX = ms.X;
                lastY = ms.Y;
            }

            float diffX = ms.X - lastX;
            float diffY = ms.Y - lastY;
            lastX = ms.X;
            lastY = ms.Y;

            float currentRot = Transform.RotationToPlane(camera.LocalTransform.Forwards, Vector3.UnitY);

            camera.LocalTransform.Rotate(camera.LocalTransform.Right, -diffY * pitchScale);
            camera.LocalTransform.Rotate(Vector3.UnitY, -diffX * yawScale);
        }
    }
}
