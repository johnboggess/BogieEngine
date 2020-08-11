using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using OpenTK;

using BogieEngineCore.Shading;
using BogieEngineCore.Texturing;
using OpenTK.Graphics.ES10;

using Assimp;

namespace BogieEngineCore.Materials
{
    public class PhongMaterial : Material
    {
        public Texture DiffuseTexture;
        public Texture SpecularTexture;
        public float Shininess = 32;

        public override void LoadFromMesh(Mesh mesh, Scene scene, ContentManager contentManager, string folder)
        {
            if (mesh.MaterialIndex > -1)
            {
                Assimp.Material material = scene.Materials[mesh.MaterialIndex];
                string diffusePath = material.TextureDiffuse.FilePath;
                string specularPath = material.TextureSpecular.FilePath;
                if (diffusePath != null)
                    DiffuseTexture = contentManager.LoadTexture(folder + "/" + diffusePath, OpenTK.Graphics.OpenGL4.TextureUnit.Texture0);
                if (specularPath != null)
                    SpecularTexture = contentManager.LoadTexture(folder + "/" + specularPath, OpenTK.Graphics.OpenGL4.TextureUnit.Texture1);
                Shininess = material.Shininess;
            }
        }

        public override void SetMaterialUniform(string materialName, Shader shader)
        {
            shader.SetUniform1(materialName + ".diffuse", (int)DiffuseTexture.TextureUnit - (int)TextureUnit.Texture0);
            shader.SetUniform1(materialName + ".specular", (int)SpecularTexture.TextureUnit - (int)TextureUnit.Texture0);
            shader.SetUniform1(materialName + ".shininess", Shininess);

            DiffuseTexture.Bind();
            SpecularTexture.Bind();
        }

        public override Material Clone()
        {
            PhongMaterial material = new PhongMaterial();
            material.DiffuseTexture = DiffuseTexture;
            material.SpecularTexture = SpecularTexture;
            material.Shininess = Shininess;

            return material;
        }
    }
}
