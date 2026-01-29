using PanicEngine.Maths;
using PanicEngine.Logger;
using System;

namespace PanicEngine.Physix
{
    public sealed class Body2D
    {
        public int Id { get; }
        public Vector2D Position { get; set; }
        public Vector2D Velocity { get; set; }

        private float _radius;
        public float Radius
        {
            get => _radius;
            set
            {
                if(value <= 0f) throw new ArgumentException("Radius must be greater than 0");
                _radius = value;
            }
        }
        public float Mass { get; } 
        public float InverseMass { get; }
        private float _restitution = 0.6f;
        public float Restitution
        {
            get => _restitution;
            set => _restitution = Math.Clamp(value, 0f, 1f);
        }
        private float _linearDamping = 0.5f;
        public float LinearDamping
        {
            get => _linearDamping;
            set => _linearDamping = Math.Clamp(value, 0f, 1f);
        }
        public float SleepSpeed { get; set; } = 0.02f;
        public bool IsSleeping { get; set; } = false;
        public bool IsStatic { get; set; } = false;

        public Body2D(int id, Vector2D position, float mass, float radius)
        {
            if(id < 0) throw new ArgumentException("Id must be greater than 0");
            if(radius <= 0f) throw new ArgumentException("Radius must be greater than 0");

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
            if(deltaTime <= 0f) return;
            if(IsStatic) return;

            // Расчет коэффициента затухания (v_new = v_old * (1 - damping * Δt))
            float damping = 1f - LinearDamping * deltaTime;
            if(damping < 0f) damping = 0f;

            Velocity *= damping;

            if(Velocity.LengthSquared < SleepSpeed * SleepSpeed)
            {
                if(!IsSleeping)
                {
                    Velocity = Vector2D.Zero;
                    IsSleeping = true;
                    PanicLogger.Info($"Body {Id} is sleeping");
                }
                return;
            }

            IsSleeping = false;
            Position += Velocity * deltaTime;
        }

        /// <summary>
        /// Мгновенный импульс (удар, столкновение).
        /// v += impulse * invMass
        /// </summary>
        public void ApplyImpulse(Vector2D impulse)
        {
            if(IsStatic) return;
            if(IsSleeping) IsSleeping = false;
            Velocity += impulse * InverseMass;
        }
    }
}