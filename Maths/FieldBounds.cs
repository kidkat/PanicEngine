namespace PanicEngine.Maths
{

    public readonly struct FieldBounds
    {
        public float MinX { get; }
        public float MaxX { get; }
        public float MinY { get; }
        public float MaxY { get; }

        public FieldBounds(float minX, float maxX, float minY, float maxY)
        {
            MinX = minX;
            MaxX = maxX;
            MinY = minY;
            MaxY = maxY;
        }

        public float Width => MaxX - MinX;
        public float Height => MaxY - MinY;

        public FieldBounds Normalized()
        {
            float minX = MinX, maxX = MaxX;
            float minY = MinY, maxY = MaxY;

            if(minX > maxX)
            {
                float t = minX;
                minX = maxX;
                maxX = t;
            }

            if(minY < maxY)
            {
                float t = minY;
                minY = maxY;
                maxY = t;
            }

            return new FieldBounds(minX, maxX, minY, maxY);
        }
    }
}