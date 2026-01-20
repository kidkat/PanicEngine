using System;

namespace PanicEngine.Maths
{
    
    public struct Vector2D
    {
        public float X;
        public float Y;

        public Vector2D() : this(0f, 0f) {}

        public Vector2D(float x, float y)
        {
            X = x;
            Y = y;
        }

        public static Vector2D Zero = new(0f, 0f);
        
        public readonly float LengthSquared => X * X + Y * Y;
        public readonly float Length => (float) Math.Sqrt(LengthSquared);
        public readonly Vector2D Normalized() => Length > 0 ? this / Length : Zero;
        public readonly float Dot(Vector2D other) => X * other.X + Y * other.Y;
        public readonly Vector2D Perp() => new(-Y, X);
        public readonly Vector2D Reflect(Vector2D unitNormal)
        {
            float d = Dot(unitNormal);
            return this - 2f * d * unitNormal;
        }

        //operators
        public static Vector2D operator +(Vector2D a, Vector2D b) => new(a.X + b.X, a.Y + b.Y);
        public static Vector2D operator -(Vector2D a, Vector2D b) => new(a.X - b.X, a.Y - b.Y);
        public static Vector2D operator -(Vector2D a) => new(-a.X, -a.Y);
        public static Vector2D operator *(Vector2D a, float b) => new(a.X * b, a.Y * b);
        public static Vector2D operator *(float b, Vector2D a) => new(a.X * b, a.Y * b);
        public static Vector2D operator /(Vector2D a, float b) => new(a.X / b, a.Y / b);
    }
}