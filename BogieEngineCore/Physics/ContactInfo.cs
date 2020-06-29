
using BepuPhysics.Collidables;
namespace BogieEngineCore.Physics
{
    internal class ContactInfo
    {
        public int Handle;
        public CollidableMobility HandleMobility;

        public ContactInfo(int handle, CollidableMobility handleMobility)
        {
            Handle = handle;
            HandleMobility = handleMobility;
        }

        public override bool Equals(object obj)
        {
            return Handle == ((ContactInfo)obj).Handle && HandleMobility == ((ContactInfo)obj).HandleMobility;
        }

        public override int GetHashCode()
        {
            return Handle + (int)HandleMobility;
        }
    }
}
