using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using BogieEngineCore.Shading;

using Assimp;

namespace BogieEngineCore.Materials
{
    public abstract class Material
    {
        public abstract void LoadFromMesh(Mesh mesh, Scene scene, ContentManager contentManager, string folder);
        public abstract void SetMaterialUniform(string materialName, Shader shader);
        public abstract Material Clone();
    }
}
