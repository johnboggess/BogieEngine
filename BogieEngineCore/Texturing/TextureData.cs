using OpenTK.Graphics.OpenGL4;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Advanced;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using System.Collections.Generic;

namespace BogieEngineCore.Texturing
{
    public class TextureData : IDisposable
    {
        public bool Disposed { get { return _disposed; } }
        internal TextureUnit _TextureUnit;

        int _handle;
        private bool _disposed = false;

        internal TextureData(string filePath, TextureUnit textureUnit)
        {
            _handle = GL.GenTexture();
            _TextureUnit = textureUnit;

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
            GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, image.Width, image.Height, 0, PixelFormat.Rgba, PixelType.UnsignedByte, pixels.ToArray());
            //GL.GenerateMipmaps()
            UnBind();
        }

        public void Bind()
        {
            GL.ActiveTexture(_TextureUnit);
            GL.BindTexture(TextureTarget.Texture2D, _handle);
        }

        public void UnBind()
        {
            GL.BindTexture(TextureTarget.Texture2D, 0);
        }

        public void Dispose()
        {
            _disposed = true;
            GL.DeleteTexture(_handle);
        }
    }
}
