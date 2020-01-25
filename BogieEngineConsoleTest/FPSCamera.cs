using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using BogieEngineCore;
using BogieEngineCore.Nodes;

using OpenTK.Input;
using OpenTK;

namespace BogieEngineConsoleTest
{
    class FPSCamera : Camera
    {
        float moveScale = 1f;

        bool initMouse = true;
        float lastX = 0f;
        float lastY = 0f;

        float yawScale = 0.01f;
        float pitchScale = 0.01f;

        public override void Process(float deltaT, Transform parentWorldTransform)
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

            LocalTransform.Rotate(Vector3.UnitY, diffX * yawScale);
            LocalTransform.Rotate(LocalTransform.Right, diffY * pitchScale);
            //Console.WriteLine(diffY * pitchScale);


            if (ks.IsKeyDown(Key.A))
            {
                LocalTransform.Position -= LocalTransform.Right * moveScale * deltaT;
            }
            if (ks.IsKeyDown(Key.D))
            {
                LocalTransform.Position += LocalTransform.Right * moveScale * deltaT;
            }

            if (ks.IsKeyDown(Key.W))
            {
                LocalTransform.Position -= LocalTransform.Forwards * moveScale * deltaT;
            }
            if (ks.IsKeyDown(Key.S))
            {
                LocalTransform.Position += LocalTransform.Forwards * moveScale * deltaT;
            }

            if (ks.IsKeyDown(Key.Q))
            {
                LocalTransform.Rotate(LocalTransform.Forwards, .1f);
            }
            if (ks.IsKeyDown(Key.E))
            {
                LocalTransform.Rotate(LocalTransform.Forwards, -.1f);
            }

            if (ks.IsKeyDown(Key.ShiftLeft))
            {
                LocalTransform.Position += Vector3.UnitY * moveScale * deltaT;
            }
            if (ks.IsKeyDown(Key.ControlLeft))
            {
                LocalTransform.Position -= Vector3.UnitY * moveScale * deltaT;
            }
        }
    }
}
