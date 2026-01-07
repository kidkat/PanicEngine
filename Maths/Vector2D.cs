using System;

namespace PanicEngine.Maths;
public struct Vector2D
{
    public readonly float X;
    public readonly float Y;

    public Vector2D(float x, float y)
    {
        X = x;
        Y = y;
    }

    public static Vector2D Zero = new(0f, 0f);
    
    public float LengthSquared  => X * X + Y * Y;
    public float Length => (float) Math.Sqrt(LengthSquared);
    public Vector2D Normalized() => Length > 0 ? this / Length : Zero;
    public float Dot(Vector2D other) => X * other.X + Y * other.Y;
    public Vector2D Perp() => new(-Y, X);
    public Vector2D Reflect(Vector2D unitNormal)
    {
        float d = Dot(unitNormal);
        return this - 2f * d * unitNormal;
    }

    public static Vector2D operator +(Vector2D a, Vector2D b) => new(a.X + b.X, a.Y + b.Y);
    public static Vector2D operator -(Vector2D a, Vector2D b) => new(a.X - b.X, a.Y - b.Y);
    public static Vector2D operator -(Vector2D a) => new(-a.X, -a.Y);
    public static Vector2D operator *(Vector2D a, float b) => new(a.X * b, a.Y * b);
    public static Vector2D operator *(float b, Vector2D a) => new(a.X * b, a.Y * b);
    public static Vector2D operator /(Vector2D a, float b) => new(a.X / b, a.Y / b);
}