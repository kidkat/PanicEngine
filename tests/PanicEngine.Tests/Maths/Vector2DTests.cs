using Xunit;
using Xunit.Abstractions;
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

            Assert.Equal(new Vector2D(0.6f, 0.8f), vector.Normalized);
            float x = vector.Normalized.X;
            float y = vector.Normalized.Y;
            Assert.Equal(0.6f, x);
            Assert.Equal(0.8f, y);
            Assert.Equal(1, x * x + y * y);
        }

        [Fact]
        public void Test_Vector2D_Normalized_Zero_Vector()
        {
            var vector = Vector2D.Zero;

            Assert.Equal(Vector2D.Zero, vector.Normalized);
        }

        [Fact]
        public void Test_Vector2D_Dot()
        {
            var vector1 = new Vector2D(1, 2);
            var vector2 = new Vector2D(3, 4);

            Assert.Equal(11, vector1.Dot(vector2));
        }

        [Fact]
        public void Test_Vector2D_Cross()
        {
            var vector1 = new Vector2D(1, 2);
            var vector2 = new Vector2D(3, 4);
            
            Assert.Equal(-2, vector1.Cross(vector2));
        }

        [Fact]
        public void Test_Vector2D_Perp()
        {
            var vector = new Vector2D(1, 2);

            Assert.Equal(new Vector2D(-2, 1), vector.Perp());
        }

        [Fact]
        public void Test_Vector2D_Reflect()
        {
            var vector = new Vector2D(1, 2);
            var normal = new Vector2D(3, 4);

            Assert.Equal(new Vector2D(-65, -86), vector.Reflect(normal));
        }

        [Fact]
        public void Test_Vector2D_Distance_Squared()
        {
            var vector1 = new Vector2D(1, 2);
            var vector2 = new Vector2D(3, 4);
            
            Assert.Equal(8, vector1.DistanceSquared(vector2));
        }

        [Fact]
        public void Test_Vector2D_Distance()
        {
            var vector1 = new Vector2D(1, 2);
            var vector2 = new Vector2D(3, 4);
            
            Assert.Equal(2.828427f, vector1.Distance(vector2));
        }

        [Fact]
        public void Test_Vector2D_ToString()
        {
            var vector = new Vector2D(1, 2);
            
            Assert.Equal("(1, 2)", vector.ToString());
        }

        [Fact]
        public void Test_Vector2D_Operator_Add()
        {
            var vector1 = new Vector2D(1, 2);
            var vector2 = new Vector2D(3, 4);
            
            Assert.Equal(new Vector2D(4, 6), vector1 + vector2);
        }

        [Fact]
        public void Test_Vector2D_Operator_Subtract()
        {
            var vector1 = new Vector2D(1, 2);
            var vector2 = new Vector2D(3, 4);
            
            Assert.Equal(new Vector2D(-2, -2), vector1 - vector2);
        }

        [Fact]
        public void Test_Vector2D_Operator_Negate()
        {
            var vector = new Vector2D(1, 2);
            
            Assert.Equal(new Vector2D(-1, -2), -vector);
        }

        [Fact]
        public void Test_Vector2D_Operator_Multiply()
        {
            var vector = new Vector2D(1, 2);
            var scalar = 3;
            
            Assert.Equal(new Vector2D(3, 6), vector * scalar);
            Assert.Equal(new Vector2D(3, 6), scalar * vector);
        }

        [Fact]
        public void Test_Vector2D_Operator_Divide()
        {
            var vector = new Vector2D(1, 2);
            var scalar = 3;
            
            Assert.Equal(new Vector2D(0.33333334f, 0.6666667f), vector / scalar);
        }

        [Fact]
        public void Test_Vector2D_Operator_Divide_By_Zero()
        {
            var vector = new Vector2D(1, 2);
            var scalar = 0;
            
            Assert.Equal(Vector2D.Zero, vector / scalar);
        }

        [Fact]
        public void Test_Vector2D_Equal()
        {
            var vector1 = new Vector2D(1, 2);
            var vector2 = new Vector2D(1, 2);
            
            Assert.True(vector1.Equal(vector2));
            Assert.False(vector1.Equal(new Vector2D(1, 3)));
        }
    }
}