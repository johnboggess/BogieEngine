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
    public class TextureMaterial : Material
    {
        public Texture Texture;

        public TextureMaterial(Texture texture)
        {
            Texture = texture;
        }

        public override void SetMaterialUniform(string materialName, Shader shader)
        {
            Texture.Bind();
            //shader.SetUniform1(materialName + ".texture", Texture);
        }

        public override Material Clone()
        {
            return new TextureMaterial(Texture);
        }
    }
}
