using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using OpenTK;

using BogieEngineCore.Materials;
using BogieEngineCore.Shading;

namespace BogieEngineCore.Lighting
{
    public class PointLight : Light
    {
        public Vector3 Position = new Vector3(0);
        public Vector3 AmbientColor = new Vector3(1);
        public Vector3 DiffuseColor = new Vector3(1);
        public Vector3 SpecularColor = new Vector3(1);

        public float Constant = 1;
        public float Linear = .7f;
        public float Quadratic = 1.8f;

        public override void SetLightUniform(string lightName, Shader shader)
        {
            shader.SetUniform3(lightName + ".position", Position);
            shader.SetUniform3(lightName + ".ambient", AmbientColor);
            shader.SetUniform3(lightName + ".diffuse", DiffuseColor);
            shader.SetUniform3(lightName + ".specular", SpecularColor);

            shader.SetUniform1(lightName + ".constant", Constant);
            shader.SetUniform1(lightName + ".linear", Linear);
            shader.SetUniform1(lightName + ".quadratic", Quadratic);
        }

        public override Light Clone()
        {
            PointLight light = new PointLight();
            light.Position = Position;
            light.AmbientColor = AmbientColor;
            light.DiffuseColor = DiffuseColor;
            light.SpecularColor = SpecularColor;

            light.Constant = Constant;
            light.Linear = Linear;
            light.Quadratic = Quadratic;

            return light;
        }
    }
}
