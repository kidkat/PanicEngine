using PanicEngine.Maths.Shapes;

namespace PanicEngine.Maths
{
    public readonly struct FieldBounds
    {
        public readonly Rect2D Rect { get; }

        public FieldBounds(Rect2D rect)
        {
            Rect = rect;
        }

        public float Width => Rect.MaxX - Rect.MinX;
        public float Height => Rect.MaxY - Rect.MinY;

        public FieldBounds Normalized()
        {
            float minX = Rect.MinX, maxX = Rect.MaxX;
            float minY = Rect.MinY, maxY = Rect.MaxY;

            if(minX > maxX)
                (maxX, minX) = (minX, maxX);

            if (minY > maxY)
                (maxY, minY) = (minY, maxY);

            return new FieldBounds(new Rect2D(minX, minY, maxX, maxY));
        }
    }
}