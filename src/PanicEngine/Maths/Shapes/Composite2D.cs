using System.Collections.Generic;

namespace PanicEngine.Maths.Shapes
{
    public sealed class Composite2D : IShape2D
    {
        private readonly List<IShape2D> _shapes = new();

        public Composite2D(List<IShape2D> shapes)
        {
            _shapes = shapes ?? new();
        }

        public Composite2D(params IShape2D[] shapes)
        {
            _shapes.AddRange(shapes ?? []);
        }

        public void AddShape(IShape2D shape)
        {
            _shapes.Add(shape);
        }

        public void RemoveShape(IShape2D shape)
        {
            _shapes.Remove(shape);
        }

        public bool Contains(Vector2D point)
        {
            foreach(var shape in _shapes)
                if(shape.Contains(point))
                    return true;

            return false;
        }

        public bool Overlaps(Vector2D point, float radius)
        {
            foreach(var shape in _shapes)
                if(shape.Overlaps(point, radius))
                    return true;

            return false;
        }
    }
}