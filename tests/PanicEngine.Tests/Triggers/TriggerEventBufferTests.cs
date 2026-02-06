using PanicEngine.Events;
using PanicEngine.Triggers;

namespace PanicEngine.Tests.Triggers
{
    public class TriggerEventBufferTests
    {
        [Fact]
        public void Test_TriggerEventBuffer_Events_InitiallyEmpty()
        {
            var buffer = new TriggerEventBuffer();

            Assert.Empty(buffer.Events);
        }

        [Fact]
        public void Test_TriggerEventBuffer_AddEvent_AddsToList()
        {
            var buffer = new TriggerEventBuffer();
            var evt = new TriggerEvent(1, 10, EventType.BodyEntered);

            buffer.AddEvent(evt);

            Assert.Single(buffer.Events);
            Assert.Equal(1, buffer.Events[0].TriggerId);
            Assert.Equal(10, buffer.Events[0].BodyId);
            Assert.Equal(EventType.BodyEntered, buffer.Events[0].EventType);
        }

        [Fact]
        public void Test_TriggerEventBuffer_AddEvent_MultipleEvents()
        {
            var buffer = new TriggerEventBuffer();

            buffer.AddEvent(new TriggerEvent(1, 1, EventType.BodyEntered));
            buffer.AddEvent(new TriggerEvent(1, 1, EventType.BodyStayed));
            buffer.AddEvent(new TriggerEvent(2, 5, EventType.BodyExited));

            Assert.Equal(3, buffer.Events.Count);
            Assert.Equal(EventType.BodyEntered, buffer.Events[0].EventType);
            Assert.Equal(EventType.BodyStayed, buffer.Events[1].EventType);
            Assert.Equal(EventType.BodyExited, buffer.Events[2].EventType);
            Assert.Equal(2, buffer.Events[2].TriggerId);
            Assert.Equal(5, buffer.Events[2].BodyId);
        }

        [Fact]
        public void Test_TriggerEventBuffer_RemoveEvent_RemovesExactEvent()
        {
            var buffer = new TriggerEventBuffer();
            var evt = new TriggerEvent(1, 10, EventType.BodyEntered);
            buffer.AddEvent(evt);
            buffer.AddEvent(new TriggerEvent(2, 20, EventType.BodyExited));

            buffer.RemoveEvent(evt);

            Assert.Single(buffer.Events);
            Assert.Equal(2, buffer.Events[0].TriggerId);
            Assert.Equal(20, buffer.Events[0].BodyId);
        }

        [Fact]
        public void Test_TriggerEventBuffer_RemoveEvent_NoOpWhenEventNotPresent()
        {
            var buffer = new TriggerEventBuffer();
            buffer.AddEvent(new TriggerEvent(1, 1, EventType.BodyEntered));

            buffer.RemoveEvent(new TriggerEvent(99, 99, EventType.BodyExited));

            Assert.Single(buffer.Events);
        }

        [Fact]
        public void Test_TriggerEventBuffer_RemoveEventByTriggerId_RemovesAllForTrigger()
        {
            var buffer = new TriggerEventBuffer();
            buffer.AddEvent(new TriggerEvent(1, 10, EventType.BodyEntered));
            buffer.AddEvent(new TriggerEvent(1, 10, EventType.BodyStayed));
            buffer.AddEvent(new TriggerEvent(2, 20, EventType.BodyEntered));
            buffer.AddEvent(new TriggerEvent(1, 11, EventType.BodyExited));

            buffer.RemoveEventByTriggerId(1);

            Assert.Single(buffer.Events);
            Assert.Equal(2, buffer.Events[0].TriggerId);
            Assert.Equal(20, buffer.Events[0].BodyId);
        }

        [Fact]
        public void Test_TriggerEventBuffer_RemoveEventByTriggerId_NoOpWhenTriggerNotPresent()
        {
            var buffer = new TriggerEventBuffer();
            buffer.AddEvent(new TriggerEvent(5, 1, EventType.BodyEntered));

            buffer.RemoveEventByTriggerId(99);

            Assert.Single(buffer.Events);
        }

        [Fact]
        public void Test_TriggerEventBuffer_Clear_RemovesAllEvents()
        {
            var buffer = new TriggerEventBuffer();
            buffer.AddEvent(new TriggerEvent(1, 1, EventType.BodyEntered));
            buffer.AddEvent(new TriggerEvent(2, 2, EventType.BodyExited));

            buffer.Clear();

            Assert.Empty(buffer.Events);
        }

        [Fact]
        public void Test_TriggerEventBuffer_Clear_AllowsReuse()
        {
            var buffer = new TriggerEventBuffer();
            buffer.AddEvent(new TriggerEvent(1, 1, EventType.BodyEntered));
            buffer.Clear();

            buffer.AddEvent(new TriggerEvent(2, 2, EventType.BodyExited));

            Assert.Single(buffer.Events);
            Assert.Equal(2, buffer.Events[0].TriggerId);
        }
    }
}
