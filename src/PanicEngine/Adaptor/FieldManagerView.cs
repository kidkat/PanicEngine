using UnityEngine;
using PanicEngine.Core;
using PanicEngine.Physix;
using System;

public class FieldManagerView : MonoBehaviour
{
    [Header("Field bounds (Unity units)")]
    public Vector2 min = new(-8f, -4f);
    public Vector2 max = new(8f, 4f);

    [Header("Simulation")]
    [Tooltip("Recommendation: 1/60")]
    public float fixedDt = 1f / 60f;

    [Header("Solver settings")]
    public int solverIterations = 2;
    public float maxSpeed = 50f;
    public float positionCorrectionPercent = 0.8f;
    public float positionCorrectionSlop = 0.01f;

    [Header("Test impulse")]
    public bool applyImpulseOnAwake = true;
    public int testBodyId = 0;
    public Vector2 testImpulse = new(5f, 2f);

    [Header("Auto register bodies")]
    public bool autoFindBodiesOnAwake = true;
    public bool includeInactiveBodies = true;

    public FieldManager Field { get; private set; }
    private Body2DView[] _bodies;

    [Obsolete]
    private void Awake()
    {
        UnityPanicLogger.Install();
        CreateField();

        if (autoFindBodiesOnAwake)
            RegisterAllBodiesInScene();

        if (applyImpulseOnAwake)
            ApplyTestImpulseContext();
    }

    private void FixedUpdate()
    {
        if (Field == null) return;

        Field.Step(Time.deltaTime);

        if (_bodies != null)
        {
            foreach (var body in _bodies)
                body.SyncFromEngine();
        }
    }

    public void CreateField()
    {
        Field = new FieldManager(new FieldBounds(min.x, min.y, max.x, max.y));

        Field.Settings.SolverIterations = solverIterations;
        Field.Settings.MaxSpeed = maxSpeed;
        Field.Settings.PositionCorrectionPercent = positionCorrectionPercent;
        Field.Settings.PositionCorrectionSlop = positionCorrectionSlop;
    }

    [Obsolete]
    public void RegisterAllBodiesInScene()
    {
        _bodies = FindObjectsOfType<Body2DView>(includeInactiveBodies);

        foreach (var body in _bodies)
            RegisterBody(body);
    }

    public void RegisterBody(Body2DView body)
    {
        if (body == null) return;

        if (body.Body != null) return;

        body.CreateBody(Field.BodiesManager);
    }

    [ContextMenu("Apply Test Impulse")]
    private void ApplyTestImpulseContext()
    {
        if (Field == null) return;

        var body = Field.BodiesManager.GetBodyById(testBodyId);
        body?.ApplyImpulse(VectorConverter.ToPanicVector2D(testImpulse));
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;

        Vector3 a = (Vector3)min;
        Vector3 b = new(max.x, min.y, 0f);
        Vector3 c = (Vector3)max;
        Vector3 d = new(min.x, max.y, 0f);

        Gizmos.DrawLine(a, b);
        Gizmos.DrawLine(b, c);
        Gizmos.DrawLine(c, d);
        Gizmos.DrawLine(d, a);
    }
#endif
}
