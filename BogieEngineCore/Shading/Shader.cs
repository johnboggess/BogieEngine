using OpenTK;
using OpenTK.Graphics.OpenGL4;
using System.IO;

namespace BogieEngineCore.Shading
{
    public class Shader : IDisposable
    {
        public static readonly int VertexPositionLocation = 0;
        public static readonly int VertexUVLocation = 1;
        public static readonly int VertexNormalLocation = 2;

        public string VertexShaderName { get { return _vertexShaderName; } }
        public string FragmentShaderName { get { return _fragmentShaderName; } }

        public BaseGame Game;

        public bool Disposed { get { return _disposed; } }


        int _handle;
        public string _vertexShaderName;
        public string _fragmentShaderName;

        private bool _disposed = false;

        public Shader(BaseGame game, string vertexPath, string fragmentPath)
        {
            Game = game;

            //Read source code from file
            int vertexHandle;
            string vertexSource = File.ReadAllText(vertexPath);
            _vertexShaderName = vertexPath.Substring(vertexPath.LastIndexOf("/") + 1, vertexPath.Length - vertexPath.LastIndexOf("/") - 1);

            int fragHandle;
            string fragSource = File.ReadAllText(fragmentPath);
            _fragmentShaderName = fragmentPath.Substring(fragmentPath.LastIndexOf("/") + 1, fragmentPath.Length - fragmentPath.LastIndexOf("/") - 1);

            ///Create shader and bind the source code to it
            vertexHandle = GL.CreateShader(ShaderType.VertexShader);
            GL.ShaderSource(vertexHandle, vertexSource);

            fragHandle = GL.CreateShader(ShaderType.FragmentShader);
            GL.ShaderSource(fragHandle, fragSource);

            //Compile the shaders and display any log information
            GL.CompileShader(vertexHandle);
            string log = GL.GetShaderInfoLog(vertexHandle);
            if (log != System.String.Empty) { System.Console.WriteLine("Vertex Shader:\n" + log); }

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
            GL.UseProgram(_handle); //must use the shader before uniforms can be set
            int uniformCount;
            int samplersFound = 0;
            GL.GetProgram(_handle, GetProgramParameterName.ActiveUniforms, out uniformCount);
            for (int i = 0; i < uniformCount; i++)
            {
                int nameLength;
                int size;
                ActiveUniformType activeUniformType;
                string name;
                GL.GetActiveUniform(_handle, i, 100, out nameLength, out size, out activeUniformType, out name);
                if (activeUniformType == ActiveUniformType.Sampler2D)
                {
                    int location = GL.GetUniformLocation(_handle, name);
                    GL.Uniform1(location, samplersFound);
                    samplersFound += 1;
                }
            }
        }

        public virtual void Use(params object[] values)
        {
            _Use();
        }

        public void Dispose()
        {
            _disposed = true;
            GL.DeleteProgram(_handle);
        }

        /// <summary>
        /// Set a uniform.
        /// </summary>
        /// <param name="name">The name of the uniform.</param>
        /// <param name="value">The value of a uniform. Must be a double, float, int, or uint.</param>
        public void SetUniform1(string name, object value)
        {
            int location = GL.GetUniformLocation(_handle, name);
            GL.UseProgram(_handle);
            if(value is double)
                GL.Uniform1(location, (double)value);
            if (value is float)
                GL.Uniform1(location, (float)value);
            if (value is int)
                GL.Uniform1(location, (int)value);
            if (value is uint)
                GL.Uniform1(location, (uint)value);
        }

        /// <summary>
        /// Set a vector2 uniform.
        /// </summary>
        /// <param name="name">The name of the uniform.</param>
        /// <param name="value">The value of a uniform.</param>
        public void SetUniform2(string name, Vector2 value)
        {
            int location = GL.GetUniformLocation(_handle, name);
            GL.UseProgram(_handle);
            GL.Uniform2(location, value);
        }

        /// <summary>
        /// Set a vector3 uniform.
        /// </summary>
        /// <param name="name">The name of the uniform.</param>
        /// <param name="value">The value of a uniform.</param>
        public void SetUniform3(string name, Vector3 value)
        {
            int location = GL.GetUniformLocation(_handle, name);
            GL.UseProgram(_handle);
            GL.Uniform3(location, value);
        }

        /// <summary>
        /// Set a vector4 uniform.
        /// </summary>
        /// <param name="name">The name of the uniform.</param>
        /// <param name="value">The value of a uniform.</param>
        public void SetUniform4(string name, Vector4 value)
        {
            int location = GL.GetUniformLocation(_handle, name);
            GL.UseProgram(_handle);
            GL.Uniform4(location, value);
        }

        /// <summary>
        /// Set a quaternion uniform.
        /// </summary>
        /// <param name="name">The name of the uniform.</param>
        /// <param name="value">The value of a uniform.</param>
        public void SetUniform4(string name, Quaternion value)
        {
            int location = GL.GetUniformLocation(_handle, name);
            GL.UseProgram(_handle);
            GL.Uniform4(location, value);
        }

        /// <summary>
        /// Set a quaternion uniform.
        /// </summary>
        /// <param name="name">The name of the uniform.</param>
        /// <param name="value">The value of a uniform. Must be Matrix2, Matrix3, Matrix4</param>
        public void SetUniformMatrix(string name, bool transpose, object value)
        {
            int location = GL.GetUniformLocation(_handle, name);
            GL.UseProgram(_handle);
            if (value is Matrix2)
            {
                Matrix2 matrix = (Matrix2)value;
                GL.UniformMatrix2(location, transpose, ref matrix);
            }
            else if (value is Matrix3)
            {
                Matrix3 matrix = (Matrix3)value;
                GL.UniformMatrix3(location, transpose, ref matrix);
            }
            else if (value is Matrix4)
            {
                Matrix4 matrix = (Matrix4)value;
                GL.UniformMatrix4(location, transpose, ref matrix);
            }
        }

        internal void _Use()
        {
            GL.UseProgram(_handle);
        }
    }
}
