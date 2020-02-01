using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BogieEngineCore
{
    public class Utilities
    {
        public static System.Numerics.Vector3 TKVector3ToSystemVector3(OpenTK.Vector3 vec)
        {
            return new System.Numerics.Vector3(vec.X, vec.Y, vec.Z);
        }
        public static OpenTK.Vector3 SystemVector3ToTKVector3(System.Numerics.Vector3 vec)
        {
            return new OpenTK.Vector3(vec.X, vec.Y, vec.Z);
        }
    }
}
