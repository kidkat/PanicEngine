using System.Collections.Generic;
using Xunit;
using PanicEngine.Maths;
using PanicEngine.Maths.Shapes;

namespace PanicEngine.Tests.Maths.Shapes
{
    public class Composite2DTests
    {
        #region Constructor with List

        [Fact]
        public void Constructor_WithList_Contains_PointInFirstShape_ReturnsTrue()
        {
            var circle = new Circle2D(new Vector2D(0, 0), 5);
            var rect = new Rect2D(10, 10, 20, 20);
            var shapes = new List<IShape2D> { circle, rect };
            var composite = new Composite2D(shapes);
            var point = new Vector2D(2, 2);

            Assert.True(composite.Contains(point));
        }

        [Fact]
        public void Constructor_WithList_Contains_PointInSecondShape_ReturnsTrue()
        {
            var circle = new Circle2D(new Vector2D(0, 0), 2);
            var rect = new Rect2D(10, 10, 20, 20);
            var shapes = new List<IShape2D> { circle, rect };
            var composite = new Composite2D(shapes);
            var point = new Vector2D(15, 15);

            Assert.True(composite.Contains(point));
        }

        [Fact]
        public void Constructor_WithList_Contains_PointInNoShape_ReturnsFalse()
        {
            var circle = new Circle2D(new Vector2D(0, 0), 2);
            var rect = new Rect2D(10, 10, 20, 20);
            var shapes = new List<IShape2D> { circle, rect };
            var composite = new Composite2D(shapes);
            var point = new Vector2D(5, 5);

            Assert.False(composite.Contains(point));
        }

        [Fact]
        public void Constructor_WithList_Overlaps_CircleOverlapsFirstShape_ReturnsTrue()
        {
            var circle = new Circle2D(new Vector2D(0, 0), 5);
            var rect = new Rect2D(20, 20, 30, 30);
            var shapes = new List<IShape2D> { circle, rect };
            var composite = new Composite2D(shapes);
            var point = new Vector2D(3, 0);
            float radius = 2;

            Assert.True(composite.Overlaps(point, radius));
        }

        [Fact]
        public void Constructor_WithList_Overlaps_CircleOverlapsSecondShape_ReturnsTrue()
        {
            var circle = new Circle2D(new Vector2D(0, 0), 2);
            var rect = new Rect2D(10, 10, 20, 20);
            var shapes = new List<IShape2D> { circle, rect };
            var composite = new Composite2D(shapes);
            var point = new Vector2D(15, 15);
            float radius = 1;

            Assert.True(composite.Overlaps(point, radius));
        }

        [Fact]
        public void Constructor_WithList_Overlaps_CircleOverlapsNoShape_ReturnsFalse()
        {
            var circle = new Circle2D(new Vector2D(0, 0), 2);
            var rect = new Rect2D(10, 10, 20, 20);
            var shapes = new List<IShape2D> { circle, rect };
            var composite = new Composite2D(shapes);
            var point = new Vector2D(50, 50);
            float radius = 1;

            Assert.False(composite.Overlaps(point, radius));
        }

        [Fact]
        public void Constructor_WithEmptyList_Contains_ReturnsFalse()
        {
            var shapes = new List<IShape2D>();
            var composite = new Composite2D(shapes);
            var point = new Vector2D(0, 0);

            Assert.False(composite.Contains(point));
        }

        [Fact]
        public void Constructor_WithEmptyList_Overlaps_ReturnsFalse()
        {
            var shapes = new List<IShape2D>();
            var composite = new Composite2D(shapes);
            var point = new Vector2D(0, 0);
            float radius = 5;

            Assert.False(composite.Overlaps(point, radius));
        }

        [Fact]
        public void Constructor_WithNullList_Contains_ReturnsFalse()
        {
            List<IShape2D>? shapes = null;
            var composite = new Composite2D(shapes!);
            var point = new Vector2D(0, 0);

            Assert.False(composite.Contains(point));
        }

        [Fact]
        public void Constructor_WithNullList_Overlaps_ReturnsFalse()
        {
            List<IShape2D>? shapes = null;
            var composite = new Composite2D(shapes!);
            var point = new Vector2D(0, 0);
            float radius = 5;

            Assert.False(composite.Overlaps(point, radius));
        }

        [Fact]
        public void Constructor_WithList_ThreeShapes_Contains_PointInThird_ReturnsTrue()
        {
            var circle1 = new Circle2D(new Vector2D(0, 0), 1);
            var circle2 = new Circle2D(new Vector2D(5, 5), 1);
            var rect = new Rect2D(10, 10, 20, 20);
            var shapes = new List<IShape2D> { circle1, circle2, rect };
            var composite = new Composite2D(shapes);
            var point = new Vector2D(15, 15);

            Assert.True(composite.Contains(point));
        }

        [Fact]
        public void Constructor_WithList_ThreeShapes_Overlaps_ThirdShape_ReturnsTrue()
        {
            var circle1 = new Circle2D(new Vector2D(0, 0), 1);
            var circle2 = new Circle2D(new Vector2D(5, 5), 1);
            var rect = new Rect2D(10, 10, 20, 20);
            var shapes = new List<IShape2D> { circle1, circle2, rect };
            var composite = new Composite2D(shapes);
            var point = new Vector2D(15, 15);
            float radius = 1;

            Assert.True(composite.Overlaps(point, radius));
        }

        #endregion

        #region Contains

        [Fact]
        public void Contains_PointInFirstShape_ReturnsTrue()
        {
            var circle = new Circle2D(new Vector2D(0, 0), 5);
            var rect = new Rect2D(10, 10, 20, 20);
            var composite = new Composite2D(circle, rect);
            var point = new Vector2D(2, 2);

            Assert.True(composite.Contains(point));
        }

        [Fact]
        public void Contains_PointInSecondShape_ReturnsTrue()
        {
            var circle = new Circle2D(new Vector2D(0, 0), 2);
            var rect = new Rect2D(10, 10, 20, 20);
            var composite = new Composite2D(circle, rect);
            var point = new Vector2D(15, 15);

            Assert.True(composite.Contains(point));
        }

        [Fact]
        public void Contains_PointInNoShape_ReturnsFalse()
        {
            var circle = new Circle2D(new Vector2D(0, 0), 2);
            var rect = new Rect2D(10, 10, 20, 20);
            var composite = new Composite2D(circle, rect);
            var point = new Vector2D(5, 5);

            Assert.False(composite.Contains(point));
        }

        [Fact]
        public void Contains_EmptyComposite_ReturnsFalse()
        {
            var composite = new Composite2D();
            var point = new Vector2D(0, 0);

            Assert.False(composite.Contains(point));
        }

        #endregion

        #region Overlaps

        [Fact]
        public void Overlaps_CircleOverlapsFirstShape_ReturnsTrue()
        {
            var circle = new Circle2D(new Vector2D(0, 0), 5);
            var rect = new Rect2D(20, 20, 30, 30);
            var composite = new Composite2D(circle, rect);
            var point = new Vector2D(3, 0);
            float radius = 2;

            Assert.True(composite.Overlaps(point, radius));
        }

        [Fact]
        public void Overlaps_CircleOverlapsSecondShape_ReturnsTrue()
        {
            var circle = new Circle2D(new Vector2D(0, 0), 2);
            var rect = new Rect2D(10, 10, 20, 20);
            var composite = new Composite2D(circle, rect);
            var point = new Vector2D(15, 15);
            float radius = 1;

            Assert.True(composite.Overlaps(point, radius));
        }

        [Fact]
        public void Overlaps_CircleOverlapsNoShape_ReturnsFalse()
        {
            var circle = new Circle2D(new Vector2D(0, 0), 2);
            var rect = new Rect2D(10, 10, 20, 20);
            var composite = new Composite2D(circle, rect);
            var point = new Vector2D(50, 50);
            float radius = 1;

            Assert.False(composite.Overlaps(point, radius));
        }

        [Fact]
        public void Overlaps_EmptyComposite_ReturnsFalse()
        {
            var composite = new Composite2D();
            var point = new Vector2D(0, 0);
            float radius = 5;

            Assert.False(composite.Overlaps(point, radius));
        }

        #endregion
    }
}
