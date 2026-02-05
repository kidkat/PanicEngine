using System;   
using System.Collections.Generic;
using PanicEngine.Maths;
using PanicEngine.Physix;
using PanicEngine.Logger;

namespace PanicEngine.Core
{
    public sealed class FieldManager
    {
        public BodiesManager BodiesManager { get; }
        public FieldBounds FieldBounds { get; }

        public PhysixSettings Settings { get; set; } = new();

        // private readonly List<GoalTrigger> _goals = new(4);
        // private readonly List<PanicEvent> _events = new(8);
        // private readonly List<Trigger2D> _triggers = new(8);
        // private readonly List<TriggerEvent> _triggerEvents = new(8);
        // public IReadOnlyList<PanicEvent> Events => _events;
        // public IReadOnlyList<TriggerEvent> TriggerEvents => _triggerEvents;

        public FieldManager(FieldBounds fieldBounds)
        {
            FieldBounds = fieldBounds.Normalized();

            if(FieldBounds.Width <= 0f || FieldBounds.Height <= 0f)
            {
                PanicLogger.Error("Field bounds are invalid");
                throw new ArgumentException("Invalid FieldBounds: width/height must be > 0");
            }

            BodiesManager = new BodiesManager();
        }

        public FieldManager(FieldBounds fieldBounds, BodiesManager bodiesManager) : this(fieldBounds)
        {
            if(bodiesManager == null)
            {
                PanicLogger.Error("Bodies manager is null");
                throw new ArgumentNullException(nameof(bodiesManager));
            }
            BodiesManager = bodiesManager;
        }

        // public void AddGoalTrigger(GoalTrigger goal)
        // {
        //     _goals.Add(goal);
        // }

        // public void ClearGoals()
        // {
        //     _goals.Clear();
        // }

        // public void AddTrigger(Trigger2D trigger)
        // {
        //     if(trigger == null)
        //     {
        //         PanicLogger.Error("Trigger is null");
        //         throw new ArgumentNullException(nameof(trigger));
        //     }
        //     _triggers.Add(trigger);
        // }

        // public void ClearTriggers()
        // {
        //     _triggers.Clear();
        // }

        // ------------------------
        // Шаг симуляции
        // ------------------------
        public void Step(float deltaTime)
        {
            PanicLogger.Debug($"Stepping with deltaTime: {deltaTime}");
            if(deltaTime <= 0f) return;

            BodiesManager.LimitVelocity(Settings);
            BodiesManager.UpdateBodies(deltaTime);
            SolveCollisions();

            // _events.Clear();
            // CheckGoals();

            // _triggerEvents.Clear();
            // UpdateTriggers();
        }

        private void SolveCollisions()
        {
            int iterations = Settings.SolverIterations;
            PanicLogger.Debug($"Solving collisions with {iterations} iterations");
            for(int iter = 0; iter < iterations; iter++)
            {
                //wall collisions
                for(int i= 0; i < BodiesManager.Bodies.Count; i++)
                {
                    Collision2D.ResolveWallCollision(BodiesManager.Bodies[i], FieldBounds);
                }

                //body collisions
                for(int i= 0; i < BodiesManager.Bodies.Count; i++)
                {
                    for(int j= i+1; j < BodiesManager.Bodies.Count; j++)
                    {
                        Collision2D.ResolveBodyCollision(BodiesManager.Bodies[i], BodiesManager.Bodies[j], Settings);
                    }
                }
            }
        }

        // private void CheckGoals()
        // {
        //     Body2D? ball = null;
        //     for(int i = 0; i < _bodies.Count; i++)
        //     {
        //         if(_bodies[i].Id == 0)
        //         {
        //             ball = _bodies[i];
        //             break;
        //         }
        //     }

        //     if(ball == null) return;

        //     for(int i = 0; i < _goals.Count; i++)
        //     {
        //         if(_goals[i].IsInside(ball))
        //         {
        //             _events.Add(new PanicEvent(PanicEventType.GoalScored, _goals[i].TeamId));
        //             return;
        //         }
        //     }
        // }

        // private void UpdateTriggers()
        // {
        //     for(int i = 0; i < _triggers.Count; i++)
        //     {
        //         Trigger2D trigger = _triggers[i];
        //         for(int j = 0;j < _bodies.Count; j++)
        //         {
        //             trigger.Update(_bodies[j], _triggerEvents);
        //         }
        //     }
        // }
    }
}