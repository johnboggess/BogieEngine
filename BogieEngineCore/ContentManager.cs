using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using OpenTK.Graphics.OpenGL4;

using BogieEngineCore.Shading;
using BogieEngineCore.Texturing;
namespace BogieEngineCore
{
    public class ContentManager
    {
        Dictionary<string, TextureData> _pathToTextureData = new Dictionary<string, TextureData>();

        public Texture LoadTexture(string filePath, TextureUnit textureUnit)
        {
            if(_pathToTextureData.ContainsKey(filePath))
            {
                return new Texture(_pathToTextureData[filePath]);
            }
            TextureData textureData = new TextureData(filePath, textureUnit);
            _pathToTextureData.Add(filePath, textureData);
            return new Texture(textureData);
        }
    }
}
