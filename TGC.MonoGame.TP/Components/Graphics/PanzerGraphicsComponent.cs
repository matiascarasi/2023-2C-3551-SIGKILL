﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using TGC.MonoGame.TP.Content.Services;

namespace TGC.MonoGame.TP.Components
{
    class PanzerGraphicsComponent : IGraphicsComponent
    {

        public void LoadContent(GameObject Tank, ContentManager Content)
        {
            Tank.Model = Content.Load<Model>(PathsService.TankModelsFolder + "Panzer/Panzer");
        }

        public void Draw(GameObject Tank, GameTime gameTime, Matrix view, Matrix projection)
        {
            Tank.World = Matrix.CreateScale(Tank.Scale) * Matrix.CreateRotationY(MathHelper.ToRadians(Tank.YAxisRotation)) * Matrix.CreateTranslation(Tank.Position);
            Tank.Model.Draw(Tank.World, view, projection);
        }

    }
}