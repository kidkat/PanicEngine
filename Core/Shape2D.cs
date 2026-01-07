using System;
using PanicEngine.Maths;

namespace PanicEngine.Core;

public readonly struct Shape2D
{
    public Shape2DType Type { get; }
    public float Radius { get; }
    public Vector2D HalfSize { get; }

    private Shape2D(Shape2DType type, float radius, Vector2D halfSize)
    {
        Type = type;
        Radius = radius;
        HalfSize = halfSize;
    }

    public static Shape2D Circle(float radius) => new Shape2D(Shape2DType.Circle, radius, Vector2D.Zero);
    public static Shape2D Box(float width, float height) => new Shape2D(Shape2DType.Box, 0f, new Vector2D(width * 0.5f, height * 0.5f));
}