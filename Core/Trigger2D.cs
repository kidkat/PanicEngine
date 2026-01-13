using PanicEngine.Maths;
using PanicEngine.Events;

namespace PanicEngine.Core
{
    public sealed class Trigger2D
    {
        public int Id { get; }
        public string Tag { get; }
        public int Data { get; }
        public Rect2D Area { get; }

        private readonly HashSet<int> _insideBodies = new();

        public Trigger2D(int id, string tag, int data, Rect2D area)
        {
            Id = id;
            Tag = tag ?? string.Empty;
            Data = data;
            Area = area;
        }

        /// <summary>
        /// Обновить состояние триггера для конкретного тела и (если нужно) добавить события.
        /// Вызывается из FieldManager каждый тик.
        /// </summary>
        public void Update(Body2D body, List<TriggerEvent> events)
        {
            bool isInside = IntersectsCircle(body.Position, body.Radius);
            bool wasInside = _insideBodies.Contains(body.Id);

            if(!wasInside && isInside)
            {
                _insideBodies.Add(body.Id);
                events.Add(new TriggerEvent(TriggerEventType.Entered, Id, body.Id, Tag));
                return;
            }

            if(wasInside && isInside)
            {
                events.Add(new TriggerEvent(TriggerEventType.Stayed, Id, body.Id, Tag));
            }

            if(wasInside && !isInside)
            {
                _insideBodies.Remove(body.Id);
                events.Add(new TriggerEvent(TriggerEventType.Exited, Id, body.Id, Tag));
            }
        }

        /// <summary>
        /// Если тело удалили из матча — желательно почистить его из триггеров.
        /// </summary>
        public void RemoveBody(int bodyId)
        {
            _insideBodies.Remove(bodyId);
        }

        /// <summary>
        /// Circle vs AABB intersection: проверяем, пересекает ли круг прямоугольник.
        /// Это работает и для "ворот", и для любых зон.
        /// </summary>
        private bool IntersectsCircle(Vector2D center, float radius)
        {
            float closestX = Area.ClampX(center.X);
            float closestY = Area.ClampY(center.Y);

            float distanceX = center.X - closestX;
            float distanceY = center.Y - closestY;

            return (distanceX * distanceX + distanceY * distanceY) <= (radius * radius);
        }
    }
}