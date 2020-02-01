using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using OpenTK;
using OpenTK.Graphics.OpenGL4;
namespace BogieEngineCore.Nodes
{
    public class Camera : Node
    {
        public Matrix4 Projection;
        public Matrix4 View { get { return WorldTransform.GetMatrix4().Inverted(); } }

        public Camera(BaseGame game) : base(game)
        {
            Projection = Matrix4.CreatePerspectiveFieldOfView(MathHelper.DegreesToRadians(45.0f), 500 / 500, 0.1f, 100.0f);
        }
    }
}
