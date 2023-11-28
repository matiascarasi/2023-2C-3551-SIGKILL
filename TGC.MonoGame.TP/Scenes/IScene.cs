using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using System.Collections.Generic;
using TGC.MonoGame.TP.Actors;

namespace TGC.MonoGame.TP.Scenes
{
    interface IScene
    {
        public void LoadContent(ContentManager content, HeightMap HeightMap, List<GameObject> objects);
    }
}
