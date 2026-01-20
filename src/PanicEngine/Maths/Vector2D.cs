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
        public readonly Vector2D Normalized => Length > float.Epsilon ? this / Length : Zero;
        public readonly float Dot(Vector2D other) => X * other.X + Y * other.Y;
        public readonly float Cross(Vector2D other) => X * other.Y - Y * other.X;
        public readonly Vector2D Perp() => new(-Y, X);
        public readonly Vector2D Reflect(Vector2D unitNormal)
        {
            var dot = Dot(unitNormal);
            return this - 2f * dot * unitNormal;
        }

        public readonly float DistanceSquared(Vector2D other)
        {
            var distanceX = X - other.X;
            var distanceY = Y - other.Y;

            return distanceX * distanceX + distanceY * distanceY;
        }

        public readonly float Distance(Vector2D other) => (float) Math.Sqrt(DistanceSquared(other));

        public override readonly string ToString() => $"({X}, {Y})";

        //operators
        public static Vector2D operator +(Vector2D a, Vector2D b) => new(a.X + b.X, a.Y + b.Y);
        public static Vector2D operator -(Vector2D a, Vector2D b) => new(a.X - b.X, a.Y - b.Y);
        public static Vector2D operator -(Vector2D a) => new(-a.X, -a.Y);
        public static Vector2D operator *(Vector2D a, float b) => new(a.X * b, a.Y * b);
        public static Vector2D operator *(float b, Vector2D a) => new(a.X * b, a.Y * b);
        public static Vector2D operator /(Vector2D a, float b) => 
            Math.Abs(b) > float.Epsilon ? new(a.X / b, a.Y / b) : Zero;
    }
}