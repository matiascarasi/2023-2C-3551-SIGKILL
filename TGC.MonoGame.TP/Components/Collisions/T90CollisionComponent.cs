using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using TGC.MonoGame.TP.Actors;
using TGC.MonoGame.TP.Collisions;

namespace TGC.MonoGame.TP.Components.Collisions
{
    class T90CollisionComponent : CollisionComponent
    {
        public override void LoadContent(GameObject gameObject)
        {
            var boundingBox = BoundingVolumesExtensions.FromMatrix(Matrix.CreateScale(390f, 250f, 660f));
            OrientedBoundingBox = OrientedBoundingBox.FromAABB(boundingBox);
            SetBoundingBox(gameObject);
        }
    }
}
