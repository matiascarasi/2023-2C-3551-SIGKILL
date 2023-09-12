using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;

namespace TGC.MonoGame.TP.Scenes
{
    interface IScene
    {
        public void LoadContent(ContentManager content);
        public void Draw(GameTime gameTime, Matrix view, Matrix projection);
    }
}
