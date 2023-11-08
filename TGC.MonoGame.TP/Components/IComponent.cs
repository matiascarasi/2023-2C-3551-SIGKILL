﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using TGC.MonoGame.TP.Actors;

namespace TGC.MonoGame.TP.Components
{
    public interface IComponent
    {
        public void LoadContent(GameObject gameObject, ContentManager content);
        public void Update(GameObject gameObject, GameTime gameTime);
        public void Draw(GameObject gameObject, GameTime gameTime, Matrix view, Matrix projection);
    }
}