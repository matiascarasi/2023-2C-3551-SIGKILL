using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using System;
using System.Linq;
using TGC.MonoGame.TP.Components;
using TGC.MonoGame.TP.Helpers;

namespace TGC.MonoGame.TP.Scenes
{
    class Forest : IScene
    {

        private readonly GameObject[] Miscellaneous;

        private Vector2 Center;
        private readonly double _radius;

        private readonly int smallTreesAmount;

        public Forest(Vector2 center, double radius, double density)
        {
            Center = center;
            _radius = radius;
            smallTreesAmount = (int) (Math.PI * Math.Pow(_radius, 2) * 0.00001 * density);
            Miscellaneous = new GameObject[smallTreesAmount];
        }

        public void LoadContent(ContentManager Content)
        {
            Random Random = new();
            Vector2[] posistionsBuffer = new Vector2[smallTreesAmount];

            // LOAD SMALL TREES
            for(var i = 0; i < smallTreesAmount; i++)
            {
                var position = AlgebraHelper.GetRandomPointInCircle(Center, _radius, Random);
                var rotation = Convert.ToSingle(AlgebraHelper.FULL_ROTATION * Random.NextDouble());
                var scale = Convert.ToSingle(Math.Max(0.7, Random.NextDouble()));

                // TODO: Check tree position doesn't collide
                
                var tree = new GameObject(
                    new T90GraphicsComponent(),
                    new EmptyInputComponent(),
                    new Vector3(position.X, 0f, position.Y),
                    rotation,
                    scale
                );

                tree.LoadContent(Content);

                Miscellaneous[i] = tree;
                posistionsBuffer[i] = position;
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
    }
}
