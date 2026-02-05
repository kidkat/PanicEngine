using Xunit;
using Xunit.Abstractions;
using PanicEngine.Maths;
using PanicEngine.Physix;
using PanicEngine.Core;

namespace PanicEngine.Tests.Core
{
    public class BodiesManagerTests
    {
        private static Body2D CreateBody(int id, Vector2D position, float mass = 1f, float radius = 1f)
            => new Body2D(id, position, mass, radius);

        // --- AddBody ---
        [Fact]
        public void AddBody_AddsBodyToList()
        {
            var manager = new BodiesManager();
            var body = CreateBody(1, Vector2D.Zero);
            manager.AddBody(body);
            Assert.Single(manager.Bodies);
            Assert.Same(body, manager.Bodies[0]);
        }

        [Fact]
        public void AddBody_Null_ThrowsArgumentNullException()
        {
            var manager = new BodiesManager();
            Assert.Throws<ArgumentNullException>(() => manager.AddBody(null!));
        }

        // --- RemoveBody ---
        [Fact]
        public void RemoveBody_RemovesExistingBody()
        {
            var manager = new BodiesManager();
            var body = CreateBody(1, Vector2D.Zero);
            manager.AddBody(body);
            manager.RemoveBody(body);
            Assert.Empty(manager.Bodies);
        }

        [Fact]
        public void RemoveBody_Null_ThrowsArgumentNullException()
        {
            var manager = new BodiesManager();
            Assert.Throws<ArgumentNullException>(() => manager.RemoveBody(null!));
        }

        // --- RemoveBodyById ---
        [Fact]
        public void RemoveBodyById_ExistingId_RemovesBody()
        {
            var manager = new BodiesManager();
            var body = CreateBody(42, Vector2D.Zero);
            manager.AddBody(body);
            manager.RemoveBodyById(42);
            Assert.Empty(manager.Bodies);
        }

        [Fact]
        public void RemoveBodyById_NonExistentId_DoesNothing()
        {
            var manager = new BodiesManager();
            var body = CreateBody(1, Vector2D.Zero);
            manager.AddBody(body);
            manager.RemoveBodyById(999);
            Assert.Single(manager.Bodies);
        }

        [Fact]
        public void RemoveBodyById_EmptyList_DoesNotThrow()
        {
            var manager = new BodiesManager();
            manager.RemoveBodyById(1);
            Assert.Empty(manager.Bodies);
        }

        // --- GetBodyById ---
        [Fact]
        public void GetBodyById_ExistingId_ReturnsBody()
        {
            var manager = new BodiesManager();
            var body = CreateBody(7, Vector2D.Zero);
            manager.AddBody(body);
            Assert.Same(body, manager.GetBodyById(7));
        }

        [Fact]
        public void GetBodyById_NonExistentId_ReturnsNull()
        {
            var manager = new BodiesManager();
            manager.AddBody(CreateBody(1, Vector2D.Zero));
            Assert.Null(manager.GetBodyById(99));
        }

        [Fact]
        public void GetBodyById_EmptyList_ReturnsNull()
        {
            var manager = new BodiesManager();
            Assert.Null(manager.GetBodyById(1));
        }

        // --- ClearBodies ---
        [Fact]
        public void ClearBodies_RemovesAllBodies()
        {
            var manager = new BodiesManager();
            manager.AddBody(CreateBody(1, Vector2D.Zero));
            manager.AddBody(CreateBody(2, Vector2D.Zero));
            manager.ClearBodies();
            Assert.Empty(manager.Bodies);
        }

        // --- LimitVelocity ---
        [Fact]
        public void LimitVelocity_MaxSpeedZeroOrNegative_DoesNotChangeVelocities()
        {
            var manager = new BodiesManager();
            var body = CreateBody(1, Vector2D.Zero);
            body.Velocity = new Vector2D(100, 100);
            manager.AddBody(body);
            var settings = new PhysixSettings { MaxSpeed = 0f };
            manager.LimitVelocity(settings);
            Assert.Equal(100, body.Velocity.X);
            Assert.Equal(100, body.Velocity.Y);
        }

        [Fact]
        public void LimitVelocity_BodyOverMaxSpeed_ClampsVelocity()
        {
            var manager = new BodiesManager();
            var body = CreateBody(1, Vector2D.Zero);
            body.Velocity = new Vector2D(30, 40); // length 50
            manager.AddBody(body);
            var settings = new PhysixSettings { MaxSpeed = 10f };
            manager.LimitVelocity(settings);
            Assert.Equal(6f, body.Velocity.X);
            Assert.Equal(8f, body.Velocity.Y);
            Assert.Equal(10f, body.Velocity.Length);
        }

        [Fact]
        public void LimitVelocity_StaticBody_NotClamped()
        {
            var manager = new BodiesManager();
            var body = CreateBody(1, Vector2D.Zero);
            body.IsStatic = true;
            body.Velocity = new Vector2D(100, 0);
            manager.AddBody(body);
            var settings = new PhysixSettings { MaxSpeed = 10f };
            manager.LimitVelocity(settings);
            Assert.Equal(100, body.Velocity.X);
        }

        [Fact]
        public void LimitVelocity_AllUnderLimit_NoChange()
        {
            var manager = new BodiesManager();
            var body = CreateBody(1, Vector2D.Zero);
            body.Velocity = new Vector2D(1, 0);
            manager.AddBody(body);
            var settings = new PhysixSettings { MaxSpeed = 50f };
            manager.LimitVelocity(settings);
            Assert.Equal(1, body.Velocity.X);
            Assert.Equal(0, body.Velocity.Y);
        }

        // --- UpdateBodies ---
        [Fact]
        public void UpdateBodies_CallsStepOnEachBody()
        {
            var manager = new BodiesManager();
            var body = CreateBody(1, new Vector2D(0, 0));
            body.Velocity = new Vector2D(10, 0);
            manager.AddBody(body);
            manager.UpdateBodies(0.1f);
            Assert.Equal(0.949999988f, body.Position.X);
            Assert.Equal(0f, body.Position.Y);
        }

        [Fact]
        public void UpdateBodies_StaticBody_DoesNotMove()
        {
            var manager = new BodiesManager();
            var body = CreateBody(1, new Vector2D(5, 5));
            body.IsStatic = true;
            body.Velocity = new Vector2D(100, 100);
            manager.AddBody(body);
            manager.UpdateBodies(0.1f);
            Assert.Equal(5, body.Position.X);
            Assert.Equal(5, body.Position.Y);
        }
    }
}