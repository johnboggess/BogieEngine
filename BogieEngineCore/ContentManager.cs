using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using OpenTK.Graphics.OpenGL4;

using BogieEngineCore.Shading;
using BogieEngineCore.Modelling;
using BogieEngineCore.Texturing;
namespace BogieEngineCore
{
    /// <summary>
    /// Used to load various resources such as models an textures. Makes sure that the same resource are not uploaded to the GPU twice.
    /// </summary>
    public class ContentManager
    {
        Game _game;
        Dictionary<string, TextureData> _pathToTextureData = new Dictionary<string, TextureData>();
        Dictionary<string, Model> _pathToModel = new Dictionary<string, Model>();

        public ContentManager(Game game)
        {
            _game = game;
        }


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

        public Model LoadModel(string filePath)
        {
            if(_pathToModel.ContainsKey(filePath))
            {
                return new Model(_pathToModel[filePath], _game);
            }
            Model model = ModelLoader.LoadModel(filePath, _game);
            _pathToModel.Add(filePath, model);
            return new Model(model, _game);
        }
    }
}
