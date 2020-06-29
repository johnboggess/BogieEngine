using BogieEngineCore.Modelling;
using BogieEngineCore.Shading;
using BogieEngineCore.Texturing;
using OpenTK.Graphics.OpenGL4;
using System.Collections.Generic;
namespace BogieEngineCore
{
    /// <summary>
    /// Used to load various resources such as models an textures. Makes sure that the same resource are not uploaded to the GPU twice.
    /// </summary>
    public class ContentManager
    {
        Dictionary<string, TextureData> _pathToTextureData = new Dictionary<string, TextureData>();
        Dictionary<string, Model> _pathToModel = new Dictionary<string, Model>();

        public Texture LoadTexture(string filePath, TextureUnit textureUnit)
        {
            if (_pathToTextureData.ContainsKey(filePath))
            {
                return new Texture(_pathToTextureData[filePath]);
            }
            TextureData textureData = new TextureData(filePath, textureUnit);
            _pathToTextureData.Add(filePath, textureData);
            return new Texture(textureData);
        }

        public Model LoadModel(string filePath, Shader shader)
        {
            if (_pathToModel.ContainsKey(filePath))
            {
                Model mdl = new Model(_pathToModel[filePath]);
                mdl.SetShader(shader);
                return mdl;
            }
            Model model = ModelLoader.LoadModel(filePath, this, shader);
            _pathToModel.Add(filePath, model);
            return new Model(model);
        }
    }
}
