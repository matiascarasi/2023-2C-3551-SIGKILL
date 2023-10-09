using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TGC.MonoGame.TP.Content.Actors;

namespace TGC.MonoGame.TP.Graphics
{
    public interface IGraphicsComponent
    {
        Model Model
        {
            get;
        }
        public void LoadContent(GameObject gameObject);
        public void Draw(GameObject gameObject, GameTime gameTime, Matrix view, Matrix projection);
    }
}
