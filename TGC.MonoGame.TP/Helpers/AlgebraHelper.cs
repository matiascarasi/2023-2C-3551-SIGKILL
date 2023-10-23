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
    }
}
