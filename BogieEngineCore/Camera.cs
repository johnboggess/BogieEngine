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
        public Matrix4 Projection;
        public Matrix4 Transform
        {
            get { return _transform; }
            set
            {
                _transform = value;
                View = Transform.Inverted();
            }
        }
        public Matrix4 View;


        private Matrix4 _transform;

        public Camera()
        {
            Projection = Matrix4.CreatePerspectiveFieldOfView(MathHelper.DegreesToRadians(45.0f), 500 / 500, 0.1f, 100.0f);
            Transform = Matrix4.CreateTranslation(0, 0, 3);
        }
    }
}
