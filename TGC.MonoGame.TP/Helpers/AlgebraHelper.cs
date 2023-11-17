using Microsoft.Xna.Framework;
using System;

namespace TGC.MonoGame.TP.Helpers
{
    abstract class AlgebraHelper
    {

        public const double FULL_ROTATION = 360;
        static public Vector2 GetRandomPointInCircle(Vector2 center, double radius, Random Random)
        {

            var rng = Random.NextDouble();
            var randomRadius = radius * rng;
            var randomAngle = FULL_ROTATION * rng;

            var xPosition = center.X + Convert.ToSingle(randomRadius * Math.Cos(randomAngle));
            var yPosition = center.Y + Convert.ToSingle(randomRadius * Math.Sin(randomAngle));

            return new Vector2(xPosition, yPosition);
        } 

        static public float GetAngleBetweenTwoVectors(Vector3 v1, Vector3 v2)
        {
            var crossDirection = Vector3.Cross(v1, v2);
            var dotDirection = Vector3.Dot(v1, v2);

            var angleSideFactor = Vector3.Dot(Vector3.Up, crossDirection) > 0f ? 1 : -1;
            return MathHelper.ToDegrees(MathF.Atan2(crossDirection.Length(), dotDirection)) * angleSideFactor;
        }
    }
}
