using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Advanced;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;

using OpenTK;
using OpenTK.Graphics.OpenGL4;

namespace BogieEngineCore.Textures
{
    public class Texture
    {
        int _handle;
        TextureUnit _textureUnit;


        public Texture(string filePath, TextureUnit textureUnit, TextureWrapMode wrapMode = TextureWrapMode.Repeat, TextureMinFilter minFilter = TextureMinFilter.Linear, TextureMagFilter magFilter = TextureMagFilter.Linear)
        {
            _handle = GL.GenTexture();
            _textureUnit = textureUnit;

            Image<Rgba32> image = (Image<Rgba32>)Image.Load(filePath);
            image.Mutate(x => x.Flip(FlipMode.Vertical));
            Rgba32[] tempPixels = image.GetPixelSpan().ToArray();
            List<byte> pixels = new List<byte>();
            foreach (Rgba32 p in tempPixels)
            {
                pixels.Add(p.R);
                pixels.Add(p.G);
                pixels.Add(p.B);
                pixels.Add(p.A);
            }

            Bind();

            //repeat the texture in both direction
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)wrapMode);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)wrapMode);

            //Scale the texture linearly
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)minFilter);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)magFilter);

            GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, image.Width, image.Height, 0, PixelFormat.Rgba, PixelType.UnsignedByte, pixels.ToArray());
            //GL.GenerateMipmaps()
            UnBind();
        }

        public void Bind()
        {
            GL.ActiveTexture(_textureUnit);
            GL.BindTexture(TextureTarget.Texture2D, _handle);
        }

        public void UnBind()
        {
            GL.BindTexture(TextureTarget.Texture2D, 0);
        }

        public void Dispose()
        {
            GL.DeleteTexture(_handle);
        }
    }
}
