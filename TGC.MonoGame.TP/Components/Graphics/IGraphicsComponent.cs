using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace TGC.MonoGame.TP.Components.Graphics
{
    public interface IGraphicsComponent
    {
        public void LoadContent(GameObject gameObject, ContentManager content);
        public void Update(GameObject gameObject, MouseCamera mouseCamera);
        public void Draw(GameObject gameObject, GameTime gameTime, Matrix view, Matrix projection);
    }
}
