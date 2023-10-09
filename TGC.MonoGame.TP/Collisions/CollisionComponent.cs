using System;
using Microsoft.Xna.Framework;
using TGC.MonoGame.TP.Content.Actors;
using TGC.MonoGame.TP.Gizmos;

namespace TGC.MonoGame.TP.Collisions
{
    public class CollisionComponent
    {
        private BoundingBox BoundingBox;
        private BoundingBox ObjectBox;
        public CollisionComponent(GameObject gameObject)
        {
            ObjectBox = BoundingVolumesExtensions.CreateAABBFrom(gameObject.Model);
            BoundingBox = new BoundingBox(ObjectBox.Min + gameObject.Position, ObjectBox.Max + gameObject.Position);
        }

        public void Update(Vector3 position)
        {
            BoundingBox = new BoundingBox(ObjectBox.Min + position, ObjectBox.Max + position);
        }

        public bool CollidesWith(CollisionComponent other)
        {
            return BoundingBox.Intersects(other.BoundingBox);
        }

        public void DrawBoundingVolume(Gizmos.Gizmos gizmos)
        {
            Vector3[] points = {BoundingBox.Min, BoundingBox.Max};
            // Console.Write(points[0]);
            // Console.Write(points[1]);
            // Console.WriteLine();
            // gizmos.DrawPolyLine(ObjectBox.GetCorners());
            gizmos.DrawCube((BoundingBox.Max + BoundingBox.Min) / 2f, BoundingBox.Max - BoundingBox.Min);
        }
    }
}
