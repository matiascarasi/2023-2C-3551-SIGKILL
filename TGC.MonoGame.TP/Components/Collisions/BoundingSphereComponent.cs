using Microsoft.Xna.Framework;
using TGC.MonoGame.TP.Actors;
using TGC.MonoGame.TP.Components.Collisions.Volumes;

namespace TGC.MonoGame.TP.Components.Collisions
{
    public class BoundingSphereComponent : ICollisionComponent
    {
        public BoundingSphere BoundingSphere;
        public virtual void LoadContent(GameObject gameObject)
        {
            BoundingSphere = BoundingVolumesExtensions.CreateSphereFrom(gameObject.Model);
            BoundingSphere.Radius *= gameObject.Scale;
            BoundingSphere.Center = gameObject.Position;
        }

        public void Update(GameObject gameObject)
        {
            BoundingSphere.Center = gameObject.Position;
        }

        public void Draw(Gizmos.Gizmos gizmos)
        {
            gizmos.DrawSphere(BoundingSphere.Center, Vector3.One * BoundingSphere.Radius);
        }

        public bool CollidesWith(ICollisionComponent other)
        {
            return other.CollidesWithSphere(this);
        }

        public bool CollidesWithOBB(OrientedBoundingBoxComponent obbComponent)
        {
            return obbComponent.OrientedBoundingBox.Intersects(BoundingSphere);
        }

        public bool CollidesWithSphere(BoundingSphereComponent other)
        {
            return other.BoundingSphere.Intersects(BoundingSphere);
        }
    }
}
