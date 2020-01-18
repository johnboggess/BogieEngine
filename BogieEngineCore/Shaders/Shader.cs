using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

using OpenTK.Graphics.OpenGL4;

namespace BogieEngineCore.Shaders
{
    public class Shader : IDisposable
    {
        public bool Disposed { get { return _disposed; } }
        private bool _disposed = false;

        public static readonly int VertexPositionLocation = 0;
        public static readonly int VertexUVLocation = 1;

        private bool disposedValue = false;

        int _handle;
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
            if(log != System.String.Empty) { System.Console.WriteLine("Vertex Shader:\n"+log); }

            GL.CompileShader(fragHandle);
            log = GL.GetShaderInfoLog(fragHandle);
            if (log != System.String.Empty) { System.Console.WriteLine("Fragment Shader:\n" + log); }

            //Create the shader program and link the vertex and frag shaders
            _handle = GL.CreateProgram();
            GL.AttachShader(_handle, vertexHandle);
            GL.AttachShader(_handle, fragHandle);
            GL.LinkProgram(_handle);

            //clean up
            GL.DetachShader(_handle, vertexHandle);
            GL.DetachShader(_handle, fragHandle);
            GL.DeleteShader(vertexHandle);
            GL.DeleteShader(fragHandle);

            //Set the texture units of the samplers in the order they appear
            Use(); //must use the shader before uniforms can be set
            int uniformCount;
            int samplersFound = 0;
            GL.GetProgram(_handle, GetProgramParameterName.ActiveUniforms, out uniformCount);
            for(int i = 0; i < uniformCount; i++)
            {
                int nameLength;
                int size;
                ActiveUniformType activeUniformType;
                string name;
                GL.GetActiveUniform(_handle, i, 100, out nameLength, out size, out activeUniformType, out name);
                if(activeUniformType == ActiveUniformType.Sampler2D)
                {
                    int location = GL.GetUniformLocation(_handle, name);
                    GL.Uniform1(location, samplersFound);
                    samplersFound += 1;
                }
            }
        }

        public void Use()
        {
            GL.UseProgram(_handle);
        }


        public void Dispose()
        {
            _disposed = true;
            GL.DeleteProgram(_handle);
        }
    }
}
