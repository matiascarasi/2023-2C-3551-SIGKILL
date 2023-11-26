using BepuPhysics;
using BepuPhysics.Collidables;
using Microsoft.Xna.Framework;

namespace TGC.MonoGame.TP.Gizmos
{
    public class ShapeDrawer
    {
        private readonly Gizmos Gizmos = Gizmos.GetInstance();

        public void Draw(IShape shape, RigidPose pose)
        {
            if (shape.GetType() == typeof(Box))
            {
                var box = (Box)shape;
                Vector3[] points = new Vector3[8];
                points[0] = new Vector3(box.HalfWidth, box.HalfHeight, box.HalfLength);
                points[1] = new Vector3(box.HalfWidth, box.HalfHeight, -box.HalfLength);
                points[2] = new Vector3(box.HalfWidth, -box.HalfHeight, box.HalfLength);
                points[3] = new Vector3(box.HalfWidth, -box.HalfHeight, -box.HalfLength);
                points[4] = new Vector3(-box.HalfWidth, box.HalfHeight, box.HalfLength);
                points[5] = new Vector3(-box.HalfWidth, box.HalfHeight, -box.HalfLength);
                points[6] = new Vector3(-box.HalfWidth, -box.HalfHeight, box.HalfLength);
                points[7] = new Vector3(-box.HalfWidth, -box.HalfHeight, -box.HalfLength);
                var matrix = Matrix.CreateFromQuaternion(pose.Orientation) * Matrix.CreateTranslation(pose.Position);
                for (var i = 0; i < points.Length; i++)
                {
                    points[i] = Vector3.Transform(points[i], matrix);
                }
                Gizmos.DrawLine(points[0], points[4]);
                Gizmos.DrawLine(points[4], points[5]);
                Gizmos.DrawLine(points[5], points[1]);
                Gizmos.DrawLine(points[1], points[0]);
                Gizmos.DrawLine(points[0], points[2]);
                Gizmos.DrawLine(points[1], points[3]);
                Gizmos.DrawLine(points[4], points[6]);
                Gizmos.DrawLine(points[5], points[7]);
                Gizmos.DrawLine(points[2], points[3]);
                Gizmos.DrawLine(points[3], points[7]);
                Gizmos.DrawLine(points[7], points[6]);
                Gizmos.DrawLine(points[6], points[2]);
            }
        }
    }
}