using UnityEngine;
using PanicEngine.Core;
using PanicEngine.Physix;
using PanicEngine.Maths;
using PanicEngine.Logger;

[ExecuteAlways]
public class Body2DView : MonoBehaviour
{
    [Header("Engine mapping")]
    public int bodyId;
    public float radius = 0.4f;
    public float mass = 1f;

    [Header("Physics params")]
    [Range(0f, 1f)] public float restitution = 0.6f;
    [Range(0f, 1f)] public float linearDamping = 0.5f;
    public float sleepSpeed = 0.02f;

    [Header("Initial state")]
    public Vector2 initialVelocity;

    public Body2D? Body { get; private set; }

    public void CreateBody(BodiesManager bodiesManager)
    {
        SyncScaleFromRadius();

        Body = new Body2D(
            id: bodyId,
            position: VectorConverter.FromVector3ToVector2D(transform.position),
            mass: mass,
            radius: radius
        );

        Body.Restitution = restitution;
        Body.LinearDamping = linearDamping;
        Body.SleepSpeed = sleepSpeed;
        Body.Velocity = VectorConverter.FromVector3ToVector2D(initialVelocity);

        bodiesManager.AddBody(Body);
    }

    public void SyncFromEngine()
    {
        if (Body == null) return;

        Vector2D position = Body.Position;
        transform.position = new Vector3(position.X, position.Y, transform.position.z);
    }

    private void SyncScaleFromRadius()
    {
        float diameter = radius * 2f;
        transform.localScale = new Vector3(diameter, diameter, 1f);
    }

#if UNITY_EDITOR
    private void OnValidate()
    {
        if (radius < 0f) radius = 0f;
        SyncScaleFromRadius();
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, radius);
    }
#endif
}
