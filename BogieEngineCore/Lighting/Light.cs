using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using BogieEngineCore.Materials;
using BogieEngineCore.Shading;

namespace BogieEngineCore.Lighting
{
    public abstract class Light
    {
        public abstract void SetLightUniform(string materialName, Shader shader);

        public abstract Light Clone();
    }
}
