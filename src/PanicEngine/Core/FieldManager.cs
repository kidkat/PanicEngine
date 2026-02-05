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