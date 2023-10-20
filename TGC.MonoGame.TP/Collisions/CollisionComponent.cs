using System;
using System.Linq;
using System.Runtime.InteropServices;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TGC.MonoGame.TP.Content.Actors;
using TGC.MonoGame.TP.Gizmos;

namespace TGC.MonoGame.TP.Collisions
{
    public class CollisionComponent
    {
        private OrientedBoundingBox OrientedBoundingBox;
        private Matrix OrientedBoundingBoxWorld;
        //private BoundingBox BoundingBox;
        //private BoundingBox ObjectBox;
        public CollisionComponent(GameObject gameObject)
        {
            //ObjectBox = BoundingVolumesExtensions.CreateAABBFrom(gameObject.Model);
            //BoundingBox = new BoundingBox(ObjectBox.Min + gameObject.Position, ObjectBox.Max + gameObject.Position);
            var objectBox = BoundingVolumesExtensions.CreateAABBFrom(gameObject.Model);
            var temporaryCubeAABB = BoundingVolumesExtensions.Scale(objectBox, 1f);

            var hullMesh = gameObject.Model.Meshes.FirstOrDefault(mesh => mesh.Name == "Hull");
            if (hullMesh != null)
            {
                temporaryCubeAABB =
                            BoundingVolumesExtensions.FromMatrix(
                                Matrix.CreateScale(390f, 250f, 660f));

            }

            OrientedBoundingBox = OrientedBoundingBox.FromAABB(temporaryCubeAABB);
            OrientedBoundingBox.Center = BoundingVolumesExtensions.GetCenter(temporaryCubeAABB) + gameObject.Position;
            OrientedBoundingBox.Orientation = Matrix.CreateRotationY(gameObject.YAxisRotation);
            OrientedBoundingBoxWorld = CreateObjectWorld(gameObject.Position, gameObject.YAxisRotation);

           
        }

        public void Update(Vector3 position, float rotation)
        {
            //BoundingBox = new OrientedBoundingBox(ObjectBox.Min + position, ObjectBox.Max + position);
            OrientedBoundingBox.Center = position;
            OrientedBoundingBox.Orientation = Matrix.CreateRotationY(MathHelper.ToRadians(rotation));
            OrientedBoundingBoxWorld = CreateObjectWorld(position, rotation);
        }

        public bool CollidesWith(CollisionComponent other)
        {
            //return BoundingBox.Intersects(other.BoundingBox);
            return OrientedBoundingBox.Intersects(other.OrientedBoundingBox);

        }
        public Matrix CreateObjectWorld(Vector3 position, float rotation)
        {
            return Matrix.CreateScale(OrientedBoundingBox.Extents * 2f) 
                    * Matrix.CreateRotationY(MathHelper.ToRadians(rotation)) 
                        * Matrix.CreateTranslation(position + new Vector3(0f, OrientedBoundingBox.Extents.Y, 0f));
        }
        public void DrawBoundingVolume(Gizmos.Gizmos gizmos)
        {
            //Vector3[] points = {BoundingBox.Min, BoundingBox.Max};
            // Console.Write(points[0]);
            // Console.Write(points[1]);
            // Console.WriteLine();
            // gizmos.DrawPolyLine(ObjectBox.GetCorners());
            gizmos.DrawCube(OrientedBoundingBoxWorld);
        }
    }
}
