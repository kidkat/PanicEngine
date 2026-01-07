using PanicEngine.Maths;

namespace PanicEngine.Core;

public class Body2D
{
    public Vector2D Position { get; set; }
    public Vector2D Velocity { get; set; }

    public Shape2D Shape { get; }
    public float Mass { get; }
    public float InverseMass { get; }
    // Упругость/отскок: 0 = совсем без отскока, 1 = идеально упруго
    public float Restitution { get; set; } = 0.6f;
    // Линейное затухание скорости (как "трение воздуха/поля").
    // 0 = нет затухания, больше = быстрее тормозит.
    public float LinearDamping { get; set; } = 0.5f;
    // Минимальная скорость, ниже которой считаем, что тело остановилось (для стабильности)
    public float SleepSpeed { get; set; } = 0.02f;
    public bool IsStatic { get; set; } = false;
    
    // Вращение (для бильярда)
    public float AngularVelocity { get; set; } = 0.0f;
    public float Angle { get; set; } = 0.0f; // Текущий угол поворота
    public float AngularFriction { get; set; } = 0.95f; // Трение вращения

    public Body2D(Vector2D position, Shape2D shape, float mass)
    {
        Position = position;
        Velocity = Vector2D.Zero;
        Shape = shape;
        Mass = mass;
        InverseMass = (mass <= 0f) ? 0f : 1f / mass;
    }


    /// <summary>
    /// Шаг симуляции: затухание + интеграция позиции.
    /// Коллизии считаются снаружи (World/PhysicsSolver).
    /// </summary>
    public void Step(float deltaTime)
    {
        if(deltaTime <= 0f) return;
        if(IsStatic) return;

        // Расчет коэффициента затухания
        float damping = 1f - LinearDamping * deltaTime;
        if(damping < 0f) damping = 0f;

        Velocity = Velocity * damping;

        if(Velocity.LengthSquared < SleepSpeed * SleepSpeed)
        {
            Velocity = Vector2D.Zero;
            return;
        }

        Position += Velocity * deltaTime;
    }

    /// <summary>
    /// Мгновенный импульс (удар, столкновение).
    /// v += impulse * invMass
    /// </summary>
    public void ApplyImpulse(Vector2D impulse)
    {
        if(IsStatic) return;
        Velocity += impulse * InverseMass;
    }

    /// <summary>
    /// AABB тела в мире (для Box) или "обёртка" для Circle.
    /// Удобно для broad-phase (быстрых проверок).
    /// </summary>
    public Aabb GetAabb()
    {
        return Shape.Type switch
        {
            Shape2DType.Circle => Aabb.FromCenterHalfSize(Position, new Vector2D(Shape.Radius, Shape.Radius)),
            Shape2DType.Box => Aabb.FromCenterHalfSize(Position, Shape.HalfSize),
            _ => throw new InvalidOperationException($"Unsupported shape type: {Shape.Type}")
        };
    }
}