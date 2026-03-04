using PanicEngine.Maths;
using PanicEngine.Physix;

namespace PanicEngine.Snapshots
{
    public readonly struct BodyStateSnapshot
    {
        public int Id { get; }
        public Vector2D Position { get; }
        public Vector2D Velocity { get; }
        public float Radius { get; }
        public bool IsSleeping { get; }
        public bool IsStatic { get; }

        public BodyStateSnapshot(int id, Vector2D position, Vector2D velocity, float radius, bool isSleeping, bool isStatic)
        {
            Id = id;
            Position = position;
            Velocity = velocity;
            Radius = radius;
            IsSleeping = isSleeping;
            IsStatic = isStatic;
        }

        public static BodyStateSnapshot GetSnapshot(Body2D body)
        {
            return new BodyStateSnapshot(body.Id, body.Position, body.Velocity, body.Radius, body.IsSleeping, body.IsStatic);
        }
    }
}