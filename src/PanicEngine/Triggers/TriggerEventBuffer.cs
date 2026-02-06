using PanicEngine.Events;

namespace PanicEngine.Triggers
{
    public sealed class TriggerEventBuffer
    {
        private readonly List<TriggerEvent> _events = new();
        public IReadOnlyList<TriggerEvent> Events => _events;
        public void AddEvent(TriggerEvent @event)
        {
            _events.Add(@event);
        }

        public void RemoveEvent(TriggerEvent @event)
        {
            _events.Remove(@event);
        }

        public void RemoveEventByTriggerId(int triggerId)
        {
            _events.RemoveAll(e => e.TriggerId == triggerId);
        }

        public void Clear()
        {
            _events.Clear();
        }
    }
}