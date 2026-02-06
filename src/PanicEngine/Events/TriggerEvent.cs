namespace PanicEngine.Events
{
    public readonly struct TriggerEvent
    {
        public int TriggerId { get; }
        public int BodyId { get; }
        public EventType EventType { get; }

        public TriggerEvent(int triggerId, int bodyId, EventType eventType)
        {
            TriggerId = triggerId;
            BodyId = bodyId;
            EventType = eventType;
        }

        public override string ToString()
        {
            return $"TriggerEvent: TriggerId = {TriggerId}, BodyId = {BodyId}, EventType = {EventType}";
        }
    }
}