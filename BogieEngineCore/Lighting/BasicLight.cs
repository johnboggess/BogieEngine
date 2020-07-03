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
    public class BasicLight : Light
    {
        public Vector3 Position = new Vector3(0);
        public Vector3 AmbientColor = new Vector3(1);
        public Vector3 DiffuseColor = new Vector3(1);
        public Vector3 SpecularColor = new Vector3(1);

        public override void SetLightUniform(string lightName, Shader shader)
        {
            shader.SetUniform3(lightName + ".position", Position);
            shader.SetUniform3(lightName + ".ambient", AmbientColor);
            shader.SetUniform3(lightName + ".diffuse", DiffuseColor);
            shader.SetUniform3(lightName + ".specular", SpecularColor);
        }

        public override Light Clone()
        {
            BasicLight basicLight = new BasicLight();
            basicLight.Position = Position;
            basicLight.AmbientColor = AmbientColor;
            basicLight.DiffuseColor = DiffuseColor;
            basicLight.SpecularColor = SpecularColor;

            return basicLight;
        }
    }
}
