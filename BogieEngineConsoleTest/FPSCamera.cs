using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using BogieEngineCore.Nodes;

using OpenTK.Input;
using OpenTK;

namespace BogieEngineConsoleTest
{
    class FPSCamera : Camera
    {
        float moveScale = .1f;

        bool initMouse = true;
        float lastX = 0f;
        float lastY = 0f;

        float yawScale = 0.01f;
        float pitchScale = 0.01f;

        public override void Process()
        {
            KeyboardState ks = Keyboard.GetState();
            MouseState ms = Mouse.GetCursorState();

            if(initMouse)
            {
                initMouse = false;
                lastX = ms.X;
                lastY = ms.Y;
            }

            float diffX = ms.X - lastX;
            float diffY = ms.Y - lastY;
            lastX = ms.X;
            lastY = ms.Y;

            Transform.Rotate(Vector3.UnitY, diffX * yawScale);
            Transform.Rotate(Transform.Right, diffY * pitchScale);

            if (ks.IsKeyDown(Key.A))
            {
                Transform.Position -= Transform.Right * moveScale;
            }
            if (ks.IsKeyDown(Key.D))
            {
                Transform.Position += Transform.Right * moveScale;
            }

            if (ks.IsKeyDown(Key.W))
            {
                Transform.Position += Transform.Forwards * moveScale;
            }
            if (ks.IsKeyDown(Key.S))
            {
                Transform.Position -= Transform.Forwards * moveScale;
            }

            if (ks.IsKeyDown(Key.ShiftLeft))
            {
                Transform.Position += Vector3.UnitY * moveScale;
            }
            if (ks.IsKeyDown(Key.ControlLeft))
            {
                Transform.Position -= Vector3.UnitY * moveScale;
            }
        }
    }
}
