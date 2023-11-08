using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using System;
using System.Linq;
using System.Collections.Generic;
using TGC.MonoGame.TP.Components.Collisions;
using TGC.MonoGame.TP.Components.Graphics;
using TGC.MonoGame.TP.Components.Inputs;
using TGC.MonoGame.TP.Helpers;
using TGC.MonoGame.TP.Actors;

namespace TGC.MonoGame.TP.Scenes
{
    class Forest : IScene
    {

        private const double SMALL_TREES_MIN_SCALE = 1.5;
        private Vector2 Center;
        private readonly double _radius;
        private readonly int smallTreesAmount;
        private readonly int rocksAmount;
        public Forest(Vector2 center, double radius, double density)
        {
            Center = center;
            _radius = radius;
            smallTreesAmount = (int) (Math.PI * Math.Pow(_radius, 2) * 0.000001 * density);
            rocksAmount = (int)(Math.PI * Math.Pow(_radius, 2) * 0.00000075 * density);

        }

        public void LoadContent(ContentManager Content, Terrain Terrain, List<GameObject> Objects)
        {
            Random Random = new(Guid.NewGuid().GetHashCode());

            // LOAD SMALL TREES
            for(var i = 0; i < smallTreesAmount; i++)
            {
                var position = AlgebraHelper.GetRandomPointInCircle(Center, _radius, Random);
                var rotation = Convert.ToSingle(AlgebraHelper.FULL_ROTATION * Random.NextDouble());
                var scale = Convert.ToSingle(Math.Max(SMALL_TREES_MIN_SCALE, Random.NextDouble()));
                
                var tree = new GameObject(
                    new Tree1GraphicsComponent(),
                    new Vector3(position.X, Terrain.Height(position.X, position.Y), position.Y),
                    rotation,
                    scale,
                    0.1f
                );

                tree.LoadContent(Content);

                if (Objects.Any(obj => obj.CollidesWith(tree)))
                    continue;

                Objects.Add(tree);

            }

            // TODO: LOAD FERNS
            for (var i = 0; i < rocksAmount; i++)
            {
                var position = AlgebraHelper.GetRandomPointInCircle(Center, _radius, Random);
                var rotation = Convert.ToSingle(AlgebraHelper.FULL_ROTATION * Random.NextDouble());
                var scale = Convert.ToSingle(Math.Max(SMALL_TREES_MIN_SCALE, Random.NextDouble()));

                var rock = new GameObject(
                    new RockGraphicsComponent(),
                    new Vector3(position.X, Terrain.Height(position.X, position.Y), position.Y),
                    rotation,
                    scale,
                    0.1f
                );

                rock.LoadContent(Content);

                if (Objects.Any(obj => obj.CollidesWith(rock)))
                    continue;

                Objects.Add(rock);

            }

        }
    }
}
