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
    public class SpotLight : Light
    {
        public Vector3 Position = new Vector3(0);
        public Vector3 Direction = new Vector3(0, -1, 0);

        public Vector3 AmbientColor = new Vector3(1);
        public Vector3 DiffuseColor = new Vector3(1);
        public Vector3 SpecularColor = new Vector3(1);

        public float CutOff = 12.5f;
        public float OuterCutOff = 17.5f;

        public override void SetLightUniform(string lightName, Shader shader)
        {
            shader.SetUniform3(lightName + ".position", Position);
            shader.SetUniform3(lightName + ".direction", Direction);

            shader.SetUniform1(lightName + ".cutOff", (float)Math.Cos((Math.PI / 180f) * CutOff));
            shader.SetUniform1(lightName + ".outerCutOff", (float)Math.Cos((Math.PI / 180f) * OuterCutOff));

            shader.SetUniform3(lightName + ".ambient", AmbientColor);
            shader.SetUniform3(lightName + ".diffuse", DiffuseColor);
            shader.SetUniform3(lightName + ".specular", SpecularColor);
        }

        public override Light Clone()
        {
            SpotLight spotLight = new SpotLight();
            spotLight.Position = Position;
            spotLight.Direction = Direction;
            spotLight.CutOff = CutOff;
            spotLight.OuterCutOff = OuterCutOff;
            spotLight.AmbientColor = AmbientColor;
            spotLight.DiffuseColor = DiffuseColor;
            spotLight.SpecularColor = SpecularColor;

            return spotLight;
        }
    }
}
