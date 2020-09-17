using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using BogieEngineCore.Entities;
using BogieEngineCore.Modelling;
using BogieEngineCore.Shading;
using BogieEngineCore.Vertices;

using SixLabors.ImageSharp.Metadata.Profiles.Icc;

using OpenTK;

namespace BogieEngineCore.Components
{
    public class Model : Component
    {
        public ModelInstance ModelInstance { get { return _modelInstance; } }
        ModelInstance _modelInstance;

        public Model(ModelInstance modelInstance)
        {
            _modelInstance = modelInstance;
        }

        public override void EventInvoked(string evnt, params object[] eventArgs)
        {
            if (evnt == Component.RenderEvent)
                _draw(Entity.GlobalTransform.GetMatrix4());
        }

        public List<MeshInstance> GetMeshWithName(string name)
        {
            return _modelInstance.GetMeshWithName(name);
        }

        private void _draw(Matrix4 matrix)
        {
            _modelInstance.Draw(matrix);
        }
    }
}
