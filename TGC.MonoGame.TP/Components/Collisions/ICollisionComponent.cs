using TGC.MonoGame.TP.Actors;
using TGC.MonoGame.TP.Components.Collisions;

namespace TGC.MonoGame.TP.Collisions
{
    public interface ICollisionComponent
    {
        public void LoadContent(GameObject gameObject);
        public void Update(GameObject gameObject);
        public void Draw(Gizmos.Gizmos gizmos);
        public bool CollidesWith(ICollisionComponent boundingVolume);
        public bool CollidesWithOBB(OrientedBoundingBoxComponent orientedBoundingBox);
    }
}