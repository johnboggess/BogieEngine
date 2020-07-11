using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using OpenTK;

using BogieEngineCore.Shading;
using BogieEngineCore.Texturing;
using OpenTK.Graphics.ES10;

namespace BogieEngineCore.Materials
{
    public class NormalMaterial : Material
    {
        public Texture DiffuseTexture;
        public Texture SpecularTexture;
        public Texture NormalTexture;
        public float Shininess = 32;

        public override void SetMaterialUniform(string materialName, Shader shader)
        {
            shader.SetUniform1(materialName + ".diffuse", (int)DiffuseTexture.TextureUnit - (int)TextureUnit.Texture0);
            shader.SetUniform1(materialName + ".specular", (int)SpecularTexture.TextureUnit - (int)TextureUnit.Texture0);
            shader.SetUniform1(materialName + ".normal", (int)NormalTexture.TextureUnit - (int)TextureUnit.Texture0);
            shader.SetUniform1(materialName + ".shininess", Shininess);

            DiffuseTexture.Bind();
            SpecularTexture.Bind();
            NormalTexture.Bind();
        }

        public override Material Clone()
        {
            NormalMaterial material = new NormalMaterial();
            material.DiffuseTexture = DiffuseTexture;
            material.SpecularTexture = SpecularTexture;
            material.NormalTexture = NormalTexture;
            material.Shininess = Shininess;

            return material;
        }
    }
}
