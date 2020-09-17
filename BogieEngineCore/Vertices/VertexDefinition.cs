using System;
using System.Numerics;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Assimp;
namespace BogieEngineCore.Vertices
{
    public abstract class VertexDefinition
    {
        public abstract int GetVertexSizeInBytes();
        public abstract int GetVertexSizeInFloats();
        public abstract float[] CreateVertex(Mesh mesh, int index);
        public abstract void SetUpVertexAttributePointers();

        internal abstract Vector3 _GetPosition(float[] vertices, int vertexIndex);
    }
}
