namespace PanicEngine.Events
{
    public enum PanicEventType
    {
        GoalScored = 0
    }

    public readonly struct PanicEvent
    {
        public readonly PanicEventType Type;
        public readonly int TeamId;

        public PanicEvent(PanicEventType type, int teamId)
        {
            Type = type;
            TeamId = teamId;
        }
    }
}