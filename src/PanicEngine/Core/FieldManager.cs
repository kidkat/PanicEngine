using System;   
using System.Collections.Generic;
using PanicEngine.Maths;
using PanicEngine.Logger;
using PanicEngine.Events;

namespace PanicEngine.Core
{
    public sealed class FieldManager
    {
        private readonly List<Body2D> _bodies = [];
        public FieldBounds FieldBounds { get; }
        public IReadOnlyList<Body2D> Bodies => _bodies;

        public PhysicsSettings Settings { get; set; } = new();

        private readonly List<GoalTrigger> _goals = new(4);
        private readonly List<PanicEvent> _events = new(8);
        private readonly List<Trigger2D> _triggers = new(8);
        private readonly List<TriggerEvent> _triggerEvents = new(8);
        public IReadOnlyList<PanicEvent> Events => _events;
        public IReadOnlyList<TriggerEvent> TriggerEvents => _triggerEvents;

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
            // if(body == null) throw new ArgumentNullException(nameof(body));
            ArgumentNullException.ThrowIfNull(body);

            _bodies.Add(body);
            PanicLogger.Info($"Body added: {body.Id}");
        }

        public void RemoveBody(Body2D body)
        {
            // if(body == null) throw new ArgumentNullException(nameof(body));
            ArgumentNullException.ThrowIfNull(body);
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

        public void AddGoalTrigger(GoalTrigger goal)
        {
            _goals.Add(goal);
        }

        public void ClearGoals()
        {
            _goals.Clear();
        }

        public void AddTrigger(Trigger2D trigger)
        {
            if(trigger == null)
            {
                PanicLogger.Error("Trigger is null");
                throw new ArgumentNullException(nameof(trigger));
            }
            _triggers.Add(trigger);
        }

        public void ClearTriggers()
        {
            _triggers.Clear();
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

            _events.Clear();
            CheckGoals();

            _triggerEvents.Clear();
            UpdateTriggers();
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
                    body.Velocity = body.Velocity.Normalized * maxSpeed;
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

        private void CheckGoals()
        {
            Body2D? ball = null;
            for(int i = 0; i < _bodies.Count; i++)
            {
                if(_bodies[i].Id == 0)
                {
                    ball = _bodies[i];
                    break;
                }
            }

            if(ball == null) return;

            for(int i = 0; i < _goals.Count; i++)
            {
                if(_goals[i].IsInside(ball))
                {
                    _events.Add(new PanicEvent(PanicEventType.GoalScored, _goals[i].TeamId));
                    return;
                }
            }
        }

        private void UpdateTriggers()
        {
            for(int i = 0; i < _bodies.Count; i++)
            {
                Trigger2D trigger = _triggers[i];
                for(int j = 0;j < _bodies.Count; j++)
                {
                    trigger.Update(_bodies[j], _triggerEvents);
                }
            }
        }
    }
}