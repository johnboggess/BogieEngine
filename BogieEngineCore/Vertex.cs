
using OpenTK;
namespace BogieEngineCore
{
    /// <summary>
    /// Contains attributes of a vertex, such as position and UV
    /// </summary>
    struct Vertex
    {
        /// <summary>
        /// Size in bytes of the vertex
        /// </summary>
        public static readonly int Size = 8 * sizeof(float);

        internal Vector3 _Position;
        internal Vector2 _UV;
        internal Vector3 _Normal;

        public Vertex(Vector3 position, Vector2 uv, Vector3 _normal)
        {
            _Position = position;
            _UV = uv;
            _Normal = _normal;
        }
    }
}
