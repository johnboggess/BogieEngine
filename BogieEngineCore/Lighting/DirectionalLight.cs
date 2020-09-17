using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using OpenTK;

using BogieEngineCore.Shading;

namespace BogieEngineCore.Lighting
{
    public class DirectionalLight : Light
    {
        public Vector3 Direction = new Vector3(0,-1,0);
        public Vector3 AmbientColor = new Vector3(1);
        public Vector3 DiffuseColor = new Vector3(1);
        public Vector3 SpecularColor = new Vector3(1);

        public override void SetLightUniform(string lightName, Shader shader)
        {
            shader.SetUniform3(lightName + ".direction", Direction);
            shader.SetUniform3(lightName + ".ambient", AmbientColor);
            shader.SetUniform3(lightName + ".diffuse", DiffuseColor);
            shader.SetUniform3(lightName + ".specular", SpecularColor);
        }

        public override Light Clone()
        {
            DirectionalLight directionalLight = new DirectionalLight();
            directionalLight.Direction = Direction;
            directionalLight.AmbientColor = AmbientColor;
            directionalLight.DiffuseColor = DiffuseColor;
            directionalLight.SpecularColor = SpecularColor;
            return directionalLight;
        }
    }
}
