using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using OpenTK;
using OpenTK.Graphics.OpenGL4;
namespace BogieEngineCore
{
    public class Camera
    {
        public Transform Transform = new Transform();

        public Matrix4 Projection;
        public Matrix4 View { get { return Transform.GetMatrix4().Inverted(); } }
        public Vector3 Position = new Vector3(0,0,3);
        public Vector3 Forwards = -Vector3.UnitZ;
        public Vector3 Up = Vector3.UnitY;
        public Vector3 Rotation = Vector3.Zero;

        public Camera()
        {
            Transform.Position = new Vector3(0, 0, 3);
            Projection = Matrix4.CreatePerspectiveFieldOfView(MathHelper.DegreesToRadians(45.0f), 500 / 500, 0.1f, 100.0f);
        }
    }
}
