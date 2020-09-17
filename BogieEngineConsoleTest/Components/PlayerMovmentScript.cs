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
using BogieEngineCore.Lighting;
using BogieEngineCore.Physics;

using BogieEngineConsoleTest.Entities;

namespace BogieEngineConsoleTest.Components
{
    class PlayerMovmentScript : Component
    {
        BasicLight basicLight = new BasicLight();
        Camera _camera;
        RigidBox _playerBody;
        bool flashLight = false;

        bool _fUp = true;

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
            _playerBody.IsAwake(true);
            KeyboardState ks = Keyboard.GetState();

            _camera.LocalTransform.Position = _playerBody.Entity.LocalTransform.Position;
            _playerBody.Orientation = new BepuUtilities.Quaternion(0, 0, 0, 1);

            if (ks.IsKeyDown(Key.W) || ks.IsKeyDown(Key.S))
            {
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
                Vector3 vector = _camera.LocalTransform.Right.Normalized() * (ks.IsKeyDown(Key.A) ? -1 : 1);
                vector.Y = 0;
                System.Numerics.Vector3 vel = _playerBody.Velocity;
                vel.Y = 0;
                if (vel.LengthSquared() < 10)
                {
                    _playerBody.Velocity += Utilities.ConvertVector3Type(vector)*.5f;
                }
            }

            if (ks.IsKeyDown(Key.Space))
            {
                Ray ray = new Ray(_playerBody.Position, new System.Numerics.Vector3(0, -1, 0));
                float dist = 0;
                if (ray.Cast(new List<PhysicsObject>() { _playerBody }, out dist) != null && dist <= .5)
                {
                    _playerBody.Velocity += new System.Numerics.Vector3(0, 6, 0);
                }

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
                ((Game)Game.GlobalGame).NormalShader.BasicLight.Position = Entity.GlobalTransform.Position;

                ((Game)Game.GlobalGame).PhongShader.DirLight.Direction = Entity.GlobalTransform.Position;
                ((Game)Game.GlobalGame).PhongShader.PointLight.Position = Entity.GlobalTransform.Position;
            }

            if (_fUp && ks.IsKeyDown(Key.F))
                flashLight = !flashLight;
            _fUp = ks.IsKeyUp(Key.F);

            if (!flashLight)
            {
                ((Game)Game.GlobalGame).PhongShader.SpotLight.Position = new Vector3(0, 0, 0);
                ((Game)Game.GlobalGame).PhongShader.SpotLight.Direction = new Vector3(0, 1, 0);
            }
            else
            {
                ((Game)Game.GlobalGame).PhongShader.SpotLight.Position = Entity.GlobalTransform.Position;
                ((Game)Game.GlobalGame).PhongShader.SpotLight.Direction = -_camera.GlobalTransform.Forwards;
            }


            ((Game)Game.GlobalGame).PhongShader.ViewPosition = _camera.GlobalTransform.Position;//todo: track all shaders and have the cameras auto update view pos
            ((Game)Game.GlobalGame).NormalShader.ViewPosition = _camera.GlobalTransform.Position;
        }
    }
}
