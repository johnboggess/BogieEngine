using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Assimp;
namespace BogieEngineCore.Modelling
{
    internal class ModelLoader
    {
        public static Model LoadModel(string filePath)
        {
            AssimpContext assimpContext = new AssimpContext();
            Scene scene = assimpContext.ImportFile(filePath, PostProcessSteps.Triangulate | PostProcessSteps.FlipUVs | PostProcessSteps.GenerateNormals);

            return new Model(ProcessNode(scene.RootNode, scene));
        }

        private static List<Mesh> ProcessNode(Node node, Scene scene)
        {
            List<Mesh> meshes = new List<Mesh>();
            for(int i = 0; i < node.MeshCount; i++)
            {
                Assimp.Mesh aiMesh = scene.Meshes[node.MeshIndices[i]];
                meshes.Add(ProcessMesh(aiMesh, scene));
            }

            foreach(Node child in node.Children)
            {
                meshes.AddRange(ProcessNode(child, scene));
            }
            return meshes;

        }

        private static Mesh ProcessMesh(Assimp.Mesh aiMesh, Scene scene)
        {
            List<uint> indices = new List<uint>();
            Vertex[] vertices = new Vertex[aiMesh.VertexCount];

            for (int i = 0; i < aiMesh.FaceCount; i++)
            {
                Face face = aiMesh.Faces[i];

                for (int j = 0; j < face.IndexCount; j++)
                {
                    int index = face.Indices[j];
                    indices.Add((uint)index);

                    Vector3D pos = aiMesh.Vertices[index];
                    //Vector3D norm = aiMesh.Normals[index];
                    Vector3D UV = new Vector3D(0, 0, 0);

                    if (aiMesh.HasTextureCoords(0))
                    {
                        UV = aiMesh.TextureCoordinateChannels[0][index];
                    }

                    Vertex vertex = new Vertex(new OpenTK.Vector3(pos.X, pos.Y, pos.Z), new OpenTK.Vector2(UV.X, UV.Y));
                    vertices[index] = vertex;
                }
            }

            VertexBuffer vb = new VertexBuffer();
            vb.SetVertices(vertices.ToArray());
            ElementBuffer eb = new ElementBuffer();
            eb.SetIndices(indices.ToArray());

            VertexArray vertexArray = new VertexArray();
            vertexArray.Setup(vb, eb);

            return new Mesh(vertexArray);
        }

    }
}
