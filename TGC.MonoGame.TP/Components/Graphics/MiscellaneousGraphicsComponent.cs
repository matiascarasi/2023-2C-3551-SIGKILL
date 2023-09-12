using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using TGC.MonoGame.TP.Content.Services;

namespace TGC.MonoGame.TP.Components
{
    class MiscellaneousGraphicsComponent : IGraphicsComponent
    {
        private readonly string _path;
        public MiscellaneousGraphicsComponent(string path)
        {
            _path = path;
        }

        public void LoadContent(GameObject Object, ContentManager Content)
        {
            Object.Model = Content.Load<Model>(PathsService.MiscellaneousFolder + _path);
        }

        public void Draw(GameObject Object, GameTime gameTime, Matrix view, Matrix projection)
        {
            Object.World = Matrix.CreateScale(Object.Scale) * Matrix.CreateRotationY(MathHelper.ToRadians(Object.YAxisRotation)) * Matrix.CreateTranslation(Object.Position);
            Object.Model.Draw(Object.World, view, projection);
        }

    }
}
