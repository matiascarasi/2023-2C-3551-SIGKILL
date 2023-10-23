using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using System;
using System.Linq;
using System.Collections.Generic;
using TGC.MonoGame.TP.Components.Collisions;
using TGC.MonoGame.TP.Components.Graphics;
using TGC.MonoGame.TP.Components.Inputs;
using TGC.MonoGame.TP.Helpers;

namespace TGC.MonoGame.TP.Scenes
{
    class Forest : IScene
    {

        private const double SMALL_TREES_MIN_SCALE = 0.5;
        private readonly List<GameObject> Miscellaneous;
        private Vector2 Center;
        private readonly double _radius;
        private readonly int smallTreesAmount;

        public Forest(Vector2 center, double radius, double density)
        {
            Center = center;
            _radius = radius;
            smallTreesAmount = (int) (Math.PI * Math.Pow(_radius, 2) * 0.000001 * density);
            Miscellaneous = new List<GameObject>();
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
                    new NoInputComponent(),
                    new CollisionComponent(),
                    new Vector3(position.X, Terrain.Height(position.X, position.Y), position.Y),
                    rotation,
                    scale,
                    0.1f,
                    0f
                );

                tree.LoadContent(Content);

                if (Objects.Any(obj => obj.CollidesWith(tree)))
                    continue;

                Miscellaneous.Add(tree);
                Objects.Add(tree);

            }

            // TODO: LOAD FERNS

        }

        public void Draw(GameTime gameTime, Matrix view, Matrix projection)
        {
            foreach( var misc in Miscellaneous )
            {
                misc.Draw(gameTime, view, projection);
            }
        }

        public void Draw(GameTime gameTime, Matrix view, Matrix projection, Gizmos.Gizmos Gizmos)
        {
            foreach (var misc in Miscellaneous)
            {
                misc.Draw(gameTime, view, projection, Gizmos);
            }
        }
    }
}
