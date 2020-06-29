namespace BogieEngineCore
{
    public class Utilities
    {
        public static System.Numerics.Vector3 ConvertVector3Type(OpenTK.Vector3 vec)
        {
            return new System.Numerics.Vector3(vec.X, vec.Y, vec.Z);
        }
        public static OpenTK.Vector3 ConvertVector3Type(System.Numerics.Vector3 vec)
        {
            return new OpenTK.Vector3(vec.X, vec.Y, vec.Z);
        }

        public static OpenTK.Quaternion ConvertQuaternionType(BepuUtilities.Quaternion quaternion)
        {
            return new OpenTK.Quaternion(quaternion.X, quaternion.Y, quaternion.Z, quaternion.W);
        }
        public static BepuUtilities.Quaternion ConvertQuaternionType(OpenTK.Quaternion quaternion)
        {
            return new BepuUtilities.Quaternion(quaternion.X, quaternion.Y, quaternion.Z, quaternion.W);
        }
    }
}
