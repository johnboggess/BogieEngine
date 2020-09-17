using System;
using System.Numerics;
using System.Collections.Generic;

using BogieEngineCore.Shading;
using BogieEngineCore.Vertices;

using BepuPhysics.Collidables;

using OpenTK.Graphics.OpenGL4;
namespace BogieEngineCore
{
    /// <summary>
    /// GPU array representing the vertices for a mesh, attributes of each vertex, and the order the vertices should be rendered in
    /// </summary>
    internal class VertexArray : IDisposable
    {
        public bool Disposed { get { return _disposed; } }

        bool _disposed = false;
        int _handle;

        VertexBuffer _vbo;
        ElementBuffer _ebo;

        public VertexArray()
        {
            _handle = GL.GenVertexArray();
        }

        public void Setup(VertexBuffer vbo, ElementBuffer ebo)
        {
            _ebo = ebo;
            _vbo = vbo;
            Bind();

            vbo.Bind();

            vbo.SetUpVertexAttributePointers();

            ebo.Bind();

            UnBind();

        }

        public List<Triangle> Triangles(bool reverseWindingOrder = false, bool ignoreDuplicates = false, bool removeDegenerateTriangles = false)
        {
            Dictionary<Triangle, Triangle> triangles = new Dictionary<Triangle, Triangle>();
            List<Triangle> result = new List<Triangle>();

            if (!reverseWindingOrder)
            {
                for (int i = 0; i < _ebo.Indices.Length; i += 3)
                {
                    Vector3 v0 = _vbo.GetPosition(i + 0);
                    Vector3 v1 = _vbo.GetPosition(i + 1);
                    Vector3 v2 = _vbo.GetPosition(i + 2);

                    Triangle triangle = new Triangle(in v0, in v1, in v2);
                    if (removeDegenerateTriangles && triangle.IsDegenerate())
                    {
                        continue;
                    }

                    if (!ignoreDuplicates)
                        result.Add(triangle);
                    else if (ignoreDuplicates && !triangles.ContainsKey(triangle))
                        result.Add(triangle);
                }
            }
            else
            {
                for (int i = 0; i < _ebo.Indices.Length; i += 3)
                {
                    Vector3 v0 = _vbo.GetPosition(i + 2);
                    Vector3 v1 = _vbo.GetPosition(i + 1);
                    Vector3 v2 = _vbo.GetPosition(i + 0);

                    Triangle triangle = new Triangle(in v0, in v1, in v2);
                    if(removeDegenerateTriangles && triangle.IsDegenerate())
                    {
                        continue;
                    }

                    if (!ignoreDuplicates)
                        result.Add(triangle);
                    else if (ignoreDuplicates && !triangles.ContainsKey(triangle))
                        result.Add(triangle);
                }
            }

            return result;
        }

        public void Bind()
        {
            GL.BindVertexArray(_handle);
        }

        public void UnBind()
        {
            GL.BindVertexArray(0);
        }

        public void Draw()
        {
            GL.DrawElements(PrimitiveType.Triangles, _ebo.Indices.Length, DrawElementsType.UnsignedInt, 0);
        }

        public void Dispose()
        {
            _disposed = true;
            GL.DeleteBuffer(_handle);
        }
    }
}
