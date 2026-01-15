using UnityEngine;
using System.Collections.Generic;
using PanicEngine.Core;
using PanicEngine.Maths;
using PanicEngine.Logger;
using System;

public class FieldManagerUnityTest : MonoBehaviour
{
    [Header("Bounds (world units)")]
    public Vector2 min = new Vector2(-8f, -4f);
    public Vector2 max = new Vector2( 8f,  4f);

    [Header("Ball Visual")]
    public Transform ballVisual;

    [Header("Ball Physics")]
    public float ballRadius = 0.4f;
    public float ballMass = 0.5f;
    public Vector2 initialVelocity = new Vector2(6f, 3f);

    [Header("Material")]
    [Range(0f, 1f)] public float restitution = 0.9f;
    public float linearDamping = 0.02f;
    public float sleepSpeed = 0.01f;

    [Header("Simulation")]
    public int solverIterations = 1;
    public float maxSpeed = 50f;

    // фиксированный dt для детерминизма
    public bool fixedDt = true;
    public float dt = 1f / 60f;

    private FieldManager _field;
    private Body2D _ball;

    void Start()
    {
        PanicLogger.LogHandler = msg =>
        {
            Debug.Log(msg);
        };

        Debug.Log("BallVisual assigned: " + (ballVisual != null));
        Debug.Log("BallVisual start pos (Unity): " + (ballVisual != null ? ballVisual.position.ToString() : "NULL"));

        Debug.Log("Inspector min=" + min + " max=" + max);

        var fb = new FieldBounds(min.x, max.x, max.y, min.y);
        var fbn = fb.Normalized();
        // Принудительно стартуем в центре поля (для теста)
        Vector2 center = new Vector2(
            (fbn.Left + fbn.Right) * 0.5f,
            (fbn.Top + fbn.Bottom) * 0.5f
        );

        if (ballVisual != null)
            ballVisual.position = new Vector3(center.x, center.y, ballVisual.position.z);

        Debug.Log("Bounds normalized: min=(" + fbn.Left + "," + fbn.Top + ") max=(" + fbn.Right + "," + fbn.Bottom + ")");
        Debug.Log("Inspector initialVelocity=" + initialVelocity);  

        _field = new FieldManager(fbn);
        _field.SolverIterations = solverIterations;


        _ball = new Body2D(
            id: 1,
            position: ((Vector2)ballVisual.position).ToPanic(),
            shape: Shape2D.Circle(ballRadius),
            mass: ballMass
        );

        _ball.Restitution = restitution;
        _ball.LinearDamping = linearDamping;
        _ball.SleepSpeed = sleepSpeed;

        _ball.Velocity = initialVelocity.ToPanic();
        Debug.Log("Ball physics velocity after set: " + _ball.Velocity.X + " : " + _ball.Velocity.Y);

        _field.AddBody(_ball);
        PanicLogger.Debug($"Start Ball position: {_ball.Position.X} {_ball.Position.Y}");
    }

    void FixedUpdate()
    {
        PanicLogger.Debug($"On Start Ball position: {_ball.Position.X} {_ball.Position.Y}");
        float stepDt = fixedDt ? dt : Time.fixedDeltaTime;
        PanicLogger.Debug($"Step dt: {stepDt}");

        _field.Step(stepDt);
        if (Time.frameCount % 30 == 0)
        {
            Debug.Log("pos=" + _ball.Position + " vel=" + _ball.Velocity);
        }

        ballVisual.position = _ball.Position.ToUnity();
        PanicLogger.Debug($"On End Ball position: {_ball.Position.X} {_ball.Position.Y}");
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;

        Vector3 a = new Vector3(min.x, min.y, 0);
        Vector3 b = new Vector3(max.x, min.y, 0);
        Vector3 c = new Vector3(max.x, max.y, 0);
        Vector3 d = new Vector3(min.x, max.y, 0);

        Gizmos.DrawLine(a, b);
        Gizmos.DrawLine(b, c);
        Gizmos.DrawLine(c, d);
        Gizmos.DrawLine(d, a);

        if (!Application.isPlaying || ballVisual == null) return;

        Gizmos.color = Color.cyan;
        Vector3 from = ballVisual.position;
        Vector2 v2 = _ball.Velocity.ToUnity();
        Vector3 to = from + new Vector3(v2.x, v2.y, 0) * 0.2f; // масштаб стрелки
        Gizmos.DrawLine(from, to);
        Gizmos.DrawSphere(to, 0.05f);
    }
}
