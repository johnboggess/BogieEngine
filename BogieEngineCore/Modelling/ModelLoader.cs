using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel;

using Assimp;

using BogieEngineCore.Texturing;
using BogieEngineCore.Materials;
using BogieEngineCore.Vertices;

using OpenTK.Graphics.OpenGL;

namespace BogieEngineCore.Modelling
{
    /// <summary>
    /// Loads models
    /// </summary>
    internal class ModelLoader
    {
        static ContentManager _contentManager;
        static string _folder;
        static VertexDefinition _vertexDefinition;

        /// <summary>
        /// Loads the given file and assigns it the given shader.
        /// </summary>
        /// <param name="filePath">Path to the file.</param>
        /// <param name="contentManager">Content manager.</param>
        /// <param name="shader">The shader to assign to the model.</param>
        /// <returns>The loaded model.</returns>
        public static ModelData LoadModel<T>(string filePath, ContentManager contentManager, Shading.Shader shader, VertexDefinition vertexDefinition) where T : Materials.Material, new()
        {
            _contentManager = contentManager;
            _folder = filePath.Substring(0, filePath.LastIndexOf("/"));
            _vertexDefinition = vertexDefinition;
            AssimpContext assimpContext = new AssimpContext();
            Scene scene = assimpContext.ImportFile(filePath, PostProcessSteps.Triangulate | PostProcessSteps.GenerateNormals | PostProcessSteps.CalculateTangentSpace);

            return new ModelData(_processNode<T>(scene.RootNode, scene, shader));
        }

        private static List<MeshData> _processNode<T>(Node node, Scene scene, Shading.Shader shader) where T : Materials.Material, new()
        {
            List<MeshData> meshes = new List<MeshData>();
            for (int i = 0; i < node.MeshCount; i++)
            {
                Assimp.Mesh aiMesh = scene.Meshes[node.MeshIndices[i]];
                meshes.Add(ProcessMesh<T>(aiMesh, scene, shader));
            }

            foreach (Node child in node.Children)
            {
                meshes.AddRange(_processNode<T>(child, scene, shader));
            }
            return meshes;

        }

        private static MeshData ProcessMesh<T>(Assimp.Mesh aiMesh, Scene scene, Shading.Shader shader) where T : Materials.Material, new()
        {
            List<uint> indices = new List<uint>();
            float[] vertices = new float[aiMesh.VertexCount * _vertexDefinition.GetVertexSizeInBytes()];

            for (int i = 0; i < aiMesh.FaceCount; i++)
            {
                Face face = aiMesh.Faces[i];

                for (int j = 0; j < face.IndexCount; j++)
                {
                    int index = face.Indices[j];
                    float[] vertex = _vertexDefinition.CreateVertex(aiMesh, index);

                    Array.Copy(vertex, 0, vertices, index * _vertexDefinition.GetVertexSizeInFloats(), vertex.Length);
                    indices.Add((uint)index);
                }
            }
            
            Materials.Material meshMaterial = new T();
            meshMaterial.LoadFromMesh(aiMesh, scene, _contentManager, _folder);

            VertexBuffer vb = new VertexBuffer(vertices.ToArray(), _vertexDefinition);
            ElementBuffer eb = new ElementBuffer();
            eb.SetIndices(indices.ToArray());

            VertexArray vertexArray = new VertexArray();
            vertexArray.Setup(vb, eb);

            MeshData meshData = new MeshData(aiMesh.Name, vertexArray, meshMaterial, shader);
            return meshData;
        }
    }
}
