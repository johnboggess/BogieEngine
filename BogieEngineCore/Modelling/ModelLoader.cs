﻿using System;
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

        /// <summary>
        /// Loads the given file and assigns it the given shader.
        /// </summary>
        /// <param name="filePath">Path to the file.</param>
        /// <param name="contentManager">Content manager.</param>
        /// <param name="shader">The shader to assign to the model.</param>
        /// <returns>The loaded model.</returns>
        public static Model LoadModel(string filePath, ContentManager contentManager, Shading.Shader shader, Type vertexType)
        {
            _contentManager = contentManager;
            _folder = filePath.Substring(0, filePath.LastIndexOf("/"));
            AssimpContext assimpContext = new AssimpContext();
            Scene scene = assimpContext.ImportFile(filePath, PostProcessSteps.Triangulate | PostProcessSteps.GenerateNormals);

            return new Model(_processNode(scene.RootNode, scene, shader));
        }

        private static List<MeshInstance> _processNode(Node node, Scene scene, Shading.Shader shader)
        {
            List<MeshInstance> meshes = new List<MeshInstance>();
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

        private static MeshInstance ProcessMesh(Assimp.Mesh aiMesh, Scene scene, Shading.Shader shader)
        {
            List<uint> indices = new List<uint>();
            DefaultVertex[] vertices = new DefaultVertex[aiMesh.VertexCount];

            for (int i = 0; i < aiMesh.FaceCount; i++)
            {
                Face face = aiMesh.Faces[i];

                for (int j = 0; j < face.IndexCount; j++)
                {
                    int index = face.Indices[j];
                    DefaultVertex defaultVertex = new DefaultVertex();
                    defaultVertex.FillData(aiMesh, index);

                    vertices[index] = defaultVertex;
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

            VertexBuffer<DefaultVertex> vb = new VertexBuffer<DefaultVertex>();
            vb.SetVertices(vertices.ToArray());
            ElementBuffer eb = new ElementBuffer();
            eb.SetIndices(indices.ToArray());

            VertexArray<DefaultVertex> vertexArray = new VertexArray<DefaultVertex>();
            vertexArray.Setup(vb, eb);

            MeshData meshData = new MeshData(aiMesh.Name, vertexArray);

            MeshInstance mesh = new MeshInstance(meshData);
            mesh.Shader = shader;

            mesh.Material = meshMaterial;

            return mesh;
        }

    }
}
