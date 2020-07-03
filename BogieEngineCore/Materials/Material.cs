using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using BogieEngineCore.Shading;

namespace BogieEngineCore.Materials
{
    public abstract class Material
    {
        public abstract void SetMaterialUniform(string materialName, Shader shader);

        public abstract Material Clone();
    }
}
