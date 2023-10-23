using System;
using System.Linq;
using Microsoft.Xna.Framework;
using TGC.MonoGame.TP.Collisions;

namespace TGC.MonoGame.TP.Components.Collisions
{
    public class CollisionComponent : ICollisionComponent
    {
        private OrientedBoundingBox OrientedBoundingBox;
        private Matrix OrientedBoundingBoxWorld;

        virtual public void LoadContent(GameObject gameObject)
        {
            var objectBox = BoundingVolumesExtensions.CreateAABBFrom(gameObject.Model);
            var boundingBox = BoundingVolumesExtensions.Scale(objectBox, gameObject.Scale);
            SetBoundingBox(gameObject, boundingBox);
        }

        public void Update(GameObject gameObject)
        {
            OrientedBoundingBox.Center = gameObject.Position;
            OrientedBoundingBox.Orientation = Matrix.CreateRotationY(MathHelper.ToRadians(gameObject.YAxisRotation));
            OrientedBoundingBoxWorld = CreateObjectWorld(gameObject.Position, gameObject.YAxisRotation);
        }
        public void Draw(Gizmos.Gizmos gizmos)
        {
            gizmos.DrawCube(OrientedBoundingBoxWorld);
        }

        public bool CollidesWith(CollisionComponent other)
        {
            return OrientedBoundingBox.Intersects(other.OrientedBoundingBox);
        }

        protected void SetBoundingBox(GameObject gameObject, BoundingBox boundingBox)
        {
            OrientedBoundingBox = OrientedBoundingBox.FromAABB(boundingBox);
            OrientedBoundingBox.Center = BoundingVolumesExtensions.GetCenter(boundingBox) + gameObject.Position;
            OrientedBoundingBox.Orientation = Matrix.CreateRotationY(gameObject.YAxisRotation);
            OrientedBoundingBoxWorld = CreateObjectWorld(gameObject.Position, gameObject.YAxisRotation);
        }

        private Matrix CreateObjectWorld(Vector3 position, float rotation)
        {
            return Matrix.CreateScale(OrientedBoundingBox.Extents * 2f) 
                    * Matrix.CreateRotationY(MathHelper.ToRadians(rotation)) 
                        * Matrix.CreateTranslation(position + new Vector3(0f, OrientedBoundingBox.Extents.Y, 0f));
        }

        
    }
}
