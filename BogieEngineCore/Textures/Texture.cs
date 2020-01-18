using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using OpenTK.Graphics.OpenGL4;
namespace BogieEngineCore.Textures
{
    public class Texture : IDisposable
    {
        public bool Disposed { get { return _textureData.Disposed; } }
        public TextureUnit TextureUnit { get { return _textureData._TextureUnit; } set { _textureData._TextureUnit = value; } }
        public TextureWrapMode WrapMode
        {
            get { return _wrapMode; }
            set
            {
                _wrapMode = value;
                _textureData.Bind();
                GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)_wrapMode);
                GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)_wrapMode);
                _textureData.UnBind();
            }
        }
        public TextureMinFilter TextureMinFilter
        {
            get { return _textureMinFilter; }
            set
            {
                _textureMinFilter = value;
                _textureData.Bind();
                GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)_textureMinFilter);
                _textureData.UnBind();
            }
        }
        public TextureMagFilter TextureMagFilter
        {
            get { return _textureMagFilter; }
            set
            {
                _textureMagFilter = value;
                _textureData.Bind();
                GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)_textureMagFilter);
                _textureData.UnBind();
            }
        }


        TextureWrapMode _wrapMode = TextureWrapMode.Repeat;
        TextureMinFilter _textureMinFilter = TextureMinFilter.Linear;
        TextureMagFilter _textureMagFilter = TextureMagFilter.Linear;
        TextureData _textureData;

        internal Texture(TextureData textureData)
        {
            _textureData = textureData;

            _textureData.Bind();
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)_wrapMode);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)_wrapMode);

            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)_textureMinFilter);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)_textureMagFilter);
            _textureData.UnBind();
        }

        public void Bind()
        {
            _textureData.Bind();
        }

        public void UnBind()
        {
            _textureData.UnBind();
        }

        public void Dispose()
        {
            _textureData.Dispose();
        }
    }
}
