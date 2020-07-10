﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using OpenTK.Input;
using OpenTK;

using BogieEngineCore;
using BogieEngineCore.Components;
using BogieEngineCore.Entities;
using BogieEngineCore.Lighting;
using BogieEngineConsoleTest.Entities;

namespace BogieEngineConsoleTest.Components
{
    class PlayerMovmentScript : Component
    {
        BasicLight basicLight = new BasicLight();
        Camera _camera;
        RigidBox _playerBody;

        public PlayerMovmentScript(Camera camera, RigidBox playerBody)
        {
            _camera = camera;
            _playerBody = playerBody;
        }

        public override void EventInvoked(string evnt, params object[] eventArgs)
        {
            if (evnt == Component.UpdateEvent)
                Update((double)eventArgs[0]);
        }

        public void Update(double deltaT)
        {
            KeyboardState ks = Keyboard.GetState();

            _camera.LocalTransform.Position = _playerBody.Entity.LocalTransform.Position;
            _playerBody.Orientation = new BepuUtilities.Quaternion(0, 0, 0, 1);

            if (ks.IsKeyDown(Key.W) || ks.IsKeyDown(Key.S))
            {
                _playerBody.IsAwake(true);
                Vector3 vector = _camera.LocalTransform.Forwards.Normalized() * (ks.IsKeyDown(Key.S) ? -1 : 1);
                vector.Y = 0;
                System.Numerics.Vector3 vel = _playerBody.Velocity;
                vel.Y = 0;
                if (vel.LengthSquared() < 25)
                {
                    _playerBody.Velocity -= Utilities.ConvertVector3Type(vector);
                }
            }
            else if(ks.IsKeyDown(Key.A) || ks.IsKeyDown(Key.D))
            {
                _playerBody.IsAwake(true);
                Vector3 vector = _camera.LocalTransform.Right.Normalized() * (ks.IsKeyDown(Key.A) ? -1 : 1);
                vector.Y = 0;
                System.Numerics.Vector3 vel = _playerBody.Velocity;
                vel.Y = 0;
                if (vel.LengthSquared() < 10)
                {
                    _playerBody.Velocity += Utilities.ConvertVector3Type(vector)*.5f;
                }
            }

            if (ks.IsKeyDown(Key.Space) && _playerBody.IsColliding())
            {
                _playerBody.Velocity += new System.Numerics.Vector3(0, 6, 0);
            }

            Vector3 velocity = Utilities.ConvertVector3Type(_playerBody.Velocity);
            Vector3 friction = -velocity.Normalized();
            friction = friction * Math.Min(_playerBody.Velocity.Length(), .2f);
            friction.Y = 0;

            if (!float.IsNaN(friction.X))
            {
                _playerBody.Velocity += Utilities.ConvertVector3Type(friction);
            }

            _playerBody.Velocity += BogieEngineConsoleTest.Game.Gravity * (float)deltaT;

            if(ks.IsKeyDown(Key.L))
            {
                ((Game)Game.GlobalGame).PhongShader.BasicLight.Position = Entity.GlobalTransform.Position;
                ((Game)Game.GlobalGame).NormalShader.BasicLight.Position = Entity.GlobalTransform.Position;
            }

            ((Game)Game.GlobalGame).PhongShader.ViewPosition = _camera.GlobalTransform.Position;
            ((Game)Game.GlobalGame).NormalShader.ViewPosition = _camera.GlobalTransform.Position;
        }
    }
}
