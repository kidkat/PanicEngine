using System;
using PanicEngine.Maths;
using PanicEngine.Physix;
using PanicEngine.Core;
using PanicEngine.Logger;

namespace PanicEngine.Physix
{
    public static class Collision2D
    {
        public static void ResolveWallCollision(Body2D body, FieldBounds fieldBounds)
        {
            if(body.IsStatic) return;

            var position = body.Position;
            var velocity = body.Velocity;
            float restitution = body.Restitution;
            PanicLogger.Debug($"Resolving wall collision for body {body.Id} with position: {position} and velocity: {velocity} and restitution: {restitution}");

            float leftBorder = fieldBounds.MinX + body.Radius; 
            float rightBorder = fieldBounds.MaxX - body.Radius;
            float topBorder = fieldBounds.MinY + body.Radius;
            float bottomBorder = fieldBounds.MaxY + body.Radius;
            
            if(position.X < leftBorder)
            {
                ResolveWallSide(body.Id, ref position, ref velocity, leftBorder, 
                    position.Y, new Vector2D(1f, 0f), restitution, "left");
            }
            
            if(position.X > rightBorder)
            {
                ResolveWallSide(body.Id, ref position, ref velocity, rightBorder, 
                    position.Y, new Vector2D(-1f, 0f), restitution, "right");
            }

            if(position.Y > bottomBorder)
            {
                ResolveWallSide(body.Id, ref position, ref velocity, position.X, 
                    bottomBorder, new Vector2D(0f, -1f), restitution, "bottom");
            }
            
            if(position.Y < topBorder)
            {
                ResolveWallSide(body.Id, ref position, ref velocity, position.X, 
                    topBorder, new Vector2D(0f, 1f), restitution, "top");
            }

            body.Position = position;
            body.Velocity = velocity;
        }

        private static void ResolveWallSide(int bodyId, ref Vector2D position, ref Vector2D velocity,
            float newX, float newY, Vector2D wallNormal, float restitution, string sideName)
        {
            position = new Vector2D(newX, newY);
            velocity = velocity.Reflect(wallNormal) * restitution;
            PanicLogger.Debug($"Wall collision detected on {sideName} side for body {bodyId} with new position: {position} and new velocity: {velocity}");
        }

        public static void ResolveBodyCollision(Body2D body1, Body2D body2, PhysixSettings settings)
        {
            if(body1.IsStatic && body2.IsStatic) return;

            Vector2D delta = body2.Position - body1.Position;
            float distanceSquared = delta.LengthSquared;
            float radiusSum = body1.Radius + body2.Radius;
            float radiusSumSquared = radiusSum * radiusSum;

            if(distanceSquared > radiusSumSquared) return;

            float distance;
            Vector2D normal;

            if(distanceSquared > float.Epsilon * float.Epsilon)
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
                float correctionPen = Math.Max(penetration - settings.PositionCorrectionSlop, 0f);
                Vector2D correction = normal * (correctionPen / invMassSum) * settings.PositionCorrectionPercent;
                
                if(!body1.IsStatic)
                    body1.Position -= correction * body1.InverseMass;

                if(!body2.IsStatic)
                    body2.Position += correction * body2.InverseMass;
            }

            // 2) Impulse (отскок)
            // Считаем относительную скорость вдоль нормали
            Vector2D relativeVelocity = body2.Velocity - body1.Velocity;
            float velocityAlongNormal = relativeVelocity.Dot(normal);

            // Если тела расходятся, импульс не нужен
            if(velocityAlongNormal > 0f) return;

            // Эффективная упругость пары (часто берут min)
            float restitution = Math.Max(body1.Restitution, body2.Restitution);

            // j = -(1+e) * (rv·n) / (invMassA + invMassB)
            float j = -(1f + restitution) * velocityAlongNormal;
            if(invMassSum > 0f) j /= invMassSum;
            else return;

            Vector2D impulse = normal * j;

            if(!body1.IsStatic)
                body1.Velocity -= impulse * body1.InverseMass;

            if(!body2.IsStatic)
                body2.Velocity += impulse * body2.InverseMass;
        }
    }
}