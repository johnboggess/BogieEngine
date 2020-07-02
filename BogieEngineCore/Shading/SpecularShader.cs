using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using OpenTK;
using OpenTK.Graphics.ES10;

namespace BogieEngineCore.Shading
{
    public class SpecularShader : Shader
    {
        public SpecularShader() : base("Resources/Shaders/default.vert", "Resources/Shaders/SpecularLight.frag") { }

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

        public Vector3 ViewPosition
        {
            set
            {
                SetUniform3(nameof(ViewPosition), value);
            }
        }

        public override void Use(params object[] values)
        {
            Model = (Matrix4)values[0];
        }
    }
}
