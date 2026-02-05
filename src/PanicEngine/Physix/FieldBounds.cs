namespace PanicEngine.Physix
{
    public readonly struct FieldBounds
    {
        public readonly float MinX { get; }
        public readonly float MinY { get; }
        public readonly float MaxX { get; }
        public readonly float MaxY { get; }

        public FieldBounds(float minX, float minY, float maxX, float maxY)
        {
            MinX = minX;
            MinY = minY;
            MaxX = maxX;
            MaxY = maxY;
        }

        public float Width => MaxX - MinX;
        public float Height => MaxY - MinY;

        public FieldBounds Normalized()
        {
            float minX = MinX, maxX = MaxX;
            float minY = MinY, maxY = MaxY;

            if(minX > maxX)
                (maxX, minX) = (minX, maxX);

            if (minY > maxY)
                (maxY, minY) = (minY, maxY);

            return new FieldBounds(minX, minY, maxX, maxY);
        }
    }
}