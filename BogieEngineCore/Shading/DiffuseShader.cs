using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using OpenTK;
using OpenTK.Graphics.ES10;

namespace BogieEngineCore.Shading
{
    public class DiffuseShader : Shader
    {
        public DiffuseShader() : base("Resources/Shaders/default.vert", "Resources/Shaders/DiffuseLight.frag") 
        {
            LightPosition = new Vector3(0, 0, 0);
        }

        public Matrix4 Projection
        {
            set 
            {
                SetUniformMatrix(nameof(Projection), false, value);
            }
        }

        public Matrix4 View
        {
            set
            {
                SetUniformMatrix(nameof(View), false, value);
            }
        }

        public Matrix4 Model
        {
            set
            {
                SetUniformMatrix(nameof(Model), false, value);
            }
        }

        public Vector3 LightPosition
        {
            set
            {
                SetUniform3(nameof(LightPosition), value);
            }
        }

        public override void Use(params object[] values)
        {
            Model = (Matrix4)values[0];
        }
    }
}
