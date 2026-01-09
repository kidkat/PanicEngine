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

        public PhysicsSettings Settings { get; set; } = new();

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

            float maxSpeed = Settings.MaxSpeed;
            if(maxSpeed <= 0f) return;

            float maxSpeedSquared = maxSpeed * maxSpeed;
            PanicLogger.Debug($"Max speed squared: {maxSpeedSquared}");

            for(int i= 0; i < _bodies.Count; i++)
            {
                var body = _bodies[i];
                if(body.IsStatic) continue;

                if(body.Velocity.LengthSquared > maxSpeedSquared)
                {
                    body.Velocity = body.Velocity.Normalized() * maxSpeed;
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
            int iterations = Settings.SolverIterations;
            PanicLogger.Debug($"Solving collisions with {iterations} iterations");
            for(int iter = 0; iter < iterations; iter++)
            {
                //wall collisions
                for(int i= 0; i < _bodies.Count; i++)
                {
                    Collision2D.ResolveWallCollision(_bodies[i], FieldBounds);
                }

                //body collisions
                for(int i= 0; i < _bodies.Count; i++)
                {
                    for(int j= i+1; j < _bodies.Count; j++)
                    {
                        Collision2D.ResolveBodyCollision(_bodies[i], _bodies[j], Settings);
                    }
                }
            }
        }
    }
}