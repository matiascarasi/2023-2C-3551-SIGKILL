using TGC.MonoGame.TP.Actors;

namespace TGC.MonoGame.TP.Components.Collisions
{
    public interface ICollisionComponent
    {
        public void LoadContent(GameObject gameObject);
        public void Update(GameObject gameObject);
        public void Draw(Gizmos.Gizmos gizmos);
        public bool CollidesWith(ICollisionComponent boundingVolume);
        public bool CollidesWithOBB(OrientedBoundingBoxComponent orientedBoundingBox);
        public bool CollidesWithSphere(BoundingSphereComponent boundingSphereComponent);
    }
}