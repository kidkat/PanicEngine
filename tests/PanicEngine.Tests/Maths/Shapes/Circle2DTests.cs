using Xunit;
using PanicEngine.Maths;
using PanicEngine.Maths.Shapes;

namespace PanicEngine.Tests.Maths.Shapes
{
    public class Circle2DTests
    {
        #region Contains

        [Fact]
        public void Contains_PointInside_ReturnsTrue()
        {
            var circle = new Circle2D(new Vector2D(0, 0), 5);
            var point = new Vector2D(2, 2);

            Assert.True(circle.Contains(point));
        }

        [Fact]
        public void Contains_PointOnBoundary_ReturnsTrue()
        {
            var circle = new Circle2D(new Vector2D(0, 0), 5);
            var point = new Vector2D(5, 0);

            Assert.True(circle.Contains(point));
        }

        [Fact]
        public void Contains_PointOutside_ReturnsFalse()
        {
            var circle = new Circle2D(new Vector2D(0, 0), 5);
            var point = new Vector2D(10, 10);

            Assert.False(circle.Contains(point));
        }

        [Fact]
        public void Contains_Center_ReturnsTrue()
        {
            var circle = new Circle2D(new Vector2D(3, 4), 2);
            var point = new Vector2D(3, 4);

            Assert.True(circle.Contains(point));
        }

        #endregion

        #region Overlaps

        [Fact]
        public void Overlaps_OverlappingCircles_ReturnsTrue()
        {
            var circle = new Circle2D(new Vector2D(0, 0), 5);
            var point = new Vector2D(3, 0);
            float radius = 3;

            Assert.True(circle.Overlaps(point, radius));
        }

        [Fact]
        public void Overlaps_TouchingCircles_ReturnsTrue()
        {
            var circle = new Circle2D(new Vector2D(0, 0), 5);
            var point = new Vector2D(10, 0);
            float radius = 5;

            Assert.True(circle.Overlaps(point, radius));
        }

        [Fact]
        public void Overlaps_SeparateCircles_ReturnsFalse()
        {
            var circle = new Circle2D(new Vector2D(0, 0), 5);
            var point = new Vector2D(20, 0);
            float radius = 2;

            Assert.False(circle.Overlaps(point, radius));
        }

        [Fact]
        public void Overlaps_ZeroRadiusAtCenter_ReturnsTrue()
        {
            var circle = new Circle2D(new Vector2D(0, 0), 5);
            var point = new Vector2D(0, 0);
            float radius = 0;

            Assert.True(circle.Overlaps(point, radius));
        }

        #endregion
    }
}
