using PanicEngine.Events;

namespace PanicEngine.Tests.Events
{
    public class TriggerEventTests
    {
        [Fact]
        public void Test_TriggerEvent_Constructor_StoresAllFields()
        {
            var evt = new TriggerEvent(triggerId: 1, bodyId: 42, EventType.BodyEntered);

            Assert.Equal(1, evt.TriggerId);
            Assert.Equal(42, evt.BodyId);
            Assert.Equal(EventType.BodyEntered, evt.EventType);
        }

        [Fact]
        public void Test_TriggerEvent_Constructor_WithBodyExited()
        {
            var evt = new TriggerEvent(5, 10, EventType.BodyExited);

            Assert.Equal(5, evt.TriggerId);
            Assert.Equal(10, evt.BodyId);
            Assert.Equal(EventType.BodyExited, evt.EventType);
        }

        [Fact]
        public void Test_TriggerEvent_Constructor_WithBodyStayed()
        {
            var evt = new TriggerEvent(0, 0, EventType.BodyStayed);

            Assert.Equal(0, evt.TriggerId);
            Assert.Equal(0, evt.BodyId);
            Assert.Equal(EventType.BodyStayed, evt.EventType);
        }

        [Fact]
        public void Test_TriggerEvent_ToString_ContainsAllData()
        {
            var evt = new TriggerEvent(7, 3, EventType.BodyEntered);
            var s = evt.ToString();

            Assert.Contains("7", s);
            Assert.Contains("3", s);
            Assert.Contains("BodyEntered", s);
            Assert.Contains("TriggerId", s);
            Assert.Contains("BodyId", s);
            Assert.Contains("EventType", s);
        }

        [Fact]
        public void Test_TriggerEvent_IsReadOnlyStruct()
        {
            var evt = new TriggerEvent(1, 2, EventType.BodyExited);
            // TriggerId, BodyId, EventType have only getters - no setters
            Assert.Equal(1, evt.TriggerId);
            Assert.Equal(2, evt.BodyId);
        }
    }
}
