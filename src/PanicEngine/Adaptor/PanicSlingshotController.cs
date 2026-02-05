using UnityEngine;
using UnityEngine.InputSystem;
using PanicEngine.Physix;
using PanicEngine.Maths;

[RequireComponent(typeof(Body2DView))]
public class PanicSlingshotController : MonoBehaviour
{
    [Header("Slingshot")]
    public float maxDragDistance = 1f;
    public float powerMultiplier = 8f;
    public float maxPower = 20f;

    [Header("Preview")]
    public LineRenderer trajectoryLine;
    public int trajectoryPoints = 30;
    public float trajectoryTimeStep = 0.05f;

    [Header("Turn")]
    public bool isActiveTurn = false;

    private Body2DView _view;
    private Camera _cam;
    private bool _isDragging;
    private Vector2 _startPos;

    private void Awake()
    {
        _view = GetComponent<Body2DView>();
        _cam = Camera.main;
        if (trajectoryLine != null)
        {
            trajectoryLine.positionCount = trajectoryPoints;
            trajectoryLine.useWorldSpace = true;
            trajectoryLine.transform.position = Vector3.zero;
            trajectoryLine.startWidth = 0.2f;   // обязательно > 0
            trajectoryLine.endWidth = 0.08f;
            Debug.Log("Trajectory line initialized");
        }
    }

    private void Update()
    {
        // if (!isActiveTurn) return;

        var pointer = Pointer.current;
        if (pointer == null) return;

        if (!_isDragging && pointer.press.wasPressedThisFrame)
        {
            Vector2 wp = _cam.ScreenToWorldPoint(pointer.position.ReadValue());
            if (IsTouchingThis(wp))
                StartDrag(wp);
        }
        else if (_isDragging && pointer.press.isPressed)
        {
            Vector2 wp = _cam.ScreenToWorldPoint(pointer.position.ReadValue());
            UpdateDrag(wp);
        }
        else if (_isDragging && pointer.press.wasReleasedThisFrame)
        {
            Vector2 wp = _cam.ScreenToWorldPoint(pointer.position.ReadValue());
            ReleaseShot(wp);
        }
    }
    private void LateUpdate()
    {
        if (_isDragging && _view.Body != null)
        {
            _view.Body.Position = VectorConverter.FromVector3ToVector2D(transform.position);
            _view.Body.Velocity = Vector2D.Zero;
        }
    }

    private bool IsTouchingThis(Vector2 worldPoint)
    {
        Vector2 center = transform.position;
        float r = _view != null ? _view.radius : 0.5f;
        return (worldPoint - center).sqrMagnitude <= r * r;
    }

    private void StartDrag(Vector2 worldPoint)
    {
        _isDragging = true;
        _startPos = transform.position;
        if (_view.Body != null)
        {
            _view.Body.Position = VectorConverter.FromVector3ToVector2D(transform.position);
            _view.Body.Velocity = Vector2D.Zero;
        }
    }

    private void UpdateDrag(Vector2 worldPoint)
    {
        Vector2 center = (Vector2)transform.position;
        Vector2 dir = center - worldPoint;
        float dist = Mathf.Min(dir.magnitude, maxDragDistance);
        Vector2 launch = dir.normalized * dist * powerMultiplier;

        if (trajectoryLine != null)
        {
            float z = 0f;
            trajectoryLine.enabled = true;
            trajectoryLine.positionCount = 2;
            trajectoryLine.SetPosition(0, new Vector3(center.x, center.y, z));
            trajectoryLine.SetPosition(1, new Vector3(center.x + launch.x * 0.15f, center.y + launch.y * 0.15f, z));
            // trajectoryLine.positionCount = trajectoryPoints;
            // trajectoryLine.enabled = true;
            // DrawTrajectory(center, launch);
        }
    }

    private void ReleaseShot(Vector2 worldPoint)
    {
        _isDragging = false;

        if (_view?.Body == null) return;

        Vector2 center = (Vector2)transform.position;
        Vector2 dir = center - worldPoint;
        float dist = Mathf.Min(dir.magnitude, maxDragDistance);
        float power = Mathf.Clamp(dist * powerMultiplier, 0f, maxPower);
        Vector2 impulse = dir.normalized * power;

        _view.Body.ApplyImpulse(VectorConverter.ToPanicVector2D(impulse));

        if (trajectoryLine != null)
        {
            trajectoryLine.positionCount = 0;
            trajectoryLine.enabled = false;
        }
        isActiveTurn = false;
    }

    private void DrawTrajectory(Vector2 start, Vector2 initialVelocity)
    {
        if (trajectoryLine == null || _view?.Body == null) return;
        trajectoryLine.positionCount = trajectoryPoints;
        float invMass = _view.Body.InverseMass;
        Vector2 vel = initialVelocity * invMass;
        Vector2 pos = start;
        float dt = trajectoryTimeStep;
        for (int i = 0; i < trajectoryPoints; i++)
        {
            trajectoryLine.SetPosition(i, pos);
            pos += vel * dt;
            vel += Physics2D.gravity * dt;
        }
    }

    public void BeginTurn()
    {
        isActiveTurn = true;
    }

    public void EndTurn()
    {
        isActiveTurn = false;
    }
}