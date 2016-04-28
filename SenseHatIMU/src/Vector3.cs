using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SenseHatIMU.src
{
    public struct Vector3
    {
        public double X
        { get; set; }

        public double Y
        { get; set; }

        public double Z
        { get; set; }

        public Vector3(double x, double y, double z)
        {
            X = x;
            Y = y;
            Z = z;
        }

        public void Zero()
        {
            X = 0;
            Y = 0;
            Z = 0;
        }

        public static Vector3 operator +(Vector3 lhs, Vector3 rhs)
        {
            return new Vector3(lhs.X + rhs.X, lhs.Y + rhs.Y, lhs.Z + rhs.Z);
        }

        public static Vector3 operator -(Vector3 lhs, Vector3 rhs)
        {
            return new Vector3(lhs.X - rhs.X, lhs.Y - rhs.Y, lhs.Z - rhs.Z);
        }

        public static double DotProduct(Vector3 a, Vector3 b)
        {
            return a.X * b.X + a.Y * b.Y + a.Z * b.Z;
        }

        public static Vector3 CrossProduct(Vector3 a, Vector3 b)
        {
            return new Vector3(a.Y * b.Z - a.Z * b.Y, a.Z * b.X - a.X * b.Z, a.X * b.Y - a.Y * b.X);
        }

        public Vector3 AccelToEuler()
        {
            Vector3 normAccel = this;
            normAccel.Normalize();

            var rollPitchYaw = new Vector3(
                Math.Atan2(normAccel.Y, normAccel.Z),
                -Math.Atan2(normAccel.X, Math.Sqrt(normAccel.Y * normAccel.Y + normAccel.Z * normAccel.Z)),
                0);

            return rollPitchYaw;
        }

        public Quaternion AccelToQuaternion()
        {
            Vector3 normAccel = this;
            Vector3 z = new Vector3(0, 0, 1.0);

            normAccel.Normalize();

            double angle = Math.Acos(DotProduct(z, normAccel));
            Vector3 vec = CrossProduct(normAccel, z);
            vec.Normalize();

            var qPose = new Quaternion();
            qPose.FromAngleVector(angle, vec);
            return qPose;
        }

        public void Normalize()
        {
            double length = Math.Sqrt(X * X + Y * Y + Z * Z);

            if ((length == 0) || (length == 1))
            {
                return;
            }

            X /= length;
            Y /= length;
            Z /= length;
        }

        public double Length()
        {
            return Math.Sqrt(X * X + Y * Y + Z * Z);
        }

        public Vector3 AsDegrees => new Vector3(
            X * MathSupport.RadToDegree,
            Y * MathSupport.RadToDegree,
            Z * MathSupport.RadToDegree);

        public override string ToString()
        {
            return ToString(false);
        }

        public string ToString(bool asDegrees)
        {
            return asDegrees
                ? $"roll: {X * MathSupport.RadToDegree:F4}, pitch: {Y * MathSupport.RadToDegree:F4}, yaw: {Z * MathSupport.RadToDegree:F4}"
                : $"x: {X:F4}, y: {Y:F4}, z: {Z:F4}";
        }
    }
}
