using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TGC.MonoGame.TP.Content.Actors;
using TGC.MonoGame.TP.Content.Controllers;

namespace TGC.MonoGame.TP.Components
{
    class TankGraphicsComponent : IGraphicsComponent
    {

        private const string TankModelsFolder = "TankWars/";
        private readonly ContentManager Content;
        private readonly string _tankName;

        public TankGraphicsComponent(ContentManager content, string tankName)
        {
            _tankName = tankName;
            Content = content;
        }

        public void LoadContent(GameObject Tank)
        {
            Tank.Model = Content.Load<Model>(PathsService.ContentFolder3D + TankModelsFolder + _tankName + "/" + _tankName);
        }

        public void Draw(GameObject Tank, GameTime gameTime, Matrix view, Matrix projection)
        {
            Tank.World = Matrix.CreateScale(Tank.Scale) * Matrix.CreateRotationY(MathHelper.ToRadians(Tank.YAxisRotation)) * Matrix.CreateTranslation(Tank.Position);
            Tank.Model.Draw(Tank.World, view, projection);
        }

    }
}
