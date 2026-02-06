using PanicEngine.Triggers;
using System.Collections.Generic;
using PanicEngine.Logger;
using PanicEngine.Physix;

namespace PanicEngine.Core
{
    public sealed class TriggerManager
    {
        private readonly List<Trigger2D> _triggers = new();
        private readonly TriggerEventBuffer _eventBuffer = new();

        public IReadOnlyList<Trigger2D> Triggers => _triggers;
        public TriggerEventBuffer EventBuffer => _eventBuffer;

        public void AddTrigger(Trigger2D trigger)
        {
            if(trigger == null)
            {
                PanicLogger.Error("Trigger is null");
                throw new ArgumentNullException(nameof(trigger));
            }

            _triggers.Add(trigger);
        }

        public void RemoveTrigger(Trigger2D trigger)
        {
            _triggers.Remove(trigger);
        }

        public void RemoveTriggerById(int id)
        {

            foreach(var trigger in _triggers)
            {
                if(trigger.Id.Equals(id))
                {
                    _triggers.Remove(trigger);
                    PanicLogger.Info($"Trigger removed: {id}");
                    return;
                }
            }
        }

        public void ClearTriggers()
        {
            _triggers.Clear();
        }

        public void Update(IReadOnlyList<Body2D> bodies)
        {
            _eventBuffer.Clear();
            foreach(var trigger in _triggers)
            {
                foreach(var body in bodies)
                {
                    trigger.Update(body, _eventBuffer);
                }
            }
        }

        public void Reset()
        {
            _eventBuffer.Clear();

            foreach(var trigger in _triggers)
                trigger.Clear();
        }
    }
}