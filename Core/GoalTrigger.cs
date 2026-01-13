using PanicEngine.Maths;

namespace PanicEngine.Core
{
    public sealed class GoalTrigger
    {
        public int TeamId { get; }
        public Rect2D Area { get; }

        public GoalTrigger(int teamId, Rect2D area)
        {
            TeamId = teamId;
            Area = area;
        }

        public bool IsInside(Body2D body)
        {
            return Area.Contains(body.Position);
        }
    }
}