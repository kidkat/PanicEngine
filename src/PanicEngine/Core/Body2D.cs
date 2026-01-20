using PanicEngine.Maths;
using PanicEngine.Logger;
using System;

namespace PanicEngine.Core
{
    public sealed class Body2D
    {
        public int Id { get; }
        public Vector2D Position { get; set; }
        public Vector2D Velocity { get; set; }

        public float Radius { get; set; }
        public float Mass { get; }
        public float InverseMass { get; }
        public float Restitution { get; set; } = 0.6f;
        public float LinearDamping { get; set; } = 0.5f;
        public float SleepSpeed { get; set; } = 0.02f;
        public bool IsStatic { get; set; } = false;
    
        // Вращение (для бильярда)
        public float AngularVelocity { get; set; } = 0.0f;
        public float Angle { get; set; } = 0.0f; // Текущий угол поворота
        public float AngularFriction { get; set; } = 0.95f; // Трение вращения

        public Body2D(int id, Vector2D position, float mass, float radius)
        {
            Id = id;
            Position = position;
            Velocity = Vector2D.Zero;
            Mass = mass;
            InverseMass = (mass <= 0f) ? 0f : 1f / mass;
            Radius = radius;
        }


        /// <summary>
        /// Шаг симуляции: затухание + интеграция позиции.
        /// Коллизии считаются снаружи (World/PhysicsSolver).
        /// </summary>
        public void Step(float deltaTime)
        {
            PanicLogger.Debug($"Stepping body {Id} with deltaTime: {deltaTime}");
            if(deltaTime <= 0f) return;
            if(IsStatic) return;

            // Расчет коэффициента затухания
            float damping = 1f - LinearDamping * deltaTime;
            if(damping < 0f) damping = 0f;

            Velocity *= damping;
            PanicLogger.Debug($"Velocity after damping: {Velocity}");

            if(Velocity.LengthSquared < SleepSpeed * SleepSpeed)
            {
                Velocity = Vector2D.Zero;
                PanicLogger.Debug($"Velocity set to zero for body {Id}");
                return;
            }

            Position += Velocity * deltaTime;
            PanicLogger.Debug($"Position after integration: {Position}");
        }

        /// <summary>
        /// Мгновенный импульс (удар, столкновение).
        /// v += impulse * invMass
        /// </summary>
        public void ApplyImpulse(Vector2D impulse)
        {
            if(IsStatic) return;
            Velocity += impulse * InverseMass;
            PanicLogger.Debug($"Velocity after impulse: {Velocity}");
        }
    }
}