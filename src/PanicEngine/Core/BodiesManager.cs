#nullable enable

using PanicEngine.Physix;
using PanicEngine.Logger;
using System.Collections.Generic;
using System;

namespace PanicEngine.Core
{
    public sealed class BodiesManager
    {
        private readonly List<Body2D> _bodies = new();
        public IReadOnlyList<Body2D> Bodies => _bodies;

        public void AddBody(Body2D body)
        {
            if(body == null)
            {
                PanicLogger.Error("Body is null");
                throw new ArgumentNullException(nameof(body));
            }
            _bodies.Add(body);
            PanicLogger.Info($"Body added: {body.Id}");
        }

        public void RemoveBody(Body2D body)
        {
            if(body == null)
            {
                PanicLogger.Error("Body is null");
                throw new ArgumentNullException(nameof(body));
            }
            _bodies.Remove(body);
            PanicLogger.Info($"Body removed: {body.Id}");
        }

        public void RemoveBodyById(int id)
        {

            foreach(var body in _bodies)
            {
                if(body.Id.Equals(id))
                {
                    _bodies.Remove(body);
                    PanicLogger.Info($"Body removed: {id}");
                    return;
                }
            }
        }

        public Body2D? GetBodyById(int id)
        {
            foreach(var body in _bodies)
            {
                if(body.Id.Equals(id))
                {
                    return body;
                }
            }

            return null;
        }

        public void ClearBodies()
        {
            _bodies.Clear();
            PanicLogger.Info("All bodies cleared");
        }

        public void LimitVelocity(PhysixSettings settings)
        {
            float maxSpeed = settings.MaxSpeed;
            if(maxSpeed <= 0f) return;

            float maxSpeedSquared = maxSpeed * maxSpeed;
            PanicLogger.Debug($"Max speed squared: {maxSpeedSquared}");

            foreach(var body in _bodies)
            {
                if(body.IsStatic) continue;

                if(body.Velocity.LengthSquared > maxSpeedSquared)
                {
                    body.Velocity = body.Velocity.Normalized * maxSpeed;
                    PanicLogger.Debug($"Clamped velocity for body {body.Id}: {body.Velocity}");
                }
            }
        }

        public void UpdateBodies(float deltaTime)
        {
            PanicLogger.Debug($"Integrating bodies with deltaTime: {deltaTime}");
            foreach(var body in _bodies)
            {
                body.Step(deltaTime);
            }
        }
    }
}