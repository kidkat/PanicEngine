namespace PanicEngine.Maths.Shapes
{
    public interface IShape2D
    {
        bool Contains(Vector2D point);

        bool Overlaps(Vector2D point, float radius);
    }
}