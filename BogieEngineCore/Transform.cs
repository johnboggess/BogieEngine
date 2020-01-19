using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using OpenTK;
namespace BogieEngineCore
{
    public class Transform
    {
        public Vector3 Forwards = -Vector3.UnitZ;
        public Vector3 Up = Vector3.UnitY;
        public Vector3 Right = Vector3.UnitX;
        public Vector3 Position = Vector3.Zero;

        public Vector3 XAxis { get => Right; set => Right = value; }
        public Vector3 YAxis { get => Up; set => Up = value; }
        public Vector3 ZAxis { get => Forwards; set => Forwards = value; }

        public Matrix4 GetMatrix4()
        {
            return new Matrix4(new Vector4(Right, 0), new Vector4(Up, 0), new Vector4(-Forwards, 0), new Vector4(Position, 1));
        }
        
        public void Rotate(Vector3 axis, float radians)
        {
            Matrix3 rot = Matrix3.CreateFromAxisAngle(axis, radians);
            Forwards = rot * Forwards;
            Up = rot * Up;
            Right = rot * Right;
        }

        public void Scale(Vector3 Scale)
        {
            Forwards = Forwards * Scale.Z;
            Up = Up * Scale.Y;
            Right = Right * Scale.X;
        }
    }
}
