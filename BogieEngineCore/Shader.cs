using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

using OpenTK.Graphics.OpenGL4;
namespace BogieEngineCore
{
    public class Shader : IDisposable
    {
        public static readonly int VertexPositionLocation = 0;

        private bool disposedValue = false;

        int Handle;
        public Shader(string vertexPath, string fragmentPath)
        {
            //Read source code from file
            int vertexHandle;
            string vertexSource = File.ReadAllText(vertexPath);

            int fragHandle;
            string fragSource = File.ReadAllText(fragmentPath);

            ///Create shader and bind the source code to it
            vertexHandle = GL.CreateShader(ShaderType.VertexShader);
            GL.ShaderSource(vertexHandle, vertexSource);

            fragHandle = GL.CreateShader(ShaderType.FragmentShader);
            GL.ShaderSource(fragHandle, fragSource);

            //Compile the shaders and display any log information
            GL.CompileShader(vertexHandle);
            string log = GL.GetShaderInfoLog(vertexHandle);
            if(log != System.String.Empty) { System.Console.WriteLine(log); }

            GL.CompileShader(fragHandle);
            log = GL.GetShaderInfoLog(fragHandle);
            if (log != System.String.Empty) { System.Console.WriteLine(log); }

            //Create the shader program and link the vertex and frag shaders
            Handle = GL.CreateProgram();
            GL.AttachShader(Handle, vertexHandle);
            GL.AttachShader(Handle, fragHandle);
            GL.LinkProgram(Handle);

            //clean up
            GL.DetachShader(Handle, vertexHandle);
            GL.DetachShader(Handle, fragHandle);
            GL.DeleteShader(vertexHandle);
            GL.DeleteShader(fragHandle);
        }

        public void Use()
        {
            GL.UseProgram(Handle);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                GL.DeleteProgram(Handle);

                disposedValue = true;
            }
        }

        ~Shader()
        {
            GL.DeleteProgram(Handle);
        }
    }
}
