using System.Collections.Generic;
using BepuPhysics.Collidables;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using TGC.MonoGame.TP.Actors;
using TGC.MonoGame.TP.Components.Graphics;
using TGC.MonoGame.TP.Components.Physics;
using TGC.MonoGame.TP.Helpers;
using TGC.MonoGame.TP.Physics;

namespace TGC.MonoGame.TP.Bounds
{
    internal class BoundsComponent
    {
        private List<GameObject> Fences { get; set; }
        private readonly Terrain terrain;
        public BoundsComponent(ContentManager contentManager, Terrain terrain, PhysicsEngine physicsEngine)
        {

            this.terrain = terrain;
            var FenceModel = contentManager.Load<Model>("Models/TankWars/Miscellaneous/Fence/chainLinkFence_low");
            var (min, max) = ModelHelper.GetMinMax(FenceModel);
            var extents = max - min;
            System.Diagnostics.Debug.WriteLine("EXTENTSMax: " + max);
            System.Diagnostics.Debug.WriteLine("EXTENTSMin: " + min);

            var heightMapExtents = new Vector2(terrain.currentHeightmap.Width, terrain.currentHeightmap.Height);
            System.Diagnostics.Debug.WriteLine("EXTENTS: " + extents);
            System.Diagnostics.Debug.WriteLine("HEIGHT MAP EXTENTS: " + heightMapExtents);

            var quantity = heightMapExtents.X / (extents.X / 2);
            var scale = 5f;
            var sizes = 4;
            var fenceShape = new Box(max.X * 2 * scale, max.Y * 2 * scale, max.Z * 2 * scale);
            System.Diagnostics.Debug.WriteLine("Q: " + quantity);

            Fences = new List<GameObject>();


            for (int i = 0; i < sizes; i++)
            {
                var rotation = 0f;
                var zPosition = 20000f;

                if (i % 2 != 0)
                {
                    zPosition = -20000f;
                }

                for (int j = 0; j < quantity * 2; j++)
                {

                    var xPosition = extents.X * 1.8f + (extents.X * scale * j) - 20000f;

                    var fence = new GameObject(
                        new FenceGraphicsComponent(),
                        new StaticPhysicsComponent<Box>(
                                physicsEngine,
                                "Fence",
                                fenceShape,
                                new Vector3(xPosition, terrain.Height(0f, 0f), zPosition),
                                Quaternion.CreateFromAxisAngle(Vector3.Up, rotation)),
                        scale,
                        1f
                    );

                    if (i % 2 == 0)
                    {
                        rotation = MathHelper.ToRadians(90f);
                        fence = new GameObject(
                            new FenceGraphicsComponent(),
                            new StaticPhysicsComponent<Box>(
                                physicsEngine,
                                "Fence",
                                fenceShape,
                                new Vector3(zPosition, terrain.Height(0f, 0f), xPosition),
                                Quaternion.CreateFromAxisAngle(Vector3.Up, rotation)),
                            scale,
                            1f
                        );
                    }
                    Fences.Add(fence);
                }
            }

            for (int i = 0; i < sizes; i++)
            {
                var rotation = MathHelper.ToRadians(90f);
                var xPosition = 20000f;

                if (i % 2 != 0)
                {
                    xPosition = -20000f;
                }

                for (int j = 0; j < quantity * 2; j++)
                {

                    var zPosition = extents.X * 1.8f + (extents.X * scale * j) - 20000f;

                    var fence = new GameObject(
                        new FenceGraphicsComponent(),
                        new StaticPhysicsComponent<Box>(
                            physicsEngine,
                            "Fence",
                            fenceShape,
                            new Vector3(xPosition, terrain.Height(0f, 0f), zPosition),
                            Quaternion.CreateFromAxisAngle(Vector3.Up, rotation)),
                        scale,
                        1f
                    );

                    if (i % 2 == 0)
                    {
                        rotation = 0f;
                        fence = new GameObject(
                        new FenceGraphicsComponent(),
                        new StaticPhysicsComponent<Box>(
                            physicsEngine,
                            "Fence",
                            fenceShape,
                            new Vector3(zPosition, terrain.Height(0f, 0f), xPosition),
                            Quaternion.CreateFromAxisAngle(Vector3.Up, rotation)),
                        scale,
                        1f
                    );
                    }
                    Fences.Add(fence);
                }
            }





        }

        public void LoadContent(ContentManager contentManager)
        {
            foreach (GameObject fence in Fences)
            {
                fence.LoadContent(contentManager);
            }
        }
        public void Update(GameTime gameTime)
        {
            foreach (GameObject fence in Fences)
            {
                fence.Position = new Vector3(fence.Position.X, terrain.Height(fence.Position.X, fence.Position.Z) - 600f, fence.Position.Z);
                fence.Update(gameTime);
            }
        }
        public void Draw(GameTime gameTime, Matrix view, Matrix projection)
        {
            foreach (GameObject fence in Fences)
            {
                fence.Draw(gameTime, view, projection);
            }
        }
        public void Draw(GameTime gameTime, Matrix view, Matrix projection, Gizmos.Gizmos gizmos)
        {
            foreach (GameObject fence in Fences)
            {
                fence.Draw(gameTime, view, projection, gizmos);
            }
        }
    }
}
