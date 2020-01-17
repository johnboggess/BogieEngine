﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using OpenTK.Graphics.OpenGL4;

namespace BogieEngineCore
{
    internal class VertexBuffer
    {
        int _handle;
        private Vector3[] vertices;
        public Vector3[] Vertice { get { return vertices; } }

        public VertexBuffer()
        {
            _handle = GL.GenBuffer();
        }

        public void SetVertices(Vector3[] vertices)
        {
            this.vertices = vertices;
            Bind();
            GL.BufferData(BufferTarget.ArrayBuffer, vertices.Length * sizeof(float) * 3, vertices, BufferUsageHint.StaticDraw);
            UnBind();
        }

        public void Bind()
        {
            GL.BindBuffer(BufferTarget.ArrayBuffer, _handle);
        }

        public void UnBind()
        {
            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
        }

        public void Dispose()
        {
            GL.DeleteBuffer(_handle);
        }
    }
}
