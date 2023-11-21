using System;
using System.Linq;
using BepuPhysics.Collidables;
using System.Transactions;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using TGC.MonoGame.TP.Actors;
using TGC.MonoGame.TP.Collisions;

namespace TGC.MonoGame.TP.Components.Collisions
{
    public class CollisionComponent
    {
        protected OrientedBoundingBox OrientedBoundingBox;
        private Matrix OrientedBoundingBoxWorld;
        private BoundingBox objectBox;
        public BoundingBox BoxWorldSpace;
        public virtual void LoadContent(GameObject gameObject)
        {
            objectBox = BoundingVolumesExtensions.CreateAABBFrom(gameObject.Model);
            var boundingBox = BoundingVolumesExtensions.Scale(objectBox, gameObject.Scale);
            OrientedBoundingBox = OrientedBoundingBox.FromAABB(boundingBox);
            SetBoundingBox(gameObject);
        }

        public void Update(GameObject gameObject)
        {
            SetBoundingBox(gameObject);
        }

        public void Draw(Gizmos.Gizmos gizmos)
        {
            gizmos.DrawCube(OrientedBoundingBoxWorld);
        }

        public bool CollidesWith(CollisionComponent other)
        {
            return OrientedBoundingBox.Intersects(other.OrientedBoundingBox);
        }
        public bool CollidesWith(OrientedBoundingBox other)
        {
            return OrientedBoundingBox.Intersects(other);
        }

        public OrientedBoundingBox DisplacedOBB(Vector3 center)
        {
            return new OrientedBoundingBox(center, OrientedBoundingBox.Extents);
        }

        protected void SetBoundingBox(GameObject gameObject)
        {
            BoxWorldSpace.Min = objectBox.Min + gameObject.World.Translation;
            BoxWorldSpace.Max = objectBox.Max + gameObject.World.Translation;
            OrientedBoundingBox.Center = gameObject.Position;
            OrientedBoundingBox.Orientation = gameObject.GetRotationMatrix();
            OrientedBoundingBoxWorld = CreateObjectWorld(gameObject);
        }

        private Matrix CreateObjectWorld(GameObject gameObject)
        {
            return Matrix.CreateScale(OrientedBoundingBox.Extents * 2f) 
                    * gameObject.GetRotationMatrix()
                        * Matrix.CreateTranslation(gameObject.Position + new Vector3(0f, OrientedBoundingBox.Extents.Y, 0f));
        }


    }
}
