﻿namespace DemoFramework
{
    public class MathHelper
    {
        public static BulletXNA.LinearMath.Vector3 Vector3(SharpDX.Vector3 vector)
        {
            return new BulletXNA.LinearMath.Vector3(vector.X, vector.Y, vector.Z);
        }

        public static SharpDX.Vector3 Vector3(BulletXNA.LinearMath.Vector3 vector)
        {
            return new SharpDX.Vector3(vector.X, vector.Y, vector.Z);
        }

        public static BulletXNA.LinearMath.Vector4 Vector4(SharpDX.Vector4 vector)
        {
            return new BulletXNA.LinearMath.Vector4(vector.X, vector.Y, vector.Z, vector.W);
        }

        public static SharpDX.Vector4 Vector4(BulletXNA.LinearMath.Vector4 vector)
        {
            return new SharpDX.Vector4(vector.X, vector.Y, vector.Z, vector.W);
        }

        public static BulletXNA.LinearMath.Matrix Matrix(SharpDX.Matrix vector)
        {
            return new BulletXNA.LinearMath.Matrix(
                vector.M11, vector.M12, vector.M13, vector.M14,
                vector.M21, vector.M22, vector.M23, vector.M24,
                vector.M31, vector.M32, vector.M33, vector.M34,
                vector.M41, vector.M42, vector.M43, vector.M44);
        }

        public static SharpDX.Matrix Matrix(BulletXNA.LinearMath.Matrix vector)
        {
            return new SharpDX.Matrix(
                vector.M11, vector.M12, vector.M13, vector.M14,
                vector.M21, vector.M22, vector.M23, vector.M24,
                vector.M31, vector.M32, vector.M33, vector.M34,
                vector.M41, vector.M42, vector.M43, vector.M44);
        }
    }
}