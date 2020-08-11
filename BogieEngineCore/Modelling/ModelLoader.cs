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
        public static ModelData LoadModel(string filePath, ContentManager contentManager, Shading.Shader shader, VertexDefinition vertexDefinition)
        {
            _contentManager = contentManager;
            _folder = filePath.Substring(0, filePath.LastIndexOf("/"));
            _vertexDefinition = vertexDefinition;
            AssimpContext assimpContext = new AssimpContext();
            Scene scene = assimpContext.ImportFile(filePath, PostProcessSteps.Triangulate | PostProcessSteps.GenerateNormals | PostProcessSteps.CalculateTangentSpace);

            return new ModelData(_processNode(scene.RootNode, scene, shader));
        }

        private static List<MeshData> _processNode(Node node, Scene scene, Shading.Shader shader)
        {
            List<MeshData> meshes = new List<MeshData>();
            for (int i = 0; i < node.MeshCount; i++)
            {
                Assimp.Mesh aiMesh = scene.Meshes[node.MeshIndices[i]];
                meshes.Add(ProcessMesh(aiMesh, scene, shader));
            }

            foreach (Node child in node.Children)
            {
                meshes.AddRange(_processNode(child, scene, shader));
            }
            return meshes;

        }

        private static MeshData ProcessMesh(Assimp.Mesh aiMesh, Scene scene, Shading.Shader shader)
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

            BogieEngineCore.Materials.Material meshMaterial = null;

            if (aiMesh.MaterialIndex > -1)
            {
                Texture diffuseTexture = null;

                Assimp.Material material = scene.Materials[aiMesh.MaterialIndex];
                string diffusePath = material.TextureDiffuse.FilePath;
                if (diffusePath != null)
                {
                    diffuseTexture = _contentManager.LoadTexture(_folder + "/" + diffusePath, OpenTK.Graphics.OpenGL4.TextureUnit.Texture0);
                }

                meshMaterial = new PhongMaterial();
                ((PhongMaterial)meshMaterial).DiffuseTexture = diffuseTexture;
                ((PhongMaterial)meshMaterial).SpecularTexture = diffuseTexture;//todo: load specular texture.
                ((PhongMaterial)meshMaterial).Shininess = material.Shininess;
            }

            VertexBuffer vb = new VertexBuffer(vertices.ToArray(), _vertexDefinition);
            ElementBuffer eb = new ElementBuffer();
            eb.SetIndices(indices.ToArray());

            VertexArray vertexArray = new VertexArray();
            vertexArray.Setup(vb, eb);

            MeshData meshData = new MeshData(aiMesh.Name, vertexArray);

            /*MeshInstance mesh = new MeshInstance(meshData);
            mesh.Shader = shader;

            mesh.Material = meshMaterial;*/

            return meshData;
        }
    }
}
