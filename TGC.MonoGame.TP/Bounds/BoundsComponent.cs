using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BepuPhysics.Constraints;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using TGC.MonoGame.TP.Actors;
using TGC.MonoGame.TP.Collisions;
using TGC.MonoGame.TP.Components.Collisions;
using TGC.MonoGame.TP.Components.Graphics;
using TGC.MonoGame.TP.Components.Inputs;

namespace TGC.MonoGame.TP.Bounds
{
    internal class BoundsComponent
    {
        private List<GameObject> Fences { get; set; }
        private HeightMap HeightMap;
        private Texture Texture { get; set; }
        private Model FenceModel { get; set; }
        private Effect Effect { get; set; }
        public BoundsComponent(ContentManager contentManager, HeightMap heightMap) {

            HeightMap = heightMap;
            FenceModel = contentManager.Load<Model>("Models/TankWars/Miscellaneous/Fence/chainLinkFence_low");
            Texture = contentManager.Load<Texture>("Models/TankWars/Miscellaneous/Fence/chainLinkFence_low_mat_BaseColor");
            Effect = contentManager.Load<Effect>("Effects/Fences");

            var objectBox = BoundingVolumesExtensions.CreateAABBFrom(FenceModel);
            var extents = objectBox.Max - objectBox.Min;

            var quantity = HeightMap.GetScaleXZ() * 2 / (extents.X / 2) ;
            var scale = 5f;
            var sizes = 4;
            System.Diagnostics.Debug.WriteLine("Q: " + quantity);

            Fences = new List<GameObject>();


            for (int i = 0; i < sizes; i++)
            {
                var rotation = 0f;
                var zPosition = 20f * HeightMap.GetScaleXZ();
                
                if (i % 2 != 0)
                {
                    zPosition = -20f * HeightMap.GetScaleXZ(); ;
                }

                for (int j = 0; j < quantity * 2; j++)
                {

                    var xPosition = extents.X * 1.8f + (extents.X * scale * j) - 20f * HeightMap.GetScaleXZ(); ;
                    
                    var fence = new GameObject(
                        new FenceGraphicsComponent(),
                        new Vector3(xPosition, HeightMap.Height(0f, 0f), zPosition),
                        rotation,
                        scale,
                        1f
                    );

                    if (i % 2 == 0)
                    {
                        rotation = 90f;
                        fence = new GameObject(
                            new FenceGraphicsComponent(),
                            new Vector3(zPosition, HeightMap.Height(0f, 0f), xPosition),
                            rotation,
                            scale,
                            1f
                        );
                    }
                    Fences.Add(fence);
                }
            }

            for (int i = 0; i < sizes; i++)
            {
                var rotation = 90f;
                var xPosition = 20f * HeightMap.GetScaleXZ(); ;

                if (i % 2 != 0)
                {
                    xPosition = -20f * HeightMap.GetScaleXZ(); ;
                }

                for (int j = 0; j < quantity * 2; j++)
                {

                    var zPosition = extents.X * 1.8f + (extents.X * scale * j) - 20f * HeightMap.GetScaleXZ(); ;

                    var fence = new GameObject(
                        new FenceGraphicsComponent(),
                        new Vector3(xPosition, HeightMap.Height(0f, 0f), zPosition),
                        rotation,
                        scale,
                        1f
                    );

                    if (i % 2 == 0)
                    {
                        rotation = 0f;
                            fence = new GameObject(
                            new FenceGraphicsComponent(),
                            new Vector3(zPosition, HeightMap.Height(0f, 0f), xPosition),
                            rotation,
                            scale,
                            1f
                        );
                    }
                    Fences.Add(fence);
                }
            }





        }

        public void LoadContent (ContentManager contentManager)
        {
            foreach( GameObject fence in  Fences)
            {
                fence.LoadContent( contentManager );
            }
        }
        public void Update(GameTime gameTime)
        {
            foreach (GameObject fence in Fences)
            {
                fence.Position = new Vector3(fence.Position.X, HeightMap.Height(fence.Position.X, fence.Position.Z) - 600f, fence.Position.Z);
                fence.Update(gameTime);
            }
        }
        public void Draw(GameTime gameTime, Matrix view, Matrix projection, Vector3 cameraPosition)
        {
            foreach (GameObject fence in Fences)
            {
                fence.Draw(gameTime, view, projection, cameraPosition);
            }
        }
        public void Draw(GameTime gameTime, Matrix view, Matrix projection, Vector3 cameraPosition, Gizmos.Gizmos gizmos)
        {
            foreach (GameObject fence in Fences)
            {
                fence.Draw(gameTime, view, projection, cameraPosition, gizmos);
            }
        }
    }
}
