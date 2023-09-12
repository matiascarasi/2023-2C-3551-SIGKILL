using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;

namespace TGC.MonoGame.TP.Components
{
    interface IGraphicsComponent
    {
        public void LoadContent(GameObject gameObject, ContentManager content);
        public void Draw(GameObject gameObject, GameTime gameTime, Matrix view, Matrix projection);
    }
}
