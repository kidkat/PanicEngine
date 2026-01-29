using Xunit;
using Xunit.Abstractions;
using PanicEngine.Maths;
using PanicEngine.Physix;
using System;

namespace PanicEngine.Tests.Physix
{
    public class Collision2DTests
    {
        #region ResolveWallCollision Tests

        [Fact]
        public void Test_ResolveWallCollision_StaticBody_DoesNothing()
        {
            var body = new Body2D(1, new Vector2D(0, 0), 1f, 1f);
            body.IsStatic = true;
            body.Position = new Vector2D(-10, -10); //out of bounds
            body.Velocity = new Vector2D(5, 5);
            var fieldBounds = new FieldBounds(-5, -5, 5, 5);
            var originalPosition = body.Position;
            var originalVelocity = body.Velocity;

            Collision2D.ResolveWallCollision(body, fieldBounds);

            Assert.Equal(originalPosition, body.Position);
            Assert.Equal(originalVelocity, body.Velocity);
        }

        [Fact]
        public void Test_ResolveWallCollision_NoCollision_BodyInsideBounds()
        {
            var body = new Body2D(1, new Vector2D(0, 0), 1f, 0.5f);
            body.Position = new Vector2D(0, 0);
            body.Velocity = new Vector2D(2, 2);
            var fieldBounds = new FieldBounds(-5, -5, 5, 5);
            var originalPosition = body.Position;
            var originalVelocity = body.Velocity;

            Collision2D.ResolveWallCollision(body, fieldBounds);

            Assert.Equal(originalPosition, body.Position);
            Assert.Equal(originalVelocity, body.Velocity);
        }

        [Fact]
        public void Test_ResolveWallCollision_LeftWall_CorrectsPositionAndReflectsVelocity()
        {
            var body = new Body2D(1, new Vector2D(0, 0), 1f, 1f);
            body.Position = new Vector2D(-6, 0); //left of left border (-5 + 1 = -4)
            body.Velocity = new Vector2D(-2, 0); //moving left
            body.Restitution = 0.8f;
            var fieldBounds = new FieldBounds(-5, -5, 5, 5);

            Collision2D.ResolveWallCollision(body, fieldBounds);

            Assert.Equal(-4f, body.Position.X, 5); //position should be on the border
            Assert.Equal(0f, body.Position.Y, 5);
            //velocity should be reflected: Reflect((1,0)) from (-2,0) = (-2,0) - 2*((-2,0)·(1,0))*(1,0)
            // = (-2,0) - 2*(-2)*1*(1,0) = (-2,0) + 4*(1,0) = (2,0)
            //then multiplied by restitution: (2,0) * 0.8 = (1.6, 0)
            Assert.Equal(1.6f, body.Velocity.X, 5);
            Assert.Equal(0f, body.Velocity.Y, 5);
        }

        [Fact]
        public void Test_ResolveWallCollision_RightWall_CorrectsPositionAndReflectsVelocity()
        {
            var body = new Body2D(1, new Vector2D(0, 0), 1f, 1f);
            body.Position = new Vector2D(6, 0); //right of right border (5 - 1 = 4)
            body.Velocity = new Vector2D(2, 0); //moving right
            body.Restitution = 0.6f;
            var fieldBounds = new FieldBounds(-5, -5, 5, 5);

            Collision2D.ResolveWallCollision(body, fieldBounds);

            Assert.Equal(4f, body.Position.X, 5);
            Assert.Equal(0f, body.Position.Y, 5);
            //velocity should be reflected: Reflect((-1,0)) from (2,0) = (2,0) - 2*((2,0)·(-1,0))*(-1,0)
            // = (2,0) - 2*(-2)*(-1,0) = (2,0) - 4*(-1,0) = (-2,0)
            //then multiplied by restitution: (-2,0) * 0.6 = (-1.2, 0)
            Assert.Equal(-1.2f, body.Velocity.X, 5);
            Assert.Equal(0f, body.Velocity.Y, 5);
        }

        [Fact]
        public void Test_ResolveWallCollision_BottomWall_CorrectsPositionAndReflectsVelocity()
        {
            var body = new Body2D(1, new Vector2D(0, 0), 1f, 1f);
            body.Position = new Vector2D(0, 6); //below bottom border (5 - 1 = 4)
            body.Velocity = new Vector2D(0, 2); //moving down
            body.Restitution = 1f; //full restitution
            var fieldBounds = new FieldBounds(-5, -5, 5, 5);

            Collision2D.ResolveWallCollision(body, fieldBounds);

            Assert.Equal(0f, body.Position.X, 5);
            Assert.Equal(6f, body.Position.Y, 5);
            Assert.Equal(0f, body.Velocity.X, 5);
            Assert.Equal(2f, body.Velocity.Y, 5);
        }

        [Fact]
        public void Test_ResolveWallCollision_TopWall_CorrectsPositionAndReflectsVelocity()
        {
            var body = new Body2D(1, new Vector2D(0, 0), 1f, 1f);
            body.Position = new Vector2D(0, -6); //above top border (-5 + 1 = -4)
            body.Velocity = new Vector2D(0, -2); //moving up
            body.Restitution = 0.5f;
            var fieldBounds = new FieldBounds(-5, -5, 5, 5);

            Collision2D.ResolveWallCollision(body, fieldBounds);

            Assert.Equal(0f, body.Position.X, 5);
            Assert.Equal(-4f, body.Position.Y, 5);
            //velocity should be reflected: Reflect((0,1)) from (0,-2) = (0,-2) - 2*((0,-2)·(0,1))*(0,1)
            // = (0,-2) - 2*(-2)*(0,1) = (0,-2) + 4*(0,1) = (0,2)
            //then multiplied by restitution: (0,2) * 0.5 = (0, 1)
            Assert.Equal(0f, body.Velocity.X, 5);
            Assert.Equal(1f, body.Velocity.Y, 5);
        }

        [Fact]
        public void Test_ResolveWallCollision_LeftTopCorner_CorrectsBothCollisions()
        {
            var body = new Body2D(1, new Vector2D(0, 0), 1f, 1f);
            body.Position = new Vector2D(-6, -6); //left top corner
            body.Velocity = new Vector2D(-2, -2);
            body.Restitution = 0.8f;
            var fieldBounds = new FieldBounds(-5, -5, 5, 5);

            Collision2D.ResolveWallCollision(body, fieldBounds);

            //left wall is processed first, then top wall
            //after left: position (-4, -6), velocity is reflected from (1,0): (2, -2) * 0.8 = (1.6, -1.6)
            //then top: position (-4, -4), velocity is reflected from (0,1): (1.6, 1.6) * 0.8 = (1.28, 1.28)
            Assert.Equal(-4f, body.Position.X, 5);
            Assert.Equal(-4f, body.Position.Y, 5);
            //check approximately due to sequential reflections
            Assert.True(body.Velocity.X > 0, "Velocity X should be positive after reflection");
            Assert.True(body.Velocity.Y > 0, "Velocity Y should be positive after reflection");
        }

        [Fact]
        public void Test_ResolveWallCollision_RightBottomCorner_CorrectsBothCollisions()
        {
            var body = new Body2D(1, new Vector2D(0, 0), 1f, 1f);
            body.Position = new Vector2D(6, 6); //right bottom corner
            body.Velocity = new Vector2D(2, 2);
            body.Restitution = 0.6f;
            var fieldBounds = new FieldBounds(-5, -5, 5, 5);

            Collision2D.ResolveWallCollision(body, fieldBounds);

            Assert.Equal(4f, body.Position.X, 5);
            Assert.Equal(6f, body.Position.Y, 5);
            //velocity should be reflected in both axes
            Assert.True(body.Velocity.X < 0, "Velocity X should be negative after reflection");
            Assert.True(body.Velocity.Y > 0, "Velocity Y should be positive after reflection");
        }

        [Fact]
        public void Test_ResolveWallCollision_ZeroRestitution_StopsVelocity()
        {
            var body = new Body2D(1, new Vector2D(0, 0), 1f, 1f);
            body.Position = new Vector2D(-6, 0);
            body.Velocity = new Vector2D(-2, 0);
            body.Restitution = 0f; //no restitution
            var fieldBounds = new FieldBounds(-5, -5, 5, 5);

            Collision2D.ResolveWallCollision(body, fieldBounds);

            Assert.Equal(-4f, body.Position.X, 5);
            Assert.Equal(0f, body.Position.Y, 5);
            //velocity should be multiplied by 0
            Assert.Equal(0f, body.Velocity.X, 5);
            Assert.Equal(0f, body.Velocity.Y, 5);
        }

        [Fact]
        public void Test_ResolveWallCollision_DiagonalVelocity_ReflectsCorrectly()
        {
            var body = new Body2D(1, new Vector2D(0, 0), 1f, 1f);
            body.Position = new Vector2D(-6, 0);
            body.Velocity = new Vector2D(-1, 1); //diagonal velocity
            body.Restitution = 1f;
            var fieldBounds = new FieldBounds(-5, -5, 5, 5);

            Collision2D.ResolveWallCollision(body, fieldBounds);

            Assert.Equal(-4f, body.Position.X, 5);
            Assert.Equal(0f, body.Position.Y, 5);
            //velocity should be reflected from the normal (1,0): Reflect((1,0)) from (-1,1)
            // = (-1,1) - 2*((-1,1)·(1,0))*(1,0) = (-1,1) - 2*(-1)*(1,0) = (-1,1) + 2*(1,0) = (1,1)
            Assert.Equal(1f, body.Velocity.X, 5);
            Assert.Equal(1f, body.Velocity.Y, 5);
        }

        [Fact]
        public void Test_ResolveWallCollision_DifferentRadius_AdjustsBorders()
        {
            var body = new Body2D(1, new Vector2D(0, 0), 1f, 2f); //radius 2
            body.Position = new Vector2D(-8, 0); //should be left of border (-5 + 2 = -3)
            body.Velocity = new Vector2D(-2, 0);
            body.Restitution = 0.8f;
            var fieldBounds = new FieldBounds(-5, -5, 5, 5);

            Collision2D.ResolveWallCollision(body, fieldBounds);

            Assert.Equal(-3f, body.Position.X, 5); //border considers radius
            Assert.Equal(0f, body.Position.Y, 5);
        }

        [Fact]
        public void Test_ResolveWallCollision_ExactlyOnBorder_NoCollision()
        {
            var body = new Body2D(1, new Vector2D(0, 0), 1f, 1f);
            body.Position = new Vector2D(-4, 0); //exactly on left border (-5 + 1 = -4)
            body.Velocity = new Vector2D(-2, 0);
            body.Restitution = 0.8f;
            var fieldBounds = new FieldBounds(-5, -5, 5, 5);
            var originalPosition = body.Position;
            var originalVelocity = body.Velocity;

            Collision2D.ResolveWallCollision(body, fieldBounds);

            //condition position.X < leftBorder is not met (not strictly less)
            Assert.Equal(originalPosition, body.Position);
            Assert.Equal(originalVelocity, body.Velocity);
        }

        [Fact]
        public void Test_ResolveWallCollision_AllFourWalls_CorrectsAllCollisions()
        {
            var body = new Body2D(1, new Vector2D(0, 0), 1f, 0.5f);
            body.Position = new Vector2D(-6, -6); //outside all borders
            body.Velocity = new Vector2D(-1, -1);
            body.Restitution = 0.7f;
            var fieldBounds = new FieldBounds(-5, -5, 5, 5);

            Collision2D.ResolveWallCollision(body, fieldBounds);

            // all borders should be processed
            // leftBorder = -5 + 0.5 = -4.5, topBorder = -5 + 0.5 = -4.5
            Assert.Equal(-4.5f, body.Position.X, 5);
            Assert.Equal(-4.5f, body.Position.Y, 5);
        }

        [Fact]
        public void Test_ResolveWallCollision_VelocityPerpendicularToWall_MaintainsPerpendicularComponent()
        {
            var body = new Body2D(1, new Vector2D(0, 0), 1f, 1f);
            body.Position = new Vector2D(-6, 0);
            body.Velocity = new Vector2D(0, 3); //only vertical component
            body.Restitution = 1f;
            var fieldBounds = new FieldBounds(-5, -5, 5, 5);

            Collision2D.ResolveWallCollision(body, fieldBounds);

            Assert.Equal(-4f, body.Position.X, 5);
            //velocity should be reflected from (1,0): Reflect((1,0)) from (0,3) = (0,3) - 2*((0,3)·(1,0))*(1,0)
            // = (0,3) - 2*0*(1,0) = (0,3)
            //vertical component is preserved when reflected from horizontal normal
            Assert.Equal(0f, body.Velocity.X, 5);
            Assert.Equal(3f, body.Velocity.Y, 5);
        }

        [Fact]
        public void Test_ResolveWallCollision_RightWallWithNegativeVelocity_NoChange()
        {
            var body = new Body2D(1, new Vector2D(0, 0), 1f, 1f);
            body.Position = new Vector2D(6, 0); //right of border
            body.Velocity = new Vector2D(-2, 0); //moving left (from wall)
            body.Restitution = 0.8f;
            var fieldBounds = new FieldBounds(-5, -5, 5, 5);

            Collision2D.ResolveWallCollision(body, fieldBounds);

            //position is still corrected (body has already penetrated)
            Assert.Equal(4f, body.Position.X, 5);
            Assert.Equal(1.6f, body.Velocity.X, 5);
        }

        #endregion

        #region ResolveBodyCollision Tests

        [Fact]
        public void Test_ResolveBodyCollision_BothStatic_DoesNothing()
        {
            var body1 = new Body2D(1, new Vector2D(0, 0), 1f, 1f);
            body1.IsStatic = true;
            var body2 = new Body2D(2, new Vector2D(1, 0), 1f, 1f);
            body2.IsStatic = true;
            var settings = new PhysicsSettings();
            var originalPos1 = body1.Position;
            var originalPos2 = body2.Position;
            var originalVel1 = body1.Velocity;
            var originalVel2 = body2.Velocity;

            Collision2D.ResolveBodyCollision(body1, body2, settings);

            Assert.Equal(originalPos1, body1.Position);
            Assert.Equal(originalPos2, body2.Position);
            Assert.Equal(originalVel1, body1.Velocity);
            Assert.Equal(originalVel2, body2.Velocity);
        }

        [Fact]
        public void Test_ResolveBodyCollision_NoCollision_BodiesTooFar()
        {
            var body1 = new Body2D(1, new Vector2D(0, 0), 1f, 1f);
            var body2 = new Body2D(2, new Vector2D(5, 0), 1f, 1f); //distance 5, sum of radii 2, 5^2 = 25 > 4
            var settings = new PhysicsSettings();
            var originalPos1 = body1.Position;
            var originalPos2 = body2.Position;
            var originalVel1 = body1.Velocity;
            var originalVel2 = body2.Velocity;

            Collision2D.ResolveBodyCollision(body1, body2, settings);

            Assert.Equal(originalPos1, body1.Position);
            Assert.Equal(originalPos2, body2.Position);
            Assert.Equal(originalVel1, body1.Velocity);
            Assert.Equal(originalVel2, body2.Velocity);
        }

        [Fact]
        public void Test_ResolveBodyCollision_Collision_CorrectsPosition()
        {
            var body1 = new Body2D(1, new Vector2D(0, 0), 1f, 1f);
            var body2 = new Body2D(2, new Vector2D(1.5f, 0), 1f, 1f); //distance 1.5, sum of radii 2, collision
            body1.Velocity = Vector2D.Zero;
            body2.Velocity = Vector2D.Zero; //no relative velocity
            var settings = new PhysicsSettings
            {
                PositionCorrectionPercent = 0.8f,
                PositionCorrectionSlop = 0.01f
            };

            Collision2D.ResolveBodyCollision(body1, body2, settings);

            Assert.True(body1.Position.X < 0, "Body1 should move left");
            Assert.True(body2.Position.X > 1.5f, "Body2 should move right");
        }

        [Fact]
        public void Test_ResolveBodyCollision_Collision_AppliesImpulse()
        {   
            var body1 = new Body2D(1, new Vector2D(0, 0), 1f, 1f);
            var body2 = new Body2D(2, new Vector2D(1.5f, 0), 1f, 1f);
            body1.Velocity = new Vector2D(2, 0); //moving right
            body2.Velocity = new Vector2D(-1, 0); //moving left
            body1.Restitution = 0.8f;
            body2.Restitution = 0.6f;
            var settings = new PhysicsSettings();

            Collision2D.ResolveBodyCollision(body1, body2, settings);

            Assert.True(body1.Velocity.X < 2, "Body1 velocity should decrease");
            Assert.True(body2.Velocity.X > -1, "Body2 velocity should increase");
        }

        [Fact]
        public void Test_ResolveBodyCollision_BodiesSeparating_NoImpulse()
        {
            var body1 = new Body2D(1, new Vector2D(0, 0), 1f, 1f);
            var body2 = new Body2D(2, new Vector2D(1.5f, 0), 1f, 1f);
            body1.Velocity = new Vector2D(-2, 0); //moving left
            body2.Velocity = new Vector2D(1, 0); //moving right (separating)
            var settings = new PhysicsSettings();
            var originalVel1 = body1.Velocity;
            var originalVel2 = body2.Velocity;

            Collision2D.ResolveBodyCollision(body1, body2, settings);

            Assert.Equal(originalVel1, body1.Velocity);
            Assert.Equal(originalVel2, body2.Velocity);
        }

        [Fact]
        public void Test_ResolveBodyCollision_OneStatic_OnlyNonStaticMoves()
        {
            var body1 = new Body2D(1, new Vector2D(0, 0), 1f, 1f);
            body1.IsStatic = true;
            var body2 = new Body2D(2, new Vector2D(1.5f, 0), 1f, 1f);
            body2.Velocity = new Vector2D(-1, 0);
            var settings = new PhysicsSettings();
            var originalPos1 = body1.Position;

            Collision2D.ResolveBodyCollision(body1, body2, settings);

            Assert.Equal(originalPos1, body1.Position);
            Assert.True(body2.Position.X > 1.5f, "Body2 should move away from static body");
        }

        [Fact]
        public void Test_ResolveBodyCollision_ZeroDistance_UsesDefaultNormal()
        {
            var body1 = new Body2D(1, new Vector2D(0, 0), 1f, 1f);
            var body2 = new Body2D(2, new Vector2D(0, 0), 1f, 1f); //exactly on the same position
            body1.Velocity = new Vector2D(1, 0);
            body2.Velocity = new Vector2D(-1, 0);
            var settings = new PhysicsSettings();

            Collision2D.ResolveBodyCollision(body1, body2, settings);

            Assert.NotEqual(new Vector2D(0, 0), body1.Position);
            Assert.NotEqual(new Vector2D(0, 0), body2.Position);
        }

        [Fact]
        public void Test_ResolveBodyCollision_DifferentMasses_CorrectsProportionally()
        {
            var body1 = new Body2D(1, new Vector2D(0, 0), 2f, 1f); //mass 2
            var body2 = new Body2D(2, new Vector2D(1.5f, 0), 1f, 1f); //mass 1
            body1.Velocity = Vector2D.Zero;
            body2.Velocity = Vector2D.Zero;
            var settings = new PhysicsSettings
            {
                PositionCorrectionPercent = 0.8f,
                PositionCorrectionSlop = 0.01f
            };
            var originalPos1 = body1.Position;
            var originalPos2 = body2.Position;

            Collision2D.ResolveBodyCollision(body1, body2, settings);

            Assert.True(Math.Abs(body1.Position.X - originalPos1.X) < Math.Abs(body2.Position.X - originalPos2.X),
                "Lighter body should move more");
        }

        [Fact]
        public void Test_ResolveBodyCollision_ZeroMass_NoPositionCorrection()
        {
            var body1 = new Body2D(1, new Vector2D(0, 0), 0f, 1f); //zero mass
            var body2 = new Body2D(2, new Vector2D(1.5f, 0), 1f, 1f);
            body1.Velocity = Vector2D.Zero;
            body2.Velocity = Vector2D.Zero;
            var settings = new PhysicsSettings();
            var originalPos1 = body1.Position;
            var originalPos2 = body2.Position;

            Collision2D.ResolveBodyCollision(body1, body2, settings);

            Assert.Equal(originalPos1, body1.Position);
            Assert.NotEqual(originalPos2, body2.Position);
        }

        [Fact]
        public void Test_ResolveBodyCollision_BothZeroMass_NoCorrection()
        {
            var body1 = new Body2D(1, new Vector2D(0, 0), 0f, 1f);
            var body2 = new Body2D(2, new Vector2D(1.5f, 0), 0f, 1f);
            body1.Velocity = new Vector2D(1, 0);
            body2.Velocity = new Vector2D(-1, 0);
            var settings = new PhysicsSettings();
            var originalPos1 = body1.Position;
            var originalPos2 = body2.Position;
            var originalVel1 = body1.Velocity;
            var originalVel2 = body2.Velocity;

            Collision2D.ResolveBodyCollision(body1, body2, settings);

            Assert.Equal(originalPos1, body1.Position);
            Assert.Equal(originalPos2, body2.Position);
            Assert.Equal(originalVel1, body1.Velocity);
            Assert.Equal(originalVel2, body2.Velocity);
        }

        [Fact]
        public void Test_ResolveBodyCollision_DiagonalCollision_CorrectsCorrectly()
        {
            // Arrange
            var body1 = new Body2D(1, new Vector2D(0, 0), 1f, 1f);
            var body2 = new Body2D(2, new Vector2D(1.0f, 1.0f), 1f, 1f); // Диагональная коллизия
            // Расстояние = sqrt(1.0^2 + 1.0^2) ≈ 1.414, сумма радиусов = 2, проникновение ≈ 0.586
            body1.Velocity = Vector2D.Zero;
            body2.Velocity = Vector2D.Zero;
            var settings = new PhysicsSettings();
            var originalDistance = body1.Position.Distance(body2.Position);
            var originalPos1 = body1.Position;
            var originalPos2 = body2.Position;

            // Act
            Collision2D.ResolveBodyCollision(body1, body2, settings);

            // Assert
            // Нормаль должна быть диагональной
            // Позиции должны быть скорректированы по нормали
            var newDistance = body1.Position.Distance(body2.Position);
            // Расстояние должно увеличиться после коррекции
            Assert.True(newDistance > originalDistance, 
                $"Distance should increase after correction. Original: {originalDistance}, New: {newDistance}");
            // Позиции должны измениться
            Assert.NotEqual(originalPos1, body1.Position);
            Assert.NotEqual(originalPos2, body2.Position);
        }

        [Fact]
        public void Test_ResolveBodyCollision_FullRestitution_PerfectBounce()
        {
            var body1 = new Body2D(1, new Vector2D(0, 0), 1f, 1f);
            var body2 = new Body2D(2, new Vector2D(1.5f, 0), 1f, 1f);
            body1.Velocity = new Vector2D(2, 0);
            body2.Velocity = new Vector2D(-2, 0);
            body1.Restitution = 1f;
            body2.Restitution = 1f;
            var settings = new PhysicsSettings();

            Collision2D.ResolveBodyCollision(body1, body2, settings);

            Assert.Equal(-2f, body1.Velocity.X, 5);
            Assert.Equal(2f, body2.Velocity.X, 5);
        }

        [Fact]
        public void Test_ResolveBodyCollision_ZeroRestitution_NoBounce()
        {
            var body1 = new Body2D(1, new Vector2D(0, 0), 1f, 1f);
            var body2 = new Body2D(2, new Vector2D(1.5f, 0), 1f, 1f);
            body1.Velocity = new Vector2D(2, 0);
            body2.Velocity = new Vector2D(-1, 0);
            body1.Restitution = 0f;
            body2.Restitution = 0f;
            var settings = new PhysicsSettings();

            Collision2D.ResolveBodyCollision(body1, body2, settings);

            Assert.Equal(0.5f, body1.Velocity.X, 5);
            Assert.Equal(0.5f, body2.Velocity.X, 5);
        }

        [Fact]
        public void Test_ResolveBodyCollision_DifferentRestitution_UsesMaximum()
        {
            var body1 = new Body2D(1, new Vector2D(0, 0), 1f, 1f);
            var body2 = new Body2D(2, new Vector2D(1.5f, 0), 1f, 1f);
            body1.Velocity = new Vector2D(2, 0);
            body2.Velocity = new Vector2D(-1, 0);
            body1.Restitution = 0.3f;
            body2.Restitution = 0.9f; //maximum restitution
            var settings = new PhysicsSettings();

            Collision2D.ResolveBodyCollision(body1, body2, settings);

            Assert.NotEqual(new Vector2D(2, 0), body1.Velocity);
            Assert.NotEqual(new Vector2D(-1, 0), body2.Velocity);
        }

        [Fact]
        public void Test_ResolveBodyCollision_PositionCorrectionSlop_IgnoresSmallPenetration()
        {
            var body1 = new Body2D(1, new Vector2D(0, 0), 1f, 1f);
            var body2 = new Body2D(2, new Vector2D(1.99f, 0), 1f, 1f); //small penetration
            body1.Velocity = Vector2D.Zero;
            body2.Velocity = Vector2D.Zero;
            var settings = new PhysicsSettings
            {
                PositionCorrectionSlop = 0.02f //more penetration
            };
            var originalPos1 = body1.Position;
            var originalPos2 = body2.Position;

            Collision2D.ResolveBodyCollision(body1, body2, settings);

            Assert.Equal(originalPos1, body1.Position);
            Assert.Equal(originalPos2, body2.Position);
        }

        [Fact]
        public void Test_ResolveBodyCollision_StaticBody1_OnlyBody2Affected()
        {
            var body1 = new Body2D(1, new Vector2D(0, 0), 1f, 1f);
            body1.IsStatic = true;
            var body2 = new Body2D(2, new Vector2D(1.5f, 0), 1f, 1f);
            body2.Velocity = new Vector2D(-1, 0);
            var settings = new PhysicsSettings();
            var originalPos1 = body1.Position;
            var originalVel1 = body1.Velocity;

            Collision2D.ResolveBodyCollision(body1, body2, settings);

            Assert.Equal(originalPos1, body1.Position);
            Assert.Equal(originalVel1, body1.Velocity);
            Assert.NotEqual(new Vector2D(1.5f, 0), body2.Position);
            Assert.NotEqual(new Vector2D(-1, 0), body2.Velocity);
        }

        [Fact]
        public void Test_ResolveBodyCollision_StaticBody2_OnlyBody1Affected()
        {
            var body1 = new Body2D(1, new Vector2D(0, 0), 1f, 1f);
            body1.Velocity = new Vector2D(1, 0);
            var body2 = new Body2D(2, new Vector2D(1.5f, 0), 1f, 1f);
            body2.IsStatic = true;
            var settings = new PhysicsSettings();
            var originalPos2 = body2.Position;
            var originalVel2 = body2.Velocity;

            Collision2D.ResolveBodyCollision(body1, body2, settings);

            Assert.Equal(originalPos2, body2.Position);
            Assert.Equal(originalVel2, body2.Velocity);
            Assert.NotEqual(new Vector2D(0, 0), body1.Position);
            Assert.NotEqual(new Vector2D(1, 0), body1.Velocity);
        }

        [Fact]
        public void Test_ResolveBodyCollision_PositionCorrectionPercent_ScalesCorrection()
        {
            var body1 = new Body2D(1, new Vector2D(0, 0), 1f, 1f);
            var body2 = new Body2D(2, new Vector2D(1.5f, 0), 1f, 1f);
            body1.Velocity = Vector2D.Zero;
            body2.Velocity = Vector2D.Zero;
            var settings1 = new PhysicsSettings { PositionCorrectionPercent = 0.5f };
            var settings2 = new PhysicsSettings { PositionCorrectionPercent = 1.0f };
            
            var body1a = new Body2D(1, new Vector2D(0, 0), 1f, 1f);
            var body2a = new Body2D(2, new Vector2D(1.5f, 0), 1f, 1f);
            var body1b = new Body2D(1, new Vector2D(0, 0), 1f, 1f);
            var body2b = new Body2D(2, new Vector2D(1.5f, 0), 1f, 1f);

            Collision2D.ResolveBodyCollision(body1a, body2a, settings1);
            Collision2D.ResolveBodyCollision(body1b, body2b, settings2);

            var correction1 = Math.Abs(body2a.Position.X - 1.5f);
            var correction2 = Math.Abs(body2b.Position.X - 1.5f);
            Assert.True(correction2 > correction1, "Higher correction percent should result in larger correction");
        }

        #endregion
    }
}