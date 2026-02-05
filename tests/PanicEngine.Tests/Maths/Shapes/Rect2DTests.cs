using Xunit;
using PanicEngine.Maths;
using PanicEngine.Maths.Shapes;

namespace PanicEngine.Tests.Maths.Shapes
{
    public class Rect2DTests
    {
        #region Contains

        [Fact]
        public void Contains_PointInside_ReturnsTrue()
        {
            var rect = new Rect2D(0, 0, 10, 10);
            var point = new Vector2D(5, 5);

            Assert.True(rect.Contains(point));
        }

        [Fact]
        public void Contains_PointOnMinBoundary_ReturnsTrue()
        {
            var rect = new Rect2D(0, 0, 10, 10);
            var point = new Vector2D(0, 5);

            Assert.True(rect.Contains(point));
        }

        [Fact]
        public void Contains_PointOnMaxBoundary_ReturnsTrue()
        {
            var rect = new Rect2D(0, 0, 10, 10);
            var point = new Vector2D(10, 10);

            Assert.True(rect.Contains(point));
        }

        [Fact]
        public void Contains_PointOutside_ReturnsFalse()
        {
            var rect = new Rect2D(0, 0, 10, 10);
            var point = new Vector2D(15, 15);

            Assert.False(rect.Contains(point));
        }

        [Fact]
        public void Contains_PointLeftOfRect_ReturnsFalse()
        {
            var rect = new Rect2D(5, 5, 15, 15);
            var point = new Vector2D(0, 10);

            Assert.False(rect.Contains(point));
        }

        #endregion

        #region Overlaps

        [Fact]
        public void Overlaps_CircleInsideRect_ReturnsTrue()
        {
            var rect = new Rect2D(0, 0, 10, 10);
            var point = new Vector2D(5, 5);
            float radius = 2;

            Assert.True(rect.Overlaps(point, radius));
        }

        [Fact]
        public void Overlaps_CircleOverlappingEdge_ReturnsTrue()
        {
            var rect = new Rect2D(0, 0, 10, 10);
            var point = new Vector2D(12, 5);
            float radius = 3;

            Assert.True(rect.Overlaps(point, radius));
        }

        [Fact]
        public void Overlaps_CircleOutside_ReturnsFalse()
        {
            var rect = new Rect2D(0, 0, 10, 10);
            var point = new Vector2D(20, 20);
            float radius = 1;

            Assert.False(rect.Overlaps(point, radius));
        }

        [Fact]
        public void Overlaps_ZeroRadiusPointInside_ReturnsTrue()
        {
            var rect = new Rect2D(0, 0, 10, 10);
            var point = new Vector2D(5, 5);
            float radius = 0;

            Assert.True(rect.Overlaps(point, radius));
        }

        #endregion
    }
}
