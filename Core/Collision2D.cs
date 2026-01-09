using System;
using PanicEngine.Maths;
using PanicEngine.Logger;

namespace PanicEngine.Core
{
    public static class Collision2D
    {
        public static void ResolveWallCollision(Body2D body, FieldBounds fieldBounds)
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

        public static void ResolveBodyCollision(Body2D body1, Body2D body2, PhysicsSettings settings)
        {
            if(body1.IsStatic && body2.IsStatic) return;

            Vector2D delta = body2.Position - body1.Position;
            float distanceSquared = delta.LengthSquared;
            float radiusSum = body1.Radius + body2.Radius;

            if(distanceSquared > radiusSum * radiusSum) return;

            float distance;
            Vector2D normal;

            if(distanceSquared > settings.Epsilon * settings.Epsilon)
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

            float invMassSum = body1.InverseMass + body2.InverseMass;
            if(invMassSum > 0f)
            {
                float correctionPen = penetration - settings.PositionCorrectionSlop;
                if(correctionPen < 0f) correctionPen = 0f;

                Vector2D correction = normal * (correctionPen / invMassSum) * settings.PositionCorrectionPercent;
                
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