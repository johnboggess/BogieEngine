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
        public Quaternion _Quaternion = new Quaternion(0,0,0);

        public Vector3 Forwards
        {
            get { return (_Quaternion * Vector3.UnitZ) * Scale.Z; }
        }
        public Vector3 Up
        {
            get { return (_Quaternion * Vector3.UnitY) * Scale.Y; }
        }
        public Vector3 Right
        {
            get { return (_Quaternion * Vector3.UnitX) * Scale.X; }
        }

        public Vector3 XAxis { get => Right; }
        public Vector3 YAxis { get => Up; }
        public Vector3 ZAxis { get => Forwards; }

        public Vector3 Position = Vector3.Zero;
        public Vector3 Scale = new Vector3(1);

        public Matrix4 GetMatrix4()
        {
            return new Matrix4(new Vector4(Right, 0), new Vector4(Up, 0), new Vector4(Forwards, 0), new Vector4(Position, 1));
        }

        public void FromMatrix4(Matrix4 matrix)
        {
            Scale = matrix.ExtractScale();
            Position = matrix.ExtractTranslation();
            _Quaternion = matrix.ExtractRotation();

            //Right = matrix.Row0.Xyz;
            //Up = matrix.Row1.Xyz;
            //Forwards = matrix.Row2.Xyz;
        }

        /// <summary>
        /// Set the rotation of the transform.
        /// </summary>
        /// <param name="eulerAngles">The rotation about the X, Y, and Z axis.</param>
        public void SetRotation(Vector3 eulerAngles)
        {
            _Quaternion = new Quaternion(eulerAngles);
        }

        /// <summary>
        /// Rotate the transform by the given Euler angles.
        /// </summary>
        /// <param name="eulerAngles">The amount to rotate around the X, Y, and Z axis by </param>
        public void Rotate(Vector3 eulerAngles)
        {
            _Quaternion = new Quaternion(eulerAngles) * _Quaternion;
        }

        /// <summary>
        /// Rotate the transform about an arbitrary axis.
        /// </summary>
        /// <param name="axis">The axis to rotate the transform round.</param>
        /// <param name="radians"> How far to rotate the transform.</param>
        public void Rotate(Vector3 axis, float radians)
        {
            _Quaternion = Quaternion.FromAxisAngle(axis, radians) * _Quaternion;
        }

        /// <summary>
        /// Scales the transform by the given scale vector.
        /// </summary>
        /// <param name="scale">Multiples the X, Y and Z axis of the transform by the X, Y and Z elements of the scale.</param>
        public void ScaleBy(Vector3 scale)
        {
            Scale.Z = Scale.Z * scale.Z;
            Scale.Y = Scale.Y * scale.Y;
            Scale.X = Scale.X * scale.X;
        }

        /// <summary>
        /// Calculates the angle between two vectors on the place defined by the vectors.
        /// </summary>
        /// <param name="origin">The vector from which the angle is calculated.</param>
        /// <param name="dest">The vector to which the angle is calculated.</param>
        /// <returns>The angle between origin and dest on the plane defined by the vectors.</returns>
        public static float Rotation(Vector3 origin, Vector3 dest)
        {
            Vector3 nOrigin = origin.Normalized();
            Vector3 nDest = dest.Normalized();
            return (float)Math.Acos(Vector3.Dot(nDest, nOrigin));
        }

        /// <summary>
        /// Calculates the angle (pi to -pi) between two vectors on the plane defined by the given normal.
        /// </summary>
        /// <param name="origin">The vector from which the angle is calculated.</param>
        /// <param name="dest">The vector to which the angle is calculated.</param>
        /// <param name="norm">Normal defining the plane. Should be normalized when passed into method.</param>
        /// <returns></returns>
        public static float Rotation(Vector3 origin, Vector3 dest, Vector3 norm)
        {
            Vector3 projOrigin = Project(origin, norm);//origin - (Vector3.Dot(origin, norm) * norm);
            Vector3 projDest = Project(dest, norm);//dest - (Vector3.Dot(dest, norm) * norm);
            projOrigin.Normalize();
            projDest.Normalize();
            float dot = Vector3.Dot(projDest, projOrigin);
            float det = Vector3.Dot(norm, Vector3.Cross(projDest, projOrigin));
            return (float)Math.Atan2(det, dot);
        }

        /// <summary>
        /// Calculates the angle (pi to -pi) of the given vector to the plane defined by the given normal.
        /// </summary>
        /// <param name="vector">The vector to which the angle will be calculated.</param>
        /// <param name="norm">The normal defining the plane from which the angle will be calculated.</param>
        /// <returns>The angle between the vector and the plane.</returns>
        public static float RotationToPlane(Vector3 vector, Vector3 norm)
        {
            Vector3 projVector = Project(vector, norm);
            projVector.Normalize();
            Vector3 nVector = vector.Normalized();

            float dot = Vector3.Dot(nVector, projVector);
            float det = Vector3.Dot(Vector3.Cross(projVector,norm), Vector3.Cross(nVector, projVector));
            return (float)Math.Atan2(det, dot);

        }

        /// <summary>
        /// Projects the given vector onto the plane defined by the given normal.
        /// </summary>
        /// <param name="vector">The vector to project.</param>
        /// <param name="norm">The vector defining the plane to project on. Should be normalized when passed into method.</param>
        /// <returns></returns>
        public static Vector3 Project(Vector3 vector, Vector3 norm)
        {
            return vector - (Vector3.Dot(vector, norm) * norm);
        }


    }
}
