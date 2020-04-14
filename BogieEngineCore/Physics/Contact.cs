using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using BepuPhysics.Collidables;
namespace BogieEngineCore.Physics
{
    internal class Contact
    {
        public int Handle1;
        public CollidableMobility Handle1Mobility;
        public int Handle2;
        public CollidableMobility Handle2Mobility;

        public Contact(int handle1, CollidableMobility handle1Mobility, int handle2, CollidableMobility handle2Mobility)
        {
            Handle1 = handle1;
            Handle2 = handle2;

            Handle1Mobility = handle1Mobility;
            Handle2Mobility = handle2Mobility;
        }
    }
}
