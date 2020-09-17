
using OpenTK.Graphics.OpenGL4;
namespace BogieEngineCore.Texturing
{
    public class Texture : IDisposable
    {
        public bool Disposed { get { return _textureData.Disposed; } }
        public TextureUnit TextureUnit;
        public TextureWrapMode WrapMode
        {
            get { return _wrapMode; }
            set
            {
                _wrapMode = value;
                _textureData.Bind(TextureUnit);
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
                _textureData.Bind(TextureUnit);
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
                _textureData.Bind(TextureUnit);
                GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)_textureMagFilter);
                _textureData.UnBind();
            }
        }


        TextureWrapMode _wrapMode = TextureWrapMode.Repeat;
        TextureMinFilter _textureMinFilter = TextureMinFilter.Linear;
        TextureMagFilter _textureMagFilter = TextureMagFilter.Linear;
        TextureData _textureData;

        internal Texture(TextureData textureData, TextureUnit textureUnit)
        {
            _textureData = textureData;
            TextureUnit = textureUnit;

            _textureData.Bind(TextureUnit);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)_wrapMode);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)_wrapMode);

            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)_textureMinFilter);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)_textureMagFilter);
            _textureData.UnBind();
        }

        public void Bind()
        {
            _textureData.Bind(TextureUnit);
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
