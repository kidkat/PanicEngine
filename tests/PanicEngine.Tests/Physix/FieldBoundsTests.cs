using Xunit;
using PanicEngine.Physix;
using PanicEngine.Maths;
using PanicEngine.Maths.Shapes;

namespace PanicEngine.Tests.Physix
{
    public class FieldBoundsTests
    {
        #region Constructor Tests

        [Fact]
        public void Test_Constructor_WithValidValues_SetsPropertiesCorrectly()
        {
            var bounds = new FieldBounds(new Rect2D(-5f, -3f, 5f, 3f));

            Assert.Equal(-5f, bounds.Rect.MinX);
            Assert.Equal(-3f, bounds.Rect.MinY);
            Assert.Equal(5f, bounds.Rect.MaxX);
            Assert.Equal(3f, bounds.Rect.MaxY);
        }

        [Fact]
        public void Test_Constructor_WithZeroValues_SetsPropertiesCorrectly()
        {
            var bounds = new FieldBounds(new Rect2D(0f, 0f, 0f, 0f));

            Assert.Equal(0f, bounds.Rect.MinX);
            Assert.Equal(0f, bounds.Rect.MinY);
            Assert.Equal(0f, bounds.Rect.MaxX);
            Assert.Equal(0f, bounds.Rect.MaxY);
        }

        [Fact]
        public void Test_Constructor_WithNegativeValues_SetsPropertiesCorrectly()
        {
            var bounds = new FieldBounds(new Rect2D(-10f, -20f, -5f, -15f));

            Assert.Equal(-10f, bounds.Rect.MinX);
            Assert.Equal(-20f, bounds.Rect.MinY);
            Assert.Equal(-5f, bounds.Rect.MaxX);
            Assert.Equal(-15f, bounds.Rect.MaxY);
        }

        [Fact]
        public void Test_Constructor_WithLargeValues_SetsPropertiesCorrectly()
        {
            var bounds = new FieldBounds(new Rect2D(100f, 200f, 1000f, 2000f));

            Assert.Equal(100f, bounds.Rect.MinX);
            Assert.Equal(200f, bounds.Rect.MinY);
            Assert.Equal(1000f, bounds.Rect.MaxX);
            Assert.Equal(2000f, bounds.Rect.MaxY);
        }

        #endregion

        #region Width Property Tests

        [Fact]
        public void Test_Width_WithNormalBounds_ReturnsCorrectValue()
        {
            var bounds = new FieldBounds(new Rect2D(-5f, -3f, 5f, 3f));

            Assert.Equal(10f, bounds.Width);
        }

        [Fact]
        public void Test_Width_WithZeroWidth_ReturnsZero()
        {
            var bounds = new FieldBounds(new Rect2D(5f, -3f, 5f, 3f));

            Assert.Equal(0f, bounds.Width);
        }

        [Fact]
        public void Test_Width_WithSwappedXValues_ReturnsNegativeValue()
        {
            var bounds = new FieldBounds(new Rect2D(5f, -3f, -5f, 3f));

            Assert.Equal(-10f, bounds.Width);
        }

        [Fact]
        public void Test_Width_WithNegativeBounds_ReturnsCorrectValue()
        {
            var bounds = new FieldBounds(new Rect2D(-10f, -20f, -5f, -15f));

            Assert.Equal(5f, bounds.Width);
        }

        #endregion

        #region Height Property Tests

        [Fact]
        public void Test_Height_WithNormalBounds_ReturnsCorrectValue()
        {
            var bounds = new FieldBounds(new Rect2D(-5f, -3f, 5f, 3f));

            Assert.Equal(6f, bounds.Height);
        }

        [Fact]
        public void Test_Height_WithZeroHeight_ReturnsZero()
        {
            var bounds = new FieldBounds(new Rect2D(-5f, 3f, 5f, 3f));

            Assert.Equal(0f, bounds.Height);
        }

        [Fact]
        public void Test_Height_WithSwappedYValues_ReturnsNegativeValue()
        {
            var bounds = new FieldBounds(new Rect2D(-5f, 3f, 5f, -3f));

            Assert.Equal(-6f, bounds.Height);
        }

        [Fact]
        public void Test_Height_WithNegativeBounds_ReturnsCorrectValue()
        {
            var bounds = new FieldBounds(new Rect2D(-10f, -20f, -5f, -15f));

            Assert.Equal(5f, bounds.Height);
        }

        #endregion

        #region Normalized Method Tests

        [Fact]
        public void Test_Normalized_WithNormalBounds_ReturnsSameBounds()
        {
            var bounds = new FieldBounds(new Rect2D(-5f, -3f, 5f, 3f));
            var normalized = bounds.Normalized();

            Assert.Equal(-5f, normalized.Rect.MinX);
            Assert.Equal(-3f, normalized.Rect.MinY);
            Assert.Equal(5f, normalized.Rect.MaxX);
            Assert.Equal(3f, normalized.Rect.MaxY);
        }

        [Fact]
        public void Test_Normalized_WithSwappedXValues_SwapsXValues()
        {
            var bounds = new FieldBounds(new Rect2D(5f, -3f, -5f, 3f));
            var normalized = bounds.Normalized();

            Assert.Equal(-5f, normalized.Rect.MinX);
            Assert.Equal(5f, normalized.Rect.MaxX);
            Assert.Equal(-3f, normalized.Rect.MinY);
            Assert.Equal(3f, normalized.Rect.MaxY);
        }

        [Fact]
        public void Test_Normalized_WithSwappedYValues_SwapsYValues()
        {
            var bounds = new FieldBounds(new Rect2D(-5f, 3f, 5f, -3f));
            var normalized = bounds.Normalized();

            Assert.Equal(-5f, normalized.Rect.MinX);
            Assert.Equal(5f, normalized.Rect.MaxX);
            Assert.Equal(-3f, normalized.Rect.MinY);
            Assert.Equal(3f, normalized.Rect.MaxY);
        }

        [Fact]
        public void Test_Normalized_WithBothXAndYSwapped_SwapsBoth()
        {
            var bounds = new FieldBounds(new Rect2D(5f, 3f, -5f, -3f));
            var normalized = bounds.Normalized();

            Assert.Equal(-5f, normalized.Rect.MinX);
            Assert.Equal(5f, normalized.Rect.MaxX);
            Assert.Equal(-3f, normalized.Rect.MinY);
            Assert.Equal(3f, normalized.Rect.MaxY);
        }

        [Fact]
        public void Test_Normalized_WithZeroBounds_ReturnsSameBounds()
        {
            var bounds = new FieldBounds(new Rect2D(0f, 0f, 0f, 0f));
            var normalized = bounds.Normalized();

            Assert.Equal(0f, normalized.Rect.MinX);
            Assert.Equal(0f, normalized.Rect.MinY);
            Assert.Equal(0f, normalized.Rect.MaxX);
            Assert.Equal(0f, normalized.Rect.MaxY);
        }

        [Fact]
        public void Test_Normalized_WithNegativeBounds_ReturnsSameBounds()
        {
            var bounds = new FieldBounds(new Rect2D(-10f, -20f, -5f, -15f));
            var normalized = bounds.Normalized();

            Assert.Equal(-10f, normalized.Rect.MinX);
            Assert.Equal(-20f, normalized.Rect.MinY);
            Assert.Equal(-5f, normalized.Rect.MaxX);
            Assert.Equal(-15f, normalized.Rect.MaxY);
        }

        [Fact]
        public void Test_Normalized_WithLargeValues_ReturnsNormalizedBounds()
        {
            var bounds = new FieldBounds(new Rect2D(1000f, 2000f, 100f, 200f));
            var normalized = bounds.Normalized();

            Assert.Equal(100f, normalized.Rect.MinX);
            Assert.Equal(200f, normalized.Rect.MinY);
            Assert.Equal(1000f, normalized.Rect.MaxX);
            Assert.Equal(2000f, normalized.Rect.MaxY);
        }

        [Fact]
        public void Test_Normalized_WithEqualMinMaxX_ReturnsSameXValues()
        {
            var bounds = new FieldBounds(new Rect2D(5f, -3f, 5f, 3f));
            var normalized = bounds.Normalized();

            Assert.Equal(5f, normalized.Rect.MinX);
            Assert.Equal(5f, normalized.Rect.MaxX);
            Assert.Equal(-3f, normalized.Rect.MinY);
            Assert.Equal(3f, normalized.Rect.MaxY);
        }

        [Fact]
        public void Test_Normalized_WithEqualMinMaxY_ReturnsSameYValues()
        {
            var bounds = new FieldBounds(new Rect2D(-5f, 3f, 5f, 3f));
            var normalized = bounds.Normalized();

            Assert.Equal(-5f, normalized.Rect.MinX);
            Assert.Equal(5f, normalized.Rect.MaxX);
            Assert.Equal(3f, normalized.Rect.MinY);
            Assert.Equal(3f, normalized.Rect.MaxY);
        }

        [Fact]
        public void Test_Normalized_DoesNotModifyOriginalBounds()
        {
            var bounds = new FieldBounds(new Rect2D(5f, 3f, -5f, -3f));
            var originalMinX = bounds.Rect.MinX;
            var originalMinY = bounds.Rect.MinY;
            var originalMaxX = bounds.Rect.MaxX;
            var originalMaxY = bounds.Rect.MaxY;

            Assert.Equal(originalMinX, bounds.Rect.MinX);
            Assert.Equal(originalMinY, bounds.Rect.MinY);
            Assert.Equal(originalMaxX, bounds.Rect.MaxX);
            Assert.Equal(originalMaxY, bounds.Rect.MaxY);
        }

        [Fact]
        public void Test_Normalized_ReturnsNewInstance()
        {
            var bounds = new FieldBounds(new Rect2D(5f, 3f, -5f, -3f));
            var normalized = bounds.Normalized();

            Assert.NotEqual(bounds, normalized);
        }

        #endregion

        #region Readonly Struct Tests

        [Fact]
        public void Test_FieldBounds_IsReadonlyStruct_PropertiesAreReadonly()
        {
            var bounds = new FieldBounds(new Rect2D(-5f, -3f, 5f, 3f));

            // Verify that properties are accessible and readonly (compiler enforces this)
            var minX = bounds.Rect.MinX;
            var minY = bounds.Rect.MinY;
            var maxX = bounds.Rect.MaxX;
            var maxY = bounds.Rect.MaxY;

            Assert.Equal(-5f, minX);
            Assert.Equal(-3f, minY);
            Assert.Equal(5f, maxX);
            Assert.Equal(3f, maxY);
        }

        #endregion
    }
}
