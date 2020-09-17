using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using BogieEngineCore.Shading;

using Assimp;

using OpenTK;
using OpenTK.Graphics.OpenGL4;

namespace BogieEngineCore.Vertices
{
    /// <summary>
    /// Defines vertices with a position, UV cords, and normal
    /// </summary>
    public class DefaultVertexDefinition : VertexDefinition
    {
        /// <summary>
        /// Size in bytes of the vertex
        /// </summary>
        public override int GetVertexSizeInBytes()
        {
            return GetVertexSizeInFloats() * sizeof(float);
        }

        public override int GetVertexSizeInFloats()
        {
            return 8;
        }

        public override void SetUpVertexAttributePointers()
        {
            GL.VertexAttribPointer(Shader.VertexPositionLocation, 3, VertexAttribPointerType.Float, false, GetVertexSizeInBytes(), 0);
            GL.EnableVertexAttribArray(Shader.VertexPositionLocation);

            GL.VertexAttribPointer(Shader.VertexUVLocation, 2, VertexAttribPointerType.Float, false, GetVertexSizeInBytes(), 3 * sizeof(float));
            GL.EnableVertexAttribArray(Shader.VertexUVLocation);

            GL.VertexAttribPointer(Shader.VertexNormalLocation, 3, VertexAttribPointerType.Float, false, GetVertexSizeInBytes(), 5 * sizeof(float));
            GL.EnableVertexAttribArray(Shader.VertexNormalLocation);
        }

        public override float[] CreateVertex(Mesh mesh, int index)
        {
            float[] result = new float[GetVertexSizeInFloats()];
            result[0] = mesh.Vertices[index].X;
            result[1] = mesh.Vertices[index].Y;
            result[2] = mesh.Vertices[index].Z;

            result[3] = mesh.TextureCoordinateChannels[0][index].X;
            result[4] = mesh.TextureCoordinateChannels[0][index].Y;

            result[5] = mesh.Normals[index].X;
            result[6] = mesh.Normals[index].Y;
            result[7] = mesh.Normals[index].Z;

            return result;
        }

        internal override System.Numerics.Vector3 _GetPosition(float[] vertices, int vertexIndex)
        {
            int i = GetVertexSizeInFloats() * vertexIndex;
            System.Numerics.Vector3 v = new System.Numerics.Vector3();
            v.X = vertices[i + 0];
            v.Y = vertices[i + 1];
            v.Z = vertices[i + 2];
            return v;
        }
    }
}
