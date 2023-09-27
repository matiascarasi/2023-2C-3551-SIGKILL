﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TGC.MonoGame.TP.Content.Actors;

namespace TGC.MonoGame.TP.Components
{
    interface IGraphicsComponent
    {
        Model Model
        {
            get;
        }
        public void LoadContent();
        public void Draw(GameObject gameObject, GameTime gameTime, Matrix view, Matrix projection);
    }
}
