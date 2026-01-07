using PanicEngine.Maths;

namespace PanicEngine.Core;

public sealed class FieldManager
{
    private readonly List<Body2D> _bodies = new();
    public FieldBounds FieldBounds { get; }
    public IReadOnlyList<Body2D> Bodies => _bodies;

    public float MaxSpeed { get; } = 50f;
    public int SolverIterations { get; set; } = 1;

    public FieldManager(FieldBounds fieldBounds)
    {
        FieldBounds = fieldBounds;
    }

    public void AddBody(Body2D body)
    {
        if(body == null) throw new ArgumentNullException(nameof(body));
        _bodies.Add(body);
    }

    public void RemoveBody(Body2D body)
    {
        if(body == null) throw new ArgumentNullException(nameof(body));
        _bodies.Remove(body);
    }

    public void ClearBodies()
    {
        _bodies.Clear();
    }
}