namespace PanicEngine.Physix
{
    public readonly struct FieldBounds(float minX, float minY, float maxX, float maxY)
    {
        public readonly float MinX { get; } = minX;  
        public readonly float MinY { get; } = minY;
        public readonly float MaxX { get; } = maxX;
        public readonly float MaxY { get; } = maxY;

        public float Width => MaxX - MinX;
        public float Height => MaxY - MinY;

        public FieldBounds Normalized()
        {
            float minX = MinX, maxX = MaxX;
            float minY = MinY, maxY = MaxY;

            if(minX > maxX)
                (maxX, minX) = (minX, maxX);

            if (minY < maxY)
                (maxY, minY) = (minY, maxY);

            return new FieldBounds(minX, maxX, minY, maxY);
        }
    }
}