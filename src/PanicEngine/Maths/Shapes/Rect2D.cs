namespace PanicEngine.Maths.Shapes
{
    public class Rect2D : IShape2D
    {
        public float MinX { get; }
        public float MinY { get; }
        public float MaxX { get; }
        public float MaxY { get; }

        public Rect2D(float minX, float minY, float maxX, float maxY)
        {
            MinX = minX;
            MinY = minY;
            MaxX = maxX;
            MaxY = maxY;
        }

        public bool Contains(Vector2D point)
        {
            return point.X >= MinX 
                && point.X <= MaxX 
                && point.Y >= MinY 
                && point.Y <= MaxY;
        }

        public bool Overlaps(Vector2D point, float radius)
        {
            float closestX = Math.Clamp(point.X, MinX, MaxX);
            float closestY = Math.Clamp(point.Y, MinY, MaxY);

            float distanceX = point.X - closestX;
            float distanceY = point.Y - closestY;

            float squaredDistance = distanceX * distanceX + distanceY * distanceY;

            return squaredDistance <= radius * radius;
        }
    }
}