﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using OpenTK.Graphics.OpenGL4;

using BogieEngineCore.Shaders;
using BogieEngineCore.Textures;

namespace BogieEngineCore
{
    class VertexArray
    {
        int _handle;
        VertexBuffer _vbo;
        ElementBuffer _ebo;
        List<Texture> _textures;

        public VertexArray()
        {
            _handle = GL.GenVertexArray();
        }

        public void Setup(Vertex[] vertices, uint[] indices, List<Texture> textures)
        {
            _vbo = new VertexBuffer();
            _vbo.SetVertices(vertices);
            _ebo = new ElementBuffer();
            _ebo.SetIndices(indices);

            Setup(_vbo, _ebo, textures);
        }

        public void Setup(VertexBuffer vbo, ElementBuffer ebo, List<Texture> textures)
        {
            _ebo = ebo;
            _vbo = vbo;
            _textures = textures;
            Bind();

            vbo.Bind();
            GL.VertexAttribPointer(Shader.VertexPositionLocation, 3, VertexAttribPointerType.Float, false, Vertex.Size, 0);
            GL.EnableVertexAttribArray(Shader.VertexPositionLocation);
            GL.VertexAttribPointer(Shader.VertexUVLocation, 2, VertexAttribPointerType.Float, false, Vertex.Size, 3 * sizeof(float));
            GL.EnableVertexAttribArray(Shader.VertexUVLocation);

            ebo.Bind();

            UnBind();

        }

        public void Bind()
        {
            GL.BindVertexArray(_handle);
        }

        public void BindTextures()
        {
            foreach (Texture texture in _textures)
                texture.Bind();
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
            _vbo.Dispose();
            _ebo.Dispose();
            foreach (Texture texture in _textures)
                texture.Dispose();
            GL.DeleteBuffer(_handle);
        }

    }
}
