using Xunit;
using Xunit.Abstractions;
using PanicEngine.Maths;
using PanicEngine.Physix;
using System;

namespace PanicEngine.Tests.Physix
{
    public class Body2DTests
    {
        #region Constructor Tests

        [Fact]
        public void Test_Body2D_Constructor_ValidParameters()
        {
            var body = new Body2D(1, new Vector2D(0, 0), 1f, 1f);
            Assert.Equal(1, body.Id);
            Assert.Equal(new Vector2D(0, 0), body.Position);
            Assert.Equal(Vector2D.Zero, body.Velocity);
            Assert.Equal(1f, body.Mass);
            Assert.Equal(1f, body.InverseMass);
            Assert.Equal(1f, body.Radius);
            Assert.Equal(0.6f, body.Restitution);
            Assert.Equal(0.5f, body.LinearDamping);
            Assert.Equal(0.02f, body.SleepSpeed);
            Assert.False(body.IsSleeping);
            Assert.False(body.IsStatic);
        }

        [Fact]
        public void Test_Body2D_Constructor_WithCustomPosition()
        {
            var body = new Body2D(2, new Vector2D(10, 20), 2f, 2f);
            Assert.Equal(2, body.Id);
            Assert.Equal(new Vector2D(10, 20), body.Position);
            Assert.Equal(2f, body.Mass);
            Assert.Equal(0.5f, body.InverseMass);
            Assert.Equal(2f, body.Radius);
        }

        [Fact]
        public void Test_Body2D_Constructor_WithZeroMass()
        {
            var body = new Body2D(3, new Vector2D(0, 0), 0f, 1f);
            Assert.Equal(0f, body.Mass);
            Assert.Equal(0f, body.InverseMass);
        }

        [Fact]
        public void Test_Body2D_Constructor_ThrowsException_WhenIdIsNegative()
        {
            Assert.Throws<ArgumentException>(() => new Body2D(-1, new Vector2D(0, 0), 1f, 1f));
        }

        [Fact]
        public void Test_Body2D_Constructor_ThrowsException_WhenRadiusIsZero()
        {
            Assert.Throws<ArgumentException>(() => new Body2D(1, new Vector2D(0, 0), 1f, 0f));
        }

        [Fact]
        public void Test_Body2D_Constructor_ThrowsException_WhenRadiusIsNegative()
        {
            Assert.Throws<ArgumentException>(() => new Body2D(1, new Vector2D(0, 0), 1f, -1f));
        }

        #endregion

        #region Property Tests

        [Fact]
        public void Test_Body2D_Position_SetAndGet()
        {
            var body = new Body2D(1, new Vector2D(0, 0), 1f, 1f);
            body.Position = new Vector2D(5, 10);
            Assert.Equal(new Vector2D(5, 10), body.Position);
        }

        [Fact]
        public void Test_Body2D_Velocity_SetAndGet()
        {
            var body = new Body2D(1, new Vector2D(0, 0), 1f, 1f);
            body.Velocity = new Vector2D(3, 4);
            Assert.Equal(new Vector2D(3, 4), body.Velocity);
        }

        [Fact]
        public void Test_Body2D_Radius_SetAndGet()
        {
            var body = new Body2D(1, new Vector2D(0, 0), 1f, 1f);
            body.Radius = 5f;
            Assert.Equal(5f, body.Radius);
        }

        [Fact]
        public void Test_Body2D_Radius_ThrowsException_WhenSetToZero()
        {
            var body = new Body2D(1, new Vector2D(0, 0), 1f, 1f);
            Assert.Throws<ArgumentException>(() => body.Radius = 0f);
        }

        [Fact]
        public void Test_Body2D_Radius_ThrowsException_WhenSetToNegative()
        {
            var body = new Body2D(1, new Vector2D(0, 0), 1f, 1f);
            Assert.Throws<ArgumentException>(() => body.Radius = -1f);
        }

        [Fact]
        public void Test_Body2D_Restitution_ClampsToZero()
        {
            var body = new Body2D(1, new Vector2D(0, 0), 1f, 1f);
            body.Restitution = -1f;
            Assert.Equal(0f, body.Restitution);
        }

        [Fact]
        public void Test_Body2D_Restitution_ClampsToOne()
        {
            var body = new Body2D(1, new Vector2D(0, 0), 1f, 1f);
            body.Restitution = 2f;
            Assert.Equal(1f, body.Restitution);
        }

        [Fact]
        public void Test_Body2D_Restitution_AcceptsValidValue()
        {
            var body = new Body2D(1, new Vector2D(0, 0), 1f, 1f);
            body.Restitution = 0.8f;
            Assert.Equal(0.8f, body.Restitution);
        }

        [Fact]
        public void Test_Body2D_LinearDamping_ClampsToZero()
        {
            var body = new Body2D(1, new Vector2D(0, 0), 1f, 1f);
            body.LinearDamping = -1f;
            Assert.Equal(0f, body.LinearDamping);
        }

        [Fact]
        public void Test_Body2D_LinearDamping_ClampsToOne()
        {
            var body = new Body2D(1, new Vector2D(0, 0), 1f, 1f);
            body.LinearDamping = 2f;
            Assert.Equal(1f, body.LinearDamping);
        }

        [Fact]
        public void Test_Body2D_LinearDamping_AcceptsValidValue()
        {
            var body = new Body2D(1, new Vector2D(0, 0), 1f, 1f);
            body.LinearDamping = 0.3f;
            Assert.Equal(0.3f, body.LinearDamping);
        }

        [Fact]
        public void Test_Body2D_SleepSpeed_SetAndGet()
        {
            var body = new Body2D(1, new Vector2D(0, 0), 1f, 1f);
            body.SleepSpeed = 0.05f;
            Assert.Equal(0.05f, body.SleepSpeed);
        }

        [Fact]
        public void Test_Body2D_IsSleeping_SetAndGet()
        {
            var body = new Body2D(1, new Vector2D(0, 0), 1f, 1f);
            body.IsSleeping = true;
            Assert.True(body.IsSleeping);
        }

        [Fact]
        public void Test_Body2D_IsStatic_SetAndGet()
        {
            var body = new Body2D(1, new Vector2D(0, 0), 1f, 1f);
            body.IsStatic = true;
            Assert.True(body.IsStatic);
        }

        [Fact]
        public void Test_Body2D_Mass_IsReadOnly()
        {
            var body = new Body2D(1, new Vector2D(0, 0), 5f, 1f);
            Assert.Equal(5f, body.Mass);
        }

        [Fact]
        public void Test_Body2D_InverseMass_IsReadOnly()
        {
            var body = new Body2D(1, new Vector2D(0, 0), 2f, 1f);
            Assert.Equal(0.5f, body.InverseMass);
        }

        #endregion

        #region Step Method Tests

        [Fact]
        public void Test_Body2D_Step_UpdatesPosition()
        {
            var body = new Body2D(1, new Vector2D(0, 0), 1f, 1f);
            body.Velocity = new Vector2D(10, 20);
            body.LinearDamping = 0f; // No damping for simplicity
            
            body.Step(0.1f);
            
            Assert.Equal(new Vector2D(1, 2), body.Position);
        }

        [Fact]
        public void Test_Body2D_Step_AppliesLinearDamping()
        {
            var body = new Body2D(1, new Vector2D(0, 0), 1f, 1f);
            body.Velocity = new Vector2D(10, 0);
            body.LinearDamping = 0.5f;
            
            body.Step(1f);
            
            // damping = 1 - 0.5 * 1 = 0.5
            // velocity should be 10 * 0.5 = 5
            Assert.Equal(5f, body.Velocity.X, 5);
            Assert.Equal(0f, body.Velocity.Y);
            Assert.False(body.IsSleeping);
            Assert.Equal(new Vector2D(5, 0), body.Position);
        }

        [Fact]
        public void Test_Body2D_Step_DoesNothing_WhenDeltaTimeIsZero()
        {
            var body = new Body2D(1, new Vector2D(0, 0), 1f, 1f);
            body.Velocity = new Vector2D(10, 20);
            var initialPosition = body.Position;
            
            body.Step(0f);
            
            Assert.Equal(initialPosition, body.Position);
            Assert.Equal(new Vector2D(10, 20), body.Velocity);
        }

        [Fact]
        public void Test_Body2D_Step_DoesNothing_WhenDeltaTimeIsNegative()
        {
            var body = new Body2D(1, new Vector2D(0, 0), 1f, 1f);
            body.Velocity = new Vector2D(10, 20);
            var initialPosition = body.Position;
            
            body.Step(-1f);
            
            Assert.Equal(initialPosition, body.Position);
            Assert.Equal(new Vector2D(10, 20), body.Velocity);
        }

        [Fact]
        public void Test_Body2D_Step_DoesNothing_WhenIsStatic()
        {
            var body = new Body2D(1, new Vector2D(0, 0), 1f, 1f);
            body.IsStatic = true;
            body.Velocity = new Vector2D(10, 20);
            var initialPosition = body.Position;
            
            body.Step(0.1f);
            
            Assert.Equal(initialPosition, body.Position);
            Assert.Equal(new Vector2D(10, 20), body.Velocity);
        }

        [Fact]
        public void Test_Body2D_Step_PutsBodyToSleep_WhenVelocityIsBelowSleepSpeed()
        {
            var body = new Body2D(1, new Vector2D(0, 0), 1f, 1f);
            body.SleepSpeed = 0.02f;
            body.Velocity = new Vector2D(0.01f, 0.01f); // LengthSquared = 0.0002 < 0.02^2 = 0.0004
            body.LinearDamping = 0f;
            
            body.Step(0.1f);
            
            Assert.True(body.IsSleeping);
            Assert.Equal(Vector2D.Zero, body.Velocity);
        }

        [Fact]
        public void Test_Body2D_Step_KeepsBodySleeping_WhenAlreadySleeping()
        {
            var body = new Body2D(1, new Vector2D(0, 0), 1f, 1f);
            body.SleepSpeed = 0.02f;
            body.Velocity = new Vector2D(0.01f, 0.01f);
            body.LinearDamping = 0f;
            body.IsSleeping = true;
            var initialPosition = body.Position;
            
            body.Step(0.1f);
            
            Assert.True(body.IsSleeping);
            Assert.Equal(initialPosition, body.Position);
        }

        [Fact]
        public void Test_Body2D_Step_WakesUpBody_WhenVelocityExceedsSleepSpeed()
        {
            var body = new Body2D(1, new Vector2D(0, 0), 1f, 1f);
            body.SleepSpeed = 0.02f;
            body.IsSleeping = true;
            body.Velocity = new Vector2D(1, 0);
            body.LinearDamping = 0f;
            
            body.Step(0.1f);
            
            Assert.False(body.IsSleeping);
            Assert.NotEqual(Vector2D.Zero, body.Position);
        }

        [Fact]
        public void Test_Body2D_Step_DampingClampsToZero_WhenDampingBecomesNegative()
        {
            var body = new Body2D(1, new Vector2D(0, 0), 1f, 1f);
            body.Velocity = new Vector2D(10, 0);
            body.LinearDamping = 2f; // Very high damping
            var initialPosition = body.Position;
            
            body.Step(1f);
            
            // damping = 1 - 2 * 1 = -1, should clamp to 0
            // velocity should be 0
            Assert.Equal(0f, body.Velocity.X);
            Assert.Equal(initialPosition, body.Position);
        }

        [Fact]
        public void Test_Body2D_Step_ComplexMovement()
        {
            var body = new Body2D(1, new Vector2D(0, 0), 1f, 1f);
            body.Velocity = new Vector2D(5, 10);
            body.LinearDamping = 0.1f;
            
            body.Step(0.2f);
            
            // damping = 1 - 0.1 * 0.2 = 0.98
            // velocity after damping = (5, 10) * 0.98 = (4.9, 9.8)
            // position = (0, 0) + (4.9, 9.8) * 0.2 = (0.98, 1.96)
            Assert.Equal(4.9f, body.Velocity.X, 1);
            Assert.Equal(9.8f, body.Velocity.Y, 1);
            Assert.Equal(0.98f, body.Position.X, 1);
            Assert.Equal(1.96f, body.Position.Y, 1);
        }

        #endregion

        #region ApplyImpulse Method Tests

        [Fact]
        public void Test_Body2D_ApplyImpulse_UpdatesVelocity()
        {
            var body = new Body2D(1, new Vector2D(0, 0), 1f, 1f);
            body.Velocity = Vector2D.Zero;
            
            body.ApplyImpulse(new Vector2D(10, 20));
            
            // v += impulse * invMass = (0,0) + (10,20) * 1 = (10,20)
            Assert.Equal(new Vector2D(10, 20), body.Velocity);
        }

        [Fact]
        public void Test_Body2D_ApplyImpulse_WithNonZeroMass()
        {
            var body = new Body2D(1, new Vector2D(0, 0), 2f, 1f);
            body.Velocity = Vector2D.Zero;
            
            body.ApplyImpulse(new Vector2D(10, 20));
            
            // v += impulse * invMass = (0,0) + (10,20) * 0.5 = (5,10)
            Assert.Equal(new Vector2D(5, 10), body.Velocity);
        }

        [Fact]
        public void Test_Body2D_ApplyImpulse_WithExistingVelocity()
        {
            var body = new Body2D(1, new Vector2D(0, 0), 1f, 1f);
            body.Velocity = new Vector2D(5, 5);
            
            body.ApplyImpulse(new Vector2D(10, 20));
            
            // v += impulse * invMass = (5,5) + (10,20) * 1 = (15,25)
            Assert.Equal(new Vector2D(15, 25), body.Velocity);
        }

        [Fact]
        public void Test_Body2D_ApplyImpulse_DoesNothing_WhenIsStatic()
        {
            var body = new Body2D(1, new Vector2D(0, 0), 1f, 1f);
            body.IsStatic = true;
            body.Velocity = Vector2D.Zero;
            
            body.ApplyImpulse(new Vector2D(10, 20));
            
            Assert.Equal(Vector2D.Zero, body.Velocity);
        }

        [Fact]
        public void Test_Body2D_ApplyImpulse_WakesUpSleepingBody()
        {
            var body = new Body2D(1, new Vector2D(0, 0), 1f, 1f);
            body.IsSleeping = true;
            body.Velocity = Vector2D.Zero;
            
            body.ApplyImpulse(new Vector2D(10, 20));
            
            Assert.False(body.IsSleeping);
            Assert.Equal(new Vector2D(10, 20), body.Velocity);
        }

        [Fact]
        public void Test_Body2D_ApplyImpulse_WithZeroMass()
        {
            var body = new Body2D(1, new Vector2D(0, 0), 0f, 1f);
            body.Velocity = Vector2D.Zero;
            
            body.ApplyImpulse(new Vector2D(10, 20));
            
            // v += impulse * invMass = (0,0) + (10,20) * 0 = (0,0)
            Assert.Equal(Vector2D.Zero, body.Velocity);
        }

        [Fact]
        public void Test_Body2D_ApplyImpulse_MultipleImpulses()
        {
            var body = new Body2D(1, new Vector2D(0, 0), 1f, 1f);
            body.Velocity = Vector2D.Zero;
            
            body.ApplyImpulse(new Vector2D(10, 0));
            body.ApplyImpulse(new Vector2D(0, 20));
            
            Assert.Equal(new Vector2D(10, 20), body.Velocity);
        }

        #endregion
    }
}