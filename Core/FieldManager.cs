using System;   
using System.Collections.Generic;
using PanicEngine.Maths;
using PanicEngine.Logger;

namespace PanicEngine.Core
{

    public sealed class FieldManager
    {
        private readonly List<Body2D> _bodies = new();
        public FieldBounds FieldBounds { get; }
        public IReadOnlyList<Body2D> Bodies => _bodies;

        public float MaxSpeed { get; } = 50f;
        // Сколько раз прогонять решатель за тик (1–4 обычно)
        public int SolverIterations { get; set; } = 1;
        // Насколько сильно раздвигать пересечения (0.8 обычно хорошо)
        public float PositionCorrectionPercent { get; set; } = 0.8f;
        // “допуск” чтобы не дрожало на микропересечениях
        public float PositionCorrectionSlop { get; set; } = 0.01f;

        public FieldManager(FieldBounds fieldBounds)
        {
            FieldBounds = fieldBounds.Normalized();

            if(FieldBounds.Width <= 0f || FieldBounds.Height <= 0f)
            {
                PanicLogger.Error("Field bounds are invalid");
                throw new ArgumentException("Invalid FieldBounds: width/height must be > 0");
            }
        }

        public void AddBody(Body2D body)
        {
            if(body == null) throw new ArgumentNullException(nameof(body));
            _bodies.Add(body);
            PanicLogger.Info($"Body added: {body.Id}");
        }

        public void RemoveBody(Body2D body)
        {
            if(body == null) throw new ArgumentNullException(nameof(body));
            _bodies.Remove(body);
            PanicLogger.Info($"Body removed: {body.Id}");
        }

        public Body2D? GetBodyById(int id)
        {
            for(int i =0; i < _bodies.Count; i++)
            {
                if(_bodies[i].Id == id)
                {
                    PanicLogger.Warning($"Body found: {id}");
                    return _bodies[i];
                }
                
            }

            PanicLogger.Error($"Body not found: {id}");
            return null;
        }

        public void ClearBodies()
        {
            _bodies.Clear();
            PanicLogger.Info("Bodies cleared");
        }

        // ------------------------
        // Шаг симуляции
        // ------------------------

        public void Step(float deltaTime)
        {
            PanicLogger.Debug($"Stepping with deltaTime: {deltaTime}");
            if(deltaTime <= 0f) return;

            ClampVelocity();
            IntegrateBodies(deltaTime);
            SolveCollisions();
        }

        private void ClampVelocity()
        {
            if(MaxSpeed <= 0f) return;

            float maxSpeedSquared = MaxSpeed * MaxSpeed;
            PanicLogger.Debug($"Max speed squared: {maxSpeedSquared}");

            for(int i= 0; i < _bodies.Count; i++)
            {
                var body = _bodies[i];
                if(body.IsStatic) continue;

                if(body.Velocity.LengthSquared > maxSpeedSquared)
                {
                    body.Velocity = body.Velocity.Normalized() * MaxSpeed;
                    PanicLogger.Debug($"Clamped velocity for body {body.Id}: {body.Velocity}");
                }
            }
        }

        private void IntegrateBodies(float deltaTime)
        {
            PanicLogger.Debug($"Integrating bodies with deltaTime: {deltaTime}");
            for(int i= 0; i < _bodies.Count; i++)
            {
                _bodies[i].Step(deltaTime);
            }
        }

        private void SolveCollisions()
        {
            PanicLogger.Debug($"Solving collisions with {SolverIterations} iterations");
            for(int iter = 0; iter < SolverIterations; iter++)
            {
                //wall collisions
                for(int i= 0; i < _bodies.Count; i++)
                {
                    ResolveWallCollision(_bodies[i], FieldBounds);
                }

                //body collisions
                for(int i= 0; i < _bodies.Count; i++)
                {
                    for(int j= i+1; j < _bodies.Count; j++)
                    {
                        ResolveBodyCollision(_bodies[i], _bodies[j]);
                    }
                }
            }
        }

        // ------------------------
        // Столкновение со стенами
        // ------------------------
        private static void ResolveWallCollision(Body2D body, FieldBounds fieldBounds)
        {
            if(body.IsStatic) return;

            var position = body.Position;
            var velocity = body.Velocity;
            float restitution = body.Restitution;
            float radius = body.Radius;
            PanicLogger.Debug($"Resolving wall collision for body {body.Id} with position: {position} and velocity: {velocity} and restitution: {restitution}");

            float left = fieldBounds.Left + radius;
            float right = fieldBounds.Right - radius;
            float top = fieldBounds.Top - radius;
            float bottom = fieldBounds.Bottom + radius;
            
            if(position.X < left)
            {
                position = new Vector2D(left, position.Y);
                var wallNormal = new Vector2D(1f, 0f);
                velocity = velocity.Reflect(wallNormal) * restitution;
                PanicLogger.Debug($"Wall collision detected on left side for body {body.Id} with new position: {position} and new velocity: {velocity}");
            }
            
            if(position.X > right)
            {
                position = new Vector2D(right, position.Y);
                var wallNormal = new Vector2D(-1f, 0f);
                velocity = velocity.Reflect(wallNormal) * restitution;
                PanicLogger.Debug($"Wall collision detected on right side for body {body.Id} with new position: {position} and new velocity: {velocity}");
            }

            if(position.Y > top)
            {
                position = new Vector2D(position.X, top);
                var wallNormal = new Vector2D(0f, -1f);
                velocity = velocity.Reflect(wallNormal) * restitution;
                PanicLogger.Debug($"Wall collision detected on top side for body {body.Id} with new position: {position} and new velocity: {velocity}");
            }
            
            if(position.Y < bottom)
            {
                position = new Vector2D(position.X, bottom);
                var wallNormal = new Vector2D(0f, 1f);
                velocity = velocity.Reflect(wallNormal) * restitution;
                PanicLogger.Debug($"Wall collision detected on bottom side for body {body.Id} with new position: {position} and new velocity: {velocity}");
            }


            body.Position = position;
            body.Velocity = velocity;
        }

        private void ResolveBodyCollision(Body2D body1, Body2D body2)
        {
            if(body1.IsStatic && body2.IsStatic) return;

            Vector2D delta = body2.Position - body1.Position;
            float distanceSquared = delta.LengthSquared;
            float radiusSum = body1.Radius + body2.Radius;

            if(distanceSquared > radiusSum * radiusSum) return;

            float distance;
            Vector2D normal;

            if(distanceSquared > 0f)
            {
                distance = (float)Math.Sqrt(distanceSquared);
                normal = delta / distance; //unit
            } 
            else
            {
                distance = 0f;
                normal = new Vector2D(1f, 0f);
            }

            float penetration = radiusSum - distance;
            // 1) Position correction (раздвижка, чтобы не слипались)
            // correction = max(penetration - slop, 0) / (invMassA + invMassB) * percent * n
            float slop = PositionCorrectionSlop;
            float percent = PositionCorrectionPercent;

            float invMassSum = body1.InverseMass + body2.InverseMass;
            if(invMassSum > 0f)
            {
                float correctionPen = penetration - slop;
                if(correctionPen < 0f) correctionPen = 0f;

                Vector2D correction = normal * (correctionPen / invMassSum) * percent;
                
                if(!body1.IsStatic)
                    body1.Position = body1.Position - correction * body1.InverseMass;

                if(!body2.IsStatic)
                    body2.Position = body2.Position + correction * body2.InverseMass;
            }

            // 2) Impulse (отскок)
            // Считаем относительную скорость вдоль нормали
            Vector2D relativeVelocity = body2.Velocity - body1.Velocity;
            float velocityAlongNormal = relativeVelocity.Dot(normal);

            // Если тела расходятся, импульс не нужен
            if(velocityAlongNormal > 0f) return;

            // Эффективная упругость пары (часто берут min)
            float restitution = body1.Restitution < body2.Restitution ? body1.Restitution : body2.Restitution;

            // j = -(1+e) * (rv·n) / (invMassA + invMassB)
            float j = -(1f + restitution) * velocityAlongNormal;
            if(invMassSum > 0f) j /= invMassSum;
            else return;

            Vector2D impulse = normal * j;

            if(!body1.IsStatic)
                body1.Velocity = body1.Velocity - impulse * body1.InverseMass;

            if(!body2.IsStatic)
                body2.Velocity = body2.Velocity + impulse * body2.InverseMass;
        }
    }
}