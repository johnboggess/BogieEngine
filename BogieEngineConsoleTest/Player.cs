using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using OpenTK;
using OpenTK.Input;

using BogieEngineCore;
using BogieEngineCore.Nodes;
namespace BogieEngineConsoleTest
{
    class Player : RigidBody
    {
        Camera _camera;
        public Player(BaseGame baseGame, Camera camera) : base(baseGame, true)
        {
            _camera = camera;
            if(_camera is FPSCamera)
            {
                ((FPSCamera)_camera).Body = this;
            }
        }

        public override void Process(float deltaT, Transform parentWorldTransform)
        {

            KeyboardState ks = Keyboard.GetState();

            _camera.LocalTransform.Position = LocalTransform.Position;
            BodyReference.Pose.Orientation = new BepuUtilities.Quaternion(0, 0, 0, 1);

            if (ks.IsKeyDown(Key.W))
            {
                BodyReference.Awake = true;
                Vector3 vector = _camera.LocalTransform.Forwards.Normalized();
                vector.Y = 0;
                System.Numerics.Vector3 vel = BodyReference.Velocity.Linear;
                vel.Y = 0;
                if (vel.LengthSquared() < 25)
                {
                    BodyReference.Velocity.Linear -= Utilities.ConvertVector3Type(vector);
                }
            }
            if(ks.IsKeyDown(Key.Space) && IsColliding())
            {
                BodyReference.Awake = true;
                BodyReference.Velocity.Linear += new System.Numerics.Vector3(0, 6, 0);
            }

            if (BodyReference.Awake)
            {
                Vector3 velocity = Utilities.ConvertVector3Type(BodyReference.Velocity.Linear);
                Vector3 friction = -velocity.Normalized();
                friction = friction * Math.Min(BodyReference.Velocity.Linear.Length(), .2f);
                friction.Y = 0;

                if(!float.IsNaN(friction.X))
                {
                    BodyReference.Velocity.Linear += Utilities.ConvertVector3Type(friction);
                }

                BodyReference.Velocity.Linear += BogieEngineConsoleTest.Game.Gravity * deltaT;
            }
        }
    }
}
