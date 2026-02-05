namespace PanicEngine.Maths.Shapes
{
    public sealed class Circle2D : IShape2D
    {
        public Vector2D Center { get; }
        public float Radius { get; }

        public Circle2D(Vector2D center, float radius)
        {
            Center = center;
            Radius = radius;
        }

        public bool Contains(Vector2D point)
        {
            return point.DistanceSquared(Center) <= Radius * Radius;
        }

        public bool Overlaps(Vector2D point, float radius)
        {
            float radiusSum = this.Radius + radius;
            return Center.DistanceSquared(point) <= radiusSum * radiusSum;
        }
    }
}