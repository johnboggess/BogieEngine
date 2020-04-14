using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using OpenTK;
using OpenTK.Graphics.OpenGL4;
namespace BogieEngineCore.Nodes
{
    /// <summary>
    /// Camera for viewing the world.
    /// </summary>
    public class Camera : Node
    {
        /// <summary>
        /// Projection Matrix. Projects from view space to the screen. Perspective projection by default.
        /// </summary>
        public Matrix4 Projection;
        /// <summary>
        /// View space (relative to camera) transform
        /// </summary>
        public Matrix4 View { get { return WorldTransform.GetMatrix4().Inverted(); } }

        public Camera(BaseGame game) : base(game)
        {
            Projection = Matrix4.CreatePerspectiveFieldOfView(MathHelper.DegreesToRadians(45.0f), 500 / 500, 0.1f, 100.0f);
        }
    }
}
