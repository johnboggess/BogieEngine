using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using OpenTK;
namespace BogieEngineCore
{
    struct Vertex
    {
        public static readonly int Size = 5 * sizeof(float);

        Vector3 _position;
        Vector2 _uV;

        public Vertex(Vector3 position, Vector2 uv)
        {
            _position = position;
            _uV = uv;
        }
    }
}
