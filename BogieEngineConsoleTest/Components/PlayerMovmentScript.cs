using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using OpenTK.Input;
using OpenTK;

using BogieEngineCore;
using BogieEngineCore.Components;
using BogieEngineCore.Entities;
using BogieEngineConsoleTest.Entities;

namespace BogieEngineConsoleTest.Components
{
    class PlayerMovmentScript : Component
    {
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

            if (ks.IsKeyDown(Key.W))
            {
                _playerBody.IsAwake(true);
                Vector3 vector = _camera.LocalTransform.Forwards.Normalized();
                vector.Y = 0;
                System.Numerics.Vector3 vel = _playerBody.Velocity;
                vel.Y = 0;
                if (vel.LengthSquared() < 25)
                {
                    _playerBody.Velocity -= Utilities.ConvertVector3Type(vector);
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
                ((Game)Game.GlobalGame).DiffuseShader.LightPosition = Entity.GlobalTransform.Position;
                ((Game)Game.GlobalGame).SpecularShader.LightPosition = Entity.GlobalTransform.Position;
                ((Game)Game.GlobalGame).PhongShader.LightPosition = Entity.GlobalTransform.Position;
            }

            ((Game)Game.GlobalGame).SpecularShader.ViewPosition = _camera.GlobalTransform.Position;
            ((Game)Game.GlobalGame).PhongShader.ViewPosition = _camera.GlobalTransform.Position;
        }
    }
}
