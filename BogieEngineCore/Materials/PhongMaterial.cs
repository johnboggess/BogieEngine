using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using OpenTK;

using BogieEngineCore.Shading;
using BogieEngineCore.Texturing;

namespace BogieEngineCore.Materials
{
    public class PhongMaterial : Material
    {
        public Vector3 AmbientColor = new Vector3(1);
        public Vector3 DiffuseColor = new Vector3(1);
        public Vector3 SpecularColor = new Vector3(1);
        public float Shininess = 32;
        public Texture Texture;

        public override void SetMaterialUniform(string materialName, Shader shader)
        {
            Texture.Bind();
            shader.SetUniform3(materialName + ".ambient", AmbientColor);
            shader.SetUniform3(materialName + ".diffuse", DiffuseColor);
            shader.SetUniform3(materialName + ".specular", SpecularColor);
            shader.SetUniform1(materialName + ".shininess", Shininess);
        }

        public override Material Clone()
        {
            PhongMaterial material = new PhongMaterial();
            material.AmbientColor = this.AmbientColor;
            material.DiffuseColor = this.DiffuseColor;
            material.SpecularColor = this.SpecularColor;
            material.Shininess = this.Shininess;
            material.Texture = this.Texture;
            return material;
        }
    }
}
