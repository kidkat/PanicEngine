using Xunit;
using PanicEngine.Maths;

namespace PanicEngine.Tests.Maths
{
    public class Vector2DTests
    {
        [Fact]
        public void Test_Vector2D_Constructor_With_Values()
        {
            var vector = new Vector2D(1, 2);
            
            Assert.Equal(1, vector.X);
            Assert.Equal(2, vector.Y);
        }

        [Fact]
        public void Test_Vector2D_Constructor_With_Zero_Values()
        {
            var vector = new Vector2D();

            Assert.Equal(0, vector.X);
            Assert.Equal(0, vector.Y);
        }

        [Fact]
        public void Test_Vector2D_Zero()
        {
            var vector = Vector2D.Zero;

            Assert.Equal(0, vector.X);
            Assert.Equal(0, vector.Y);
        }

        [Fact]
        public void Test_Vector2D_Length_Squared()
        {
            var vector = new Vector2D(3, 4);

            Assert.Equal(25, vector.LengthSquared);
        }

        [Fact]
        public void Test_Vector2D_Length()
        {
            var vector = new Vector2D(3, 4);

            Assert.Equal(5, vector.Length);
        }

        [Fact]
        public void Test_Vector2D_Normalized()
        {
            var vector = new Vector2D(3, 4);

            Assert.Equal(new Vector2D(0.6f, 0.8f), vector.Normalized());
        }

        [Fact]
        public void Test_Vector2D_Normalized_Zero_Vector()
        {
            var vector = Vector2D.Zero;

            Assert.Equal(Vector2D.Zero, vector.Normalized());
        }
    }
}