using System;
using System.Collections.Generic;
using System.Numerics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using BepuPhysics.Collidables;
namespace BogieEngineCore
{
    internal static class BEPUTriangleExt
    {
        public static bool IsDegenerate(this Triangle triangle)
        {
            Vector3 v01 = triangle.A - triangle.B;
            Vector3 v12 = triangle.B - triangle.C;
            Vector3 v20 = triangle.C - triangle.A;

            if (v01.Length() - v12.Length() == v20.Length())
            {
                return true;
            }
            if (v12.Length() - v20.Length() == v01.Length())
            {
                return true;
            }
            if (v20.Length() - v01.Length() == v12.Length())
            {
                return true;
            }
            return false;
        }
    }
}
