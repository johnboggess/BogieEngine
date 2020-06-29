using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BogieEngineCore.Entities;
using BogieEngineCore.Modelling;
using BogieEngineCore.Shading;
using SixLabors.ImageSharp.Metadata.Profiles.Icc;

namespace BogieEngineCore.Components
{
    public class Model : Component
    {
        Modelling.Model _model;
        public Model() { }

        public static Model CreateModel(string filePath, ContentManager contentManager, Shader shader, string name = nameof(Model), Entity entity = null)
        {
            Model modelComponent = new Model();
            modelComponent.Name = name;
            modelComponent._model = contentManager.LoadModel(filePath, shader);
            if (entity != null)
                modelComponent.QueueAttachToEntity(entity);
            return modelComponent;
        }

        public override void EventInvoked(string evnt, params object[] eventArgs)
        {
            if (evnt == Component.RenderEvent)
                _model.Draw(Entity.GlobalTransform.GetMatrix4());
        }

        /// <summary>
        /// Find meshes with the given name.
        /// </summary>
        /// <param name="name">The name of the meshes to search for.</param>
        /// <returns>All the meshes matching the given name.</returns>
        public List<MeshInstance> GetMeshWithName(string name)
        {
            return _model.GetMeshWithName(name);
        }

        /// <summary>
        /// Find the mesh with the given index.
        /// </summary>
        /// <param name="index">Index of the mesh</param>
        /// <returns>Mesh at the index</returns>
        public MeshInstance GetMesh(int index)
        {
            return _model.GetMesh(index);
        }
    }
}
