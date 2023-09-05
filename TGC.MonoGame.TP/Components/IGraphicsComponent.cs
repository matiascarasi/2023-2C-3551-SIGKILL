using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TGC.MonoGame.TP.Content.Actors;

namespace TGC.MonoGame.TP.Components
{
    interface IGraphicsComponent
    {
        public void LoadContent(GameObject gameObject);
        public void Draw(GameObject gameObject, GameTime gameTime, Matrix view, Matrix projection);
    }
}
