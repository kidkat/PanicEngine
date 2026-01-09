namespace PanicEngine.Maths
{
    public readonly struct Rect2D
    {
        public readonly float MinX, MinY, MaxX, MaxY;

        public Rect2D(float minX, float minY, float maxX, float maxY)
        {
            if(minX > maxX)
            {
                float temp = minX;
                minX = maxX;
                maxX = temp;
            }

            if(minY > maxY)
            {
                float temp = minY;
                minY = maxY;
                maxY = temp;
            }

            MinX = minX;
            MinY = minY;
            MaxX = maxX;
            MaxY = maxY;
        }

        public bool Contains(Vector2D point)
        {
            return point.X >= MinX && point.X <= MaxX && point.Y >= MinY && point.Y <= MaxY;
        }
    }
}