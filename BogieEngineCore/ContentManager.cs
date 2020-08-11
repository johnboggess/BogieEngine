using BogieEngineCore.Modelling;
using BogieEngineCore.Shading;
using BogieEngineCore.Texturing;
using BogieEngineCore.Vertices;
using OpenTK.Graphics.OpenGL4;
using System;
using System.Collections.Generic;
namespace BogieEngineCore
{
    /// <summary>
    /// Used to load various resources such as models an textures. Makes sure that the same resource are not uploaded to the GPU twice.
    /// </summary>
    public class ContentManager
    {
        Dictionary<string, TextureData> _pathToTextureData = new Dictionary<string, TextureData>();

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

        public ModelData LoadModel<T>(string filePath, Shader shader, VertexDefinition vertexDefinition) where T : Materials.Material, new()
        {
            ModelData model = ModelLoader.LoadModel<T>(filePath, this, shader, vertexDefinition);
            return model;
        }
    }
}
