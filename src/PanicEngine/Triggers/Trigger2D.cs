using PanicEngine.Maths.Shapes;
using PanicEngine.Events;
using PanicEngine.Physix;
using System.Collections.Generic;

namespace PanicEngine.Triggers
{
    public abstract class Trigger2D
    {
        public int Id { get; }
        protected readonly IShape2D _shape;
        
        protected readonly HashSet<int> _bodiesInside = new();

        protected Trigger2D(int id, IShape2D shape)
        {
            Id = id;
            _shape = shape;
        }

        public void Update(Body2D body, TriggerEventBuffer eventBuffer)
        {
            bool isInside = _shape.Contains(body.Position);
            bool wasInside = _bodiesInside.Contains(body.Id);

            if(isInside && !wasInside)
            {
                _bodiesInside.Add(body.Id);
                EmitEvent(body, eventBuffer, EventType.BodyEntered);
                OnEntered(body);
            }
            else if(isInside && wasInside)
            {
                EmitEvent(body, eventBuffer, EventType.BodyStayed);
                OnStayed(body);
            }
            else if(!isInside && wasInside)
            {
                _bodiesInside.Remove(body.Id);
                EmitEvent(body, eventBuffer, EventType.BodyExited);
                OnExited(body);
            }
        }
        
        private void EmitEvent(Body2D body, TriggerEventBuffer eventBuffer, EventType eventType)
        {
            eventBuffer.AddEvent(new TriggerEvent(Id, body.Id, eventType));
        }

        public virtual void Clear()
        {
            _bodiesInside.Clear();
        }

        protected virtual void OnEntered(Body2D body)
        { }

        protected virtual void OnExited(Body2D body)
        { }

        protected virtual void OnStayed(Body2D body)
        { }
    }
}