using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Assimp;

using BogieEngineCore.Texturing;
namespace BogieEngineCore.Modelling
{
    /// <summary>
    /// Loads models
    /// </summary>
    internal class ModelLoader
    {
        static ContentManager _contentManager;
        static string _folder;

        /// <summary>
        /// Loads the given file and assign it the given shader.
        /// </summary>
        /// <param name="filePath">Path to the file.</param>
        /// <param name="contentManager">Content manager.</param>
        /// <param name="shader">The shader to assign to the model.</param>
        /// <returns>The loaded model.</returns>
        public static Model LoadModel(string filePath, ContentManager contentManager, Shading.Shader shader)
        {
            _contentManager = contentManager;
            _folder = filePath.Substring(0,filePath.LastIndexOf("/"));
            AssimpContext assimpContext = new AssimpContext();
            Scene scene = assimpContext.ImportFile(filePath, PostProcessSteps.Triangulate | PostProcessSteps.GenerateNormals);

            return new Model(_processNode(scene.RootNode, scene, shader));
        }

        private static List<MeshData> _processNode(Node node, Scene scene, Shading.Shader shader)
        {
            List<MeshData> meshData = new List<MeshData>();
            for(int i = 0; i < node.MeshCount; i++)
            {
                Assimp.Mesh aiMesh = scene.Meshes[node.MeshIndices[i]];
                meshData.Add(ProcessMesh(aiMesh, scene, shader));
            }

            foreach(Node child in node.Children)
            {
                meshData.AddRange(_processNode(child, scene, shader));
            }
            return meshData;

        }

        private static MeshData ProcessMesh(Assimp.Mesh aiMesh, Scene scene, Shading.Shader shader)
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

            List<Texture> textures = new List<Texture>();
            if(aiMesh.MaterialIndex > 0)
            {
                Assimp.Material material = scene.Materials[aiMesh.MaterialIndex];

                string diffusePath = material.TextureDiffuse.FilePath;
                if (diffusePath != null)
                {
                    textures.Add(_contentManager.LoadTexture(_folder+"/" + diffusePath, OpenTK.Graphics.OpenGL4.TextureUnit.Texture0));
                }
            }

            VertexBuffer vb = new VertexBuffer();
            vb.SetVertices(vertices.ToArray());
            ElementBuffer eb = new ElementBuffer();
            eb.SetIndices(indices.ToArray());

            VertexArray vertexArray = new VertexArray();
            vertexArray.Setup(vb, eb);

            Mesh mesh = new Mesh(aiMesh.Name, vertexArray);

            MeshData meshData = new MeshData(mesh);
            meshData.Shader = shader;
            meshData.Textures = textures;

            return meshData;
        }

    }
}
