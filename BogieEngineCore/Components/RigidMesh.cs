using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;

using BogieEngineCore.Modelling;

using BepuUtilities.Memory;
using BepuPhysics.Collidables;
using BepuPhysics;

namespace BogieEngineCore.Components
{
    /// <summary>
    /// Can cause crashes if the mesh contains degenerate triangles or is non-manifold
    /// </summary>
    public class RigidMesh : RigidBody
    {
        private MeshData _meshData;

        public RigidMesh(MeshData meshData, float x, float y, float z, float widthX, float heightY, float lengthZ, bool recordContacts) : base(x, y, z, widthX, heightY, lengthZ, recordContacts, nameof(RigidBox), meshData)
        {
        }
        public RigidMesh(MeshData meshData, Vector3 position, Vector3 scale, bool recordContacts) : base(position, scale, recordContacts, nameof(RigidMesh), meshData)
        {
        }
        public RigidMesh(MeshData meshData, Transform transform, bool recordContacts) : base(transform, recordContacts, nameof(RigidMesh), meshData)
        {
        }

        protected override void _SetupCollider(float x, float y, float z, float scaleX, float scaleY, float scaleZ, object meshData)
        {
            _meshData = (MeshData)meshData;

            List<Triangle> triangles = _meshData._VertexArray.Triangles(true, true, true);//the winding order for bepu is the other direction than the opengl winding order

            Buffer<Triangle> buffer;
            BaseGame.GlobalGame._GamePhysics._BufferPool.Take<Triangle>(triangles.Count, out buffer);

            for(int i = 0; i < triangles.Count; i++)
            {
                buffer[i] = triangles[i];
            }

            Mesh mesh = new Mesh(buffer, new Vector3(scaleX, scaleY, scaleZ), BaseGame.GlobalGame._GamePhysics._BufferPool);
            BodyInertia bodyInertia;
            Vector3 center;
            mesh.ComputeClosedInertia(1, out bodyInertia, out center);
            BodyDescription bodyDescription = BodyDescription.CreateDynamic(
                new System.Numerics.Vector3(x, y, z),
                bodyInertia,
                new CollidableDescription(BaseGame.GlobalGame._PhysicsSimulation.Shapes.Add(mesh), 0.1f),
                new BodyActivityDescription(0.01f));

            //todo: get this to work. or remove
            //bodyDescription.Pose.Orientation = Utilities.ConvertQuaternionType(_Entity.LocalTransform.Quaternion);

            _BodyHandle = BaseGame.GlobalGame._PhysicsSimulation.Bodies.Add(bodyDescription);
            _BodyReference = BaseGame.GlobalGame._PhysicsSimulation.Bodies.GetBodyReference(_BodyHandle);
            _IsBody = true;
        }
    }
}
