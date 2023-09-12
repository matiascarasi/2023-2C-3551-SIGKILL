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
    class MiscellaneousGraphicsComponent : IGraphicsComponent
    {

        private const string MiscellaneousFolder = "TankWars/Miscellaneous";
        private readonly ContentManager Content;
        private readonly string _objectFolder;
        private readonly string _objectName;

        public MiscellaneousGraphicsComponent(ContentManager content, string ObjectFolder, string objectName)
        {
            _objectFolder = ObjectFolder;
            _objectName = objectName;
            Content = content;
        }

        public void LoadContent(GameObject Object)
        {
            Object.Model = Content.Load<Model>(PathsService.ContentFolder3D + MiscellaneousFolder + "/" + _objectFolder + "/" + _objectName);
        }

        public void Draw(GameObject Object, GameTime gameTime, Matrix view, Matrix projection)
        {
            Object.World = Matrix.CreateScale(Object.Scale) * Matrix.CreateRotationY(MathHelper.ToRadians(Object.YAxisRotation)) * Matrix.CreateTranslation(Object.Position);
            Object.Model.Draw(Object.World, view, projection);
        }

    }
}
