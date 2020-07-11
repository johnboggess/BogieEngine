using System;
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
    }
}
