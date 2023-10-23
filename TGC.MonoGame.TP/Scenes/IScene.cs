using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using System.Collections.Generic;

namespace TGC.MonoGame.TP.Scenes
{
    interface IScene
    {
        public void LoadContent(ContentManager content, Terrain terrain, List<GameObject> objects);
        public void Draw(GameTime gameTime, Matrix view, Matrix projection);
        public void Draw(GameTime gameTime, Matrix view, Matrix projection, Gizmos.Gizmos Gizmos);
    }
}
