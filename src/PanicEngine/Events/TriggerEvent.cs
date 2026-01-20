namespace PanicEngine.Events
{
    public readonly struct TriggerEvent
    {
        public readonly TriggerEventType Type;
        public readonly int TriggerId;
        public readonly int BodyId;
        public readonly string Tag;

        public TriggerEvent(TriggerEventType type, int triggerId, int bodyId, string tag)
        {
            Type = type;
            TriggerId = triggerId;
            BodyId = bodyId;
            Tag = tag;
        }

        public override string ToString()
        {
            return $"TriggerEvent: Type={Type}, TriggerId={TriggerId}, BodyId={BodyId}, Tag={Tag}";
        }
    }
}