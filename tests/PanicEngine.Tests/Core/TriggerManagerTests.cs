using Xunit;
using PanicEngine.Core;
using PanicEngine.Triggers;
using PanicEngine.Maths;
using PanicEngine.Maths.Shapes;
using PanicEngine.Events;
using PanicEngine.Physix;

namespace PanicEngine.Tests.Core
{
    /// <summary>
    /// Concrete Trigger2D for testing TriggerManager (abstract class cannot be instantiated directly).
    /// </summary>
    internal sealed class TestTrigger2DForManager : Trigger2D
    {
        public TestTrigger2DForManager(int id, IShape2D shape)
            : base(id, shape)
        {
        }
    }

    public class TriggerManagerTests
    {
        private static Trigger2D CreateTrigger(int id, float minX, float minY, float maxX, float maxY)
        {
            var shape = new Rect2D(minX, minY, maxX, maxY);
            return new TestTrigger2DForManager(id, shape);
        }

        private static Body2D CreateBody(int id, float x, float y)
        {
            return new Body2D(id, new Vector2D(x, y), 1f, 1f);
        }

        // --- AddTrigger ---
        [Fact]
        public void AddTrigger_AddsTriggerToList()
        {
            var manager = new TriggerManager();
            var trigger = CreateTrigger(1, 0, 0, 10, 10);
            manager.AddTrigger(trigger);
            Assert.Single(manager.Triggers);
            Assert.Same(trigger, manager.Triggers[0]);
        }

        [Fact]
        public void AddTrigger_Null_ThrowsArgumentNullException()
        {
            var manager = new TriggerManager();
            Assert.Throws<ArgumentNullException>(() => manager.AddTrigger(null!));
        }

        [Fact]
        public void AddTrigger_MultipleTriggers_AllStored()
        {
            var manager = new TriggerManager();
            var t1 = CreateTrigger(1, 0, 0, 10, 10);
            var t2 = CreateTrigger(2, 20, 20, 30, 30);
            manager.AddTrigger(t1);
            manager.AddTrigger(t2);
            Assert.Equal(2, manager.Triggers.Count);
            Assert.Same(t1, manager.Triggers[0]);
            Assert.Same(t2, manager.Triggers[1]);
        }

        // --- RemoveTrigger ---
        [Fact]
        public void RemoveTrigger_RemovesExistingTrigger()
        {
            var manager = new TriggerManager();
            var trigger = CreateTrigger(1, 0, 0, 10, 10);
            manager.AddTrigger(trigger);
            manager.RemoveTrigger(trigger);
            Assert.Empty(manager.Triggers);
        }

        [Fact]
        public void RemoveTrigger_NonExistentTrigger_DoesNotThrow()
        {
            var manager = new TriggerManager();
            var trigger = CreateTrigger(1, 0, 0, 10, 10);
            manager.RemoveTrigger(trigger);
            Assert.Empty(manager.Triggers);
        }

        // --- RemoveTriggerById ---
        [Fact]
        public void RemoveTriggerById_ExistingId_RemovesTrigger()
        {
            var manager = new TriggerManager();
            var trigger = CreateTrigger(42, 0, 0, 10, 10);
            manager.AddTrigger(trigger);
            manager.RemoveTriggerById(42);
            Assert.Empty(manager.Triggers);
        }

        [Fact]
        public void RemoveTriggerById_NonExistentId_DoesNothing()
        {
            var manager = new TriggerManager();
            var trigger = CreateTrigger(1, 0, 0, 10, 10);
            manager.AddTrigger(trigger);
            manager.RemoveTriggerById(999);
            Assert.Single(manager.Triggers);
        }

        [Fact]
        public void RemoveTriggerById_EmptyList_DoesNotThrow()
        {
            var manager = new TriggerManager();
            manager.RemoveTriggerById(1);
            Assert.Empty(manager.Triggers);
        }

        [Fact]
        public void RemoveTriggerById_RemovesOnlyFirstMatch()
        {
            var manager = new TriggerManager();
            var t1 = CreateTrigger(5, 0, 0, 10, 10);
            var t2 = CreateTrigger(5, 20, 20, 30, 30);
            manager.AddTrigger(t1);
            manager.AddTrigger(t2);
            manager.RemoveTriggerById(5);
            Assert.Single(manager.Triggers);
            Assert.Same(t2, manager.Triggers[0]);
        }

        // --- ClearTriggers ---
        [Fact]
        public void ClearTriggers_RemovesAllTriggers()
        {
            var manager = new TriggerManager();
            manager.AddTrigger(CreateTrigger(1, 0, 0, 10, 10));
            manager.AddTrigger(CreateTrigger(2, 20, 20, 30, 30));
            manager.ClearTriggers();
            Assert.Empty(manager.Triggers);
        }

        [Fact]
        public void ClearTriggers_EmptyList_DoesNotThrow()
        {
            var manager = new TriggerManager();
            manager.ClearTriggers();
            Assert.Empty(manager.Triggers);
        }

        // --- EventBuffer ---
        [Fact]
        public void EventBuffer_IsExposed()
        {
            var manager = new TriggerManager();
            Assert.NotNull(manager.EventBuffer);
            Assert.Empty(manager.EventBuffer.Events);
        }

        // --- Update ---
        [Fact]
        public void Update_ClearsEventBufferBeforeProcessing()
        {
            var manager = new TriggerManager();
            manager.AddTrigger(CreateTrigger(1, 0, 0, 10, 10));
            var body = CreateBody(1, 5, 5);
            var bodies = new List<Body2D> { body };
            manager.Update(bodies);
            Assert.Single(manager.EventBuffer.Events);

            manager.Update(bodies);
            Assert.Single(manager.EventBuffer.Events);
        }

        [Fact]
        public void Update_WithBodyInsideTrigger_EmitsBodyEntered()
        {
            var manager = new TriggerManager();
            manager.AddTrigger(CreateTrigger(10, 0, 0, 10, 10));
            var body = CreateBody(1, 5, 5);
            manager.Update(new List<Body2D> { body });

            Assert.Single(manager.EventBuffer.Events);
            Assert.Equal(10, manager.EventBuffer.Events[0].TriggerId);
            Assert.Equal(1, manager.EventBuffer.Events[0].BodyId);
            Assert.Equal(EventType.BodyEntered, manager.EventBuffer.Events[0].EventType);
        }

        [Fact]
        public void Update_WithBodyOutsideTrigger_NoEvents()
        {
            var manager = new TriggerManager();
            manager.AddTrigger(CreateTrigger(1, 0, 0, 10, 10));
            var body = CreateBody(1, 20, 20);
            manager.Update(new List<Body2D> { body });

            Assert.Empty(manager.EventBuffer.Events);
        }

        [Fact]
        public void Update_EmptyBodies_DoesNotThrow()
        {
            var manager = new TriggerManager();
            manager.AddTrigger(CreateTrigger(1, 0, 0, 10, 10));
            manager.Update(new List<Body2D>());
            Assert.Empty(manager.EventBuffer.Events);
        }

        [Fact]
        public void Update_NoTriggers_DoesNotThrow()
        {
            var manager = new TriggerManager();
            var body = CreateBody(1, 5, 5);
            manager.Update(new List<Body2D> { body });
            Assert.Empty(manager.EventBuffer.Events);
        }

        [Fact]
        public void Update_MultipleTriggersAndBodies_ProcessesAll()
        {
            var manager = new TriggerManager();
            manager.AddTrigger(CreateTrigger(1, 0, 0, 10, 10));
            manager.AddTrigger(CreateTrigger(2, 15, 15, 25, 25));
            var body1 = CreateBody(1, 5, 5);
            var body2 = CreateBody(2, 20, 20);
            manager.Update(new List<Body2D> { body1, body2 });

            Assert.Equal(2, manager.EventBuffer.Events.Count);
            Assert.Contains(manager.EventBuffer.Events, e => e.TriggerId == 1 && e.BodyId == 1 && e.EventType == EventType.BodyEntered);
            Assert.Contains(manager.EventBuffer.Events, e => e.TriggerId == 2 && e.BodyId == 2 && e.EventType == EventType.BodyEntered);
        }

        // --- Reset ---
        [Fact]
        public void Reset_ClearsEventBuffer()
        {
            var manager = new TriggerManager();
            manager.AddTrigger(CreateTrigger(1, 0, 0, 10, 10));
            var body = CreateBody(1, 5, 5);
            manager.Update(new List<Body2D> { body });
            Assert.Single(manager.EventBuffer.Events);

            manager.Reset();
            Assert.Empty(manager.EventBuffer.Events);
        }

        [Fact]
        public void Reset_EmptyManager_DoesNotThrow()
        {
            var manager = new TriggerManager();
            manager.Reset();
            Assert.Empty(manager.EventBuffer.Events);
        }
    }
}
