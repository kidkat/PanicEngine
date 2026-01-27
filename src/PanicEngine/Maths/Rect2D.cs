namespace PanicEngine.Maths
{
    public readonly struct Rect2D(float minX, float minY, float maxX, float maxY)
    {
        public readonly float MinX { get; } = minX;
        public readonly float MinY { get; } = minY;
        public readonly float MaxX { get; } = maxX;
        public readonly float MaxY { get; } = maxY;

        public bool Contains(Vector2D point)
        {
            return point.X >= MinX && point.X <= MaxX && point.Y >= MinY && point.Y <= MaxY;
        }

        public float ClampX(float x)
        {
            if(x < MinX) return MinX;
            if(x > MaxX) return MaxX;

            return x;
        }

        public float ClampY(float y)
        {
            if(y < MinY) return MinY;
            if(y > MaxY) return MaxY;

            return y;
        }
    }
}