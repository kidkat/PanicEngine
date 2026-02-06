using PanicEngine.Maths;
using PanicEngine.Maths.Shapes;
using PanicEngine.Events;
using PanicEngine.Physix;
using PanicEngine.Triggers;

namespace PanicEngine.Tests.Triggers
{
    /// <summary>
    /// Concrete implementation of Trigger2D for unit testing (abstract class cannot be instantiated directly).
    /// </summary>
    internal sealed class TestableTrigger2D : Trigger2D
    {
        public TestableTrigger2D(int id, IShape2D shape)
            : base(id, shape)
        {
        }
    }

    public class Trigger2DTests
    {
        private static (TriggerEventBuffer buffer, TestableTrigger2D trigger, Rect2D shape) CreateTriggerInRect(float minX, float minY, float maxX, float maxY, int triggerId = 1)
        {
            var buffer = new TriggerEventBuffer();
            var shape = new Rect2D(minX, minY, maxX, maxY);
            var trigger = new TestableTrigger2D(triggerId, shape);
            return (buffer, trigger, shape);
        }

        private static Body2D CreateBody(int id, float x, float y)
        {
            return new Body2D(id, new Vector2D(x, y), 1f, 1f);
        }

        #region BodyEntered

        [Fact]
        public void Test_Trigger2D_Update_EmitsBodyEntered_WhenBodyEnters()
        {
            var (buffer, trigger, _) = CreateTriggerInRect(0, 0, 10, 10);
            var body = CreateBody(1, 5, 5);

            trigger.Update(body, buffer);

            Assert.Single(buffer.Events);
            Assert.Equal(EventType.BodyEntered, buffer.Events[0].EventType);
            Assert.Equal(trigger.Id, buffer.Events[0].TriggerId);
            Assert.Equal(body.Id, buffer.Events[0].BodyId);
        }

        [Fact]
        public void Test_Trigger2D_Update_NoEvent_WhenBodyStartsOutside()
        {
            var (buffer, trigger, _) = CreateTriggerInRect(0, 0, 10, 10);
            var body = CreateBody(1, 20, 20);

            trigger.Update(body, buffer);

            Assert.Empty(buffer.Events);
        }

        [Fact]
        public void Test_Trigger2D_Update_EmitsBodyEntered_WhenBodyMovesFromOutsideToInside()
        {
            var (buffer, trigger, _) = CreateTriggerInRect(0, 0, 10, 10);
            var body = CreateBody(1, 20, 20);
            trigger.Update(body, buffer);
            Assert.Empty(buffer.Events);

            body.Position = new Vector2D(5, 5);
            trigger.Update(body, buffer);

            Assert.Single(buffer.Events);
            Assert.Equal(EventType.BodyEntered, buffer.Events[0].EventType);
        }

        #endregion

        #region BodyStayed

        [Fact]
        public void Test_Trigger2D_Update_EmitsBodyStayed_WhenBodyRemainsInside()
        {
            var (buffer, trigger, _) = CreateTriggerInRect(0, 0, 10, 10);
            var body = CreateBody(1, 5, 5);
            trigger.Update(body, buffer);
            Assert.Single(buffer.Events);

            trigger.Update(body, buffer);

            Assert.Equal(2, buffer.Events.Count);
            Assert.Equal(EventType.BodyStayed, buffer.Events[1].EventType);
        }

        [Fact]
        public void Test_Trigger2D_Update_EmitsBodyStayed_MultipleFrames()
        {
            var (buffer, trigger, _) = CreateTriggerInRect(0, 0, 10, 10);
            var body = CreateBody(1, 5, 5);
            trigger.Update(body, buffer);
            trigger.Update(body, buffer);
            trigger.Update(body, buffer);

            Assert.Equal(3, buffer.Events.Count);
            Assert.Equal(EventType.BodyEntered, buffer.Events[0].EventType);
            Assert.Equal(EventType.BodyStayed, buffer.Events[1].EventType);
            Assert.Equal(EventType.BodyStayed, buffer.Events[2].EventType);
        }

        #endregion

        #region BodyExited

        [Fact]
        public void Test_Trigger2D_Update_EmitsBodyExited_WhenBodyLeaves()
        {
            var (buffer, trigger, _) = CreateTriggerInRect(0, 0, 10, 10);
            var body = CreateBody(1, 5, 5);
            trigger.Update(body, buffer);
            trigger.Update(body, buffer);
            Assert.Equal(2, buffer.Events.Count);

            body.Position = new Vector2D(20, 20);
            trigger.Update(body, buffer);

            Assert.Equal(3, buffer.Events.Count);
            Assert.Equal(EventType.BodyExited, buffer.Events[2].EventType);
        }

        [Fact]
        public void Test_Trigger2D_Update_NoEvent_WhenBodyStaysOutside()
        {
            var (buffer, trigger, _) = CreateTriggerInRect(0, 0, 10, 10);
            var body = CreateBody(1, 5, 5);
            trigger.Update(body, buffer);
            body.Position = new Vector2D(20, 20);
            trigger.Update(body, buffer);
            Assert.Equal(2, buffer.Events.Count); // Entered + Exited

            trigger.Update(body, buffer);

            Assert.Equal(2, buffer.Events.Count);
        }

        [Fact]
        public void Test_Trigger2D_Update_EmitsBodyEnteredAgain_AfterReEnter()
        {
            var (buffer, trigger, _) = CreateTriggerInRect(0, 0, 10, 10);
            var body = CreateBody(1, 5, 5);
            trigger.Update(body, buffer);
            body.Position = new Vector2D(20, 20);
            trigger.Update(body, buffer);
            body.Position = new Vector2D(5, 5);
            trigger.Update(body, buffer);

            Assert.Equal(3, buffer.Events.Count);
            Assert.Equal(EventType.BodyEntered, buffer.Events[0].EventType);
            Assert.Equal(EventType.BodyExited, buffer.Events[1].EventType);
            Assert.Equal(EventType.BodyEntered, buffer.Events[2].EventType);
        }

        #endregion

        #region Multiple bodies

        [Fact]
        public void Test_Trigger2D_Update_TracksMultipleBodies()
        {
            var (buffer, trigger, _) = CreateTriggerInRect(0, 0, 10, 10);
            var body1 = CreateBody(1, 2, 2);
            var body2 = CreateBody(2, 3, 3);

            trigger.Update(body1, buffer);
            trigger.Update(body2, buffer);

            Assert.Equal(2, buffer.Events.Count);
            Assert.Equal(EventType.BodyEntered, buffer.Events[0].EventType);
            Assert.Equal(1, buffer.Events[0].BodyId);
            Assert.Equal(EventType.BodyEntered, buffer.Events[1].EventType);
            Assert.Equal(2, buffer.Events[1].BodyId);
        }

        [Fact]
        public void Test_Trigger2D_Update_EachBodyStayedAndExitedIndependently()
        {
            var (buffer, trigger, _) = CreateTriggerInRect(0, 0, 10, 10);
            var body1 = CreateBody(1, 2, 2);
            var body2 = CreateBody(2, 5, 5);
            trigger.Update(body1, buffer);
            trigger.Update(body2, buffer);
            trigger.Update(body1, buffer);
            trigger.Update(body2, buffer); // both stayed
            body1.Position = new Vector2D(20, 20);
            trigger.Update(body1, buffer); // body1 exited
            trigger.Update(body2, buffer); // body2 stayed

            Assert.Equal(6, buffer.Events.Count);
            Assert.Equal(EventType.BodyEntered, buffer.Events[0].EventType);
            Assert.Equal(EventType.BodyEntered, buffer.Events[1].EventType);
            Assert.Equal(EventType.BodyStayed, buffer.Events[2].EventType);
            Assert.Equal(EventType.BodyStayed, buffer.Events[3].EventType);
            Assert.Equal(EventType.BodyExited, buffer.Events[4].EventType);
            Assert.Equal(1, buffer.Events[4].BodyId);
            Assert.Equal(EventType.BodyStayed, buffer.Events[5].EventType);
            Assert.Equal(2, buffer.Events[5].BodyId);
        }

        #endregion

        #region Clear

        [Fact]
        public void Test_Trigger2D_Clear_AllowsReEnter()
        {
            var (buffer, trigger, _) = CreateTriggerInRect(0, 0, 10, 10);
            var body = CreateBody(1, 5, 5);
            trigger.Update(body, buffer);
            Assert.Single(buffer.Events);

            trigger.Clear();
            trigger.Update(body, buffer);

            Assert.Equal(2, buffer.Events.Count);
            Assert.Equal(EventType.BodyEntered, buffer.Events[1].EventType);
        }

        [Fact]
        public void Test_Trigger2D_Clear_DoesNotClearEventBuffer()
        {
            var (buffer, trigger, _) = CreateTriggerInRect(0, 0, 10, 10);
            var body = CreateBody(1, 5, 5);
            trigger.Update(body, buffer);
            trigger.Clear();

            Assert.Single(buffer.Events);
        }

        #endregion

        #region Id and boundary

        [Fact]
        public void Test_Trigger2D_Id_IsStored()
        {
            var (_, trigger, _) = CreateTriggerInRect(0, 0, 10, 10);
            Assert.Equal(1, trigger.Id);

            var shape2 = new Rect2D(0, 0, 1, 1);
            var trigger2 = new TestableTrigger2D(99, shape2);
            Assert.Equal(99, trigger2.Id);
        }

        [Fact]
        public void Test_Trigger2D_Update_BodyOnBoundary_IsInside()
        {
            var (buffer, trigger, _) = CreateTriggerInRect(0, 0, 10, 10);
            var body = CreateBody(1, 0, 0); // Rect2D contains min boundary

            trigger.Update(body, buffer);

            Assert.Single(buffer.Events);
            Assert.Equal(EventType.BodyEntered, buffer.Events[0].EventType);
        }

        #endregion
    }
}
