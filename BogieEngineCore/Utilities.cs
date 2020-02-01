using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using OpenTK;
using Jitter;
namespace BogieEngineCore
{
    public class Utilities
    {
        public static Jitter.LinearMath.JVector OpenTKVectorToJVector(OpenTK.Vector3 vector)
        {
            return new Jitter.LinearMath.JVector(vector.X, vector.Y, vector.Z);
        }
    }
}
