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
        public TriggerManager TriggerManager { get; }
        public FieldBounds FieldBounds { get; }

        public PhysixSettings Settings { get; set; } = new();

        public FieldManager(FieldBounds fieldBounds)
        {
            FieldBounds = fieldBounds.Normalized();

            if(FieldBounds.Width <= 0f || FieldBounds.Height <= 0f)
            {
                PanicLogger.Error("Field bounds are invalid");
                throw new ArgumentException("Invalid FieldBounds: width/height must be > 0");
            }

            BodiesManager = new BodiesManager();
            TriggerManager = new TriggerManager();
        }

        public FieldManager(FieldBounds fieldBounds, BodiesManager bodiesManager) : this(fieldBounds)
        {
            if(bodiesManager == null)
            {
                PanicLogger.Error("Bodies manager is null");
                throw new ArgumentNullException(nameof(bodiesManager));
            }
            BodiesManager = bodiesManager;
            TriggerManager = new TriggerManager();
        }

        public FieldManager(FieldBounds fieldBounds, BodiesManager bodiesManager, TriggerManager triggerManager) : this(fieldBounds, bodiesManager)
        {
            if(triggerManager == null)
            {
                PanicLogger.Error("Trigger manager is null");
                throw new ArgumentNullException(nameof(triggerManager));
            }
            TriggerManager = triggerManager;
        }

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

            TriggerManager.Update(BodiesManager.Bodies);
        }

        /// <summary>
        /// Запускает симуляцию до момента, пока все тела не остановятся (IsSleeping)
        /// или не будет достигнут лимит шагов (защита от бесконечного цикла).
        /// </summary>
        public int SimulateUntilRest(float fixedDeltaTime, int maxSteps = 2000)
        {
            if (fixedDeltaTime <= 0f) return 0;
            
            int steps = 0;
            while (steps < maxSteps)
            {
                if (BodiesManager.AllBodiesSleeping())
                    break;
                    
                Step(fixedDeltaTime);
                steps++;
            }
            
            return steps;
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
    }
}