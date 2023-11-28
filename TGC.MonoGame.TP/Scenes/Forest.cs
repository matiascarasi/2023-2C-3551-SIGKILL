using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using System;
using System.Collections.Generic;
using TGC.MonoGame.TP.Components.Graphics;
using TGC.MonoGame.TP.Helpers;
using TGC.MonoGame.TP.Actors;
using TGC.MonoGame.TP.Physics;
using TGC.MonoGame.TP.Components.Physics;
using BepuPhysics.Collidables;

namespace TGC.MonoGame.TP.Scenes
{
    class Forest : IScene
    {

        private const double SMALL_TREES_MIN_SCALE = 1;
        private Vector2 Center;
        private readonly double _radius;
        private readonly int smallTreesAmount;
        private readonly int rocksAmount;
        private PhysicsEngine PhysicsEngine { get; set; }
        public Forest(Vector2 center, double radius, double density, PhysicsEngine physicsEngine)
        {
            Center = center;
            _radius = radius;
            smallTreesAmount = (int) (Math.PI * Math.Pow(_radius, 2) * 0.000005 * density);
            rocksAmount = (int)(Math.PI * Math.Pow(_radius, 2) * 0.00000075 * density);
            PhysicsEngine = physicsEngine;
        }

        public void LoadContent(ContentManager Content, HeightMap HeightMap, List<GameObject> Objects)
        {
            Random Random = new(Guid.NewGuid().GetHashCode());

            // LOAD SMALL TREES
            for (var i = 0; i < smallTreesAmount; i++)
            {
                var xzPosition = AlgebraHelper.GetRandomPointInCircle(Center, _radius, Random);
                var rotation = Convert.ToSingle(AlgebraHelper.FULL_ROTATION * Random.NextDouble());
                var scale = Convert.ToSingle(Math.Max(SMALL_TREES_MIN_SCALE, Random.NextDouble()));
                var position = new Vector3(xzPosition.X, HeightMap.Height(xzPosition.X, xzPosition.Y), xzPosition.Y);
                var treeBox = new Box(50f, 200f, 50f);
                var tree = new GameObject(
                    new TreeGraphicsComponent(),
                    new StaticPhysicsComponent<Box>(
                        PhysicsEngine,
                        "Tree Box",
                        treeBox,
                        position,
                        Quaternion.CreateFromAxisAngle(Vector3.Up, rotation)),
                    scale,
                    0.1f
                );

                tree.LoadContent(Content);

                Objects.Add(tree);

            }

            // TODO: LOAD FERNS
            for (var i = 0; i < rocksAmount; i++)
            {
                var xzPosition = AlgebraHelper.GetRandomPointInCircle(Center, _radius, Random);
                var rotation = Convert.ToSingle(AlgebraHelper.FULL_ROTATION * Random.NextDouble());
                var scale = Convert.ToSingle(Math.Max(SMALL_TREES_MIN_SCALE, Random.NextDouble()));
                var position = new Vector3(xzPosition.X, HeightMap.Height(xzPosition.X, xzPosition.Y), xzPosition.Y);
                var rockBox = new Box(250f, 50f, 250f);
                var rock = new GameObject(
                    new BushGraphicsComponent(),
                    new StaticPhysicsComponent<Box>(
                        PhysicsEngine,
                        "Rock Box",
                        rockBox,
                        position,
                        Quaternion.CreateFromAxisAngle(Vector3.Up, rotation)),
                    scale,
                    0.1f
                );

                rock.LoadContent(Content);

                Objects.Add(rock);

            }

        }
    }
}
