using Microsoft.Xna.Framework;
using TGC.MonoGame.TP.Actors;
using TGC.MonoGame.TP.Components.Collisions.Volumes;

namespace TGC.MonoGame.TP.Components.Collisions
{
    public class OrientedBoundingBoxComponent: ICollisionComponent
    {
        public OrientedBoundingBox OrientedBoundingBox;
        private Matrix OrientedBoundingBoxWorld;
        public virtual void LoadContent(GameObject gameObject)
        {
            var objectBox = BoundingVolumesExtensions.CreateAABBFrom(gameObject.Model);
            var boundingBox = BoundingVolumesExtensions.Scale(objectBox, gameObject.Scale);
            SetBoundingBox(gameObject, boundingBox);
        }

        public void Update(GameObject gameObject)
        {
            OrientedBoundingBox.Center = gameObject.Position;
            OrientedBoundingBox.Orientation = Matrix.CreateRotationY(MathHelper.ToRadians(gameObject.RotationAngle));
            OrientedBoundingBoxWorld = CreateObjectWorld(gameObject.Position, gameObject.RotationAngle);
        }

        public void Draw(Gizmos.Gizmos gizmos)
        {
            gizmos.DrawCube(OrientedBoundingBoxWorld);
        }

        public bool CollidesWith(ICollisionComponent other)
        {
            return other.CollidesWithOBB(this);
        }

        protected void SetBoundingBox(GameObject gameObject, BoundingBox boundingBox)
        {
            OrientedBoundingBox = OrientedBoundingBox.FromAABB(boundingBox);
            OrientedBoundingBox.Center = BoundingVolumesExtensions.GetCenter(boundingBox) + gameObject.Position;
            OrientedBoundingBox.Orientation = Matrix.CreateRotationY(gameObject.RotationAngle);
            OrientedBoundingBoxWorld = CreateObjectWorld(gameObject.Position, gameObject.RotationAngle);
        }

        public bool CollidesWithOBB(OrientedBoundingBoxComponent other)
        {
            return other.OrientedBoundingBox.Intersects(OrientedBoundingBox);
        }

        public bool CollidesWithSphere(BoundingSphereComponent sphereComponent)
        {
            return OrientedBoundingBox.Intersects(sphereComponent.BoundingSphere);
        }

        private Matrix CreateObjectWorld(Vector3 position, float rotation)
        {
            return Matrix.CreateScale(OrientedBoundingBox.Extents * 2f) 
                    * Matrix.CreateRotationY(MathHelper.ToRadians(rotation)) 
                        * Matrix.CreateTranslation(position + new Vector3(0f, OrientedBoundingBox.Extents.Y, 0f));
        }
    }
}
