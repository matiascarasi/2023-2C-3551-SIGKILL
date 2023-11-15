using Microsoft.Xna.Framework;
using TGC.MonoGame.TP.Actors;
using TGC.MonoGame.TP.Components.Collisions.Volumes;

namespace TGC.MonoGame.TP.Components.Collisions
{
    public class AxisAlignedBoundingBoxComponent : ICollisionComponent
    {
        public BoundingBox ModelBox;
        public BoundingBox BoundingBox;
        public void LoadContent(GameObject gameObject)
        {
            ModelBox = BoundingVolumesExtensions.CreateAABBFrom(gameObject.Model);
            BoundingBox = BoundingVolumesExtensions.Scale(ModelBox, gameObject.Scale);
            Update(gameObject);
        }

        public void Update(GameObject gameObject)
        {
            BoundingBox.Min = ModelBox.Min + gameObject.Position;
            BoundingBox.Max = ModelBox.Max + gameObject.Position;
        }

        public void Draw(Gizmos.Gizmos gizmos)
        {
            gizmos.DrawCube((BoundingBox.Max + BoundingBox.Min) / 2f, BoundingBox.Max - BoundingBox.Min);
        }

        public bool CollidesWith(ICollisionComponent other)
        {
            return other.CollidesWithAABB(this);
        }

        public bool CollidesWithOBB(OrientedBoundingBoxComponent obb)
        {
            return obb.OrientedBoundingBox.Intersects(BoundingBox);
        }

        public bool CollidesWithSphere(BoundingSphereComponent sphere)
        {
            return sphere.BoundingSphere.Intersects(BoundingBox);
        }

        public bool CollidesWithAABB(AxisAlignedBoundingBoxComponent aabb)
        {
            return aabb.BoundingBox.Intersects(BoundingBox);
        }
    }
}
