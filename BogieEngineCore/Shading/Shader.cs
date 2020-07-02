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

        public Matrix4 Projection
        {
            get { return _projection; }
            set
            {
                _projection = value;
                GL.UseProgram(_handle);
                GL.UniformMatrix4(projectionLocation, false, ref _projection);

            }
        }

        public Matrix4 View
        {
            get { return _view; }
            set
            {
                _view = value;
                GL.UseProgram(_handle);
                GL.UniformMatrix4(viewLocation, false, ref _view);

            }
        }

        public bool Disposed { get { return _disposed; } }


        int _handle;
        public string _vertexShaderName;
        public string _fragmentShaderName;
        Matrix4 _projection;
        Matrix4 _view;
        private bool _disposed = false;

        int projectionLocation;
        int viewLocation;
        int modelLocation;

        public Shader(string vertexPath, string fragmentPath)
        {
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

            projectionLocation = GL.GetUniformLocation(_handle, "projection");
            viewLocation = GL.GetUniformLocation(_handle, "view");
            modelLocation = GL.GetUniformLocation(_handle, "model");
        }

        public void Use(Matrix4 model)
        {
            GL.UseProgram(_handle);
            GL.UniformMatrix4(modelLocation, false, ref model);
        }

        public void Dispose()
        {
            _disposed = true;
            GL.DeleteProgram(_handle);
        }
    }
}
