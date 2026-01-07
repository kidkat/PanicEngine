namespace PanicEngine.Maths;

public readonly struct Aabb
{
    public Vector2D Min { get; }
    public Vector2D Max { get; }

    public Aabb(Vector2D min, Vector2D max)
    {
        Min = min;
        Max = max;
    }

    public static Aabb FromCenterHalfSize(Vector2D center, Vector2D halfSize)
    {
        return new Aabb(center - halfSize, center + halfSize);
    }

    public bool Overlaps(Aabb other)
    {
        if(Max.X < other.Min.X || Min.X > other.Max.X) return false;
        if(Max.Y < other.Min.Y || Min.Y > other.Max.Y) return false;
        return true;
    }
}