namespace PanicEngine.Maths;

public readonly struct FieldBounds
{
    public float Left { get; }
    public float Right { get; }
    public float Top { get; }
    public float Bottom { get; }

    public FieldBounds(float left, float right, float top, float bottom)
    {
        Left = left;
        Right = right;
        Top = top;
        Bottom = bottom;
    }

    public float Width => Right - Left;
    public float Height => Top - Bottom;
}