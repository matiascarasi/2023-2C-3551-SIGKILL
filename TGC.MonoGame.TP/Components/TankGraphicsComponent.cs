using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using TGC.MonoGame.TP.Content.Actors;
using TGC.MonoGame.TP.Content.Controllers;

namespace TGC.MonoGame.TP.Components
{
    class TankGraphicsComponent : IGraphicsComponent
    {

        private const string TankModelsFolder = "TankWars/";

        private readonly ContentManager Content;
        private readonly string _tankName;
        private Effect Effect { get; set; }

        public TankGraphicsComponent(ContentManager content, string tankName)
        {
            _tankName = tankName;
            Content = content;

        }

        public GraphicsDeviceManager Graphics { get; set; }

        public void LoadContent(GameObject Tank)
        {
            Tank.Model = Content.Load<Model>(PathsService.ContentFolder3D + TankModelsFolder + _tankName + "/" + _tankName);
            Effect = Content.Load<Effect>(PathsService.ContentFolderEffects + "BasicShader");
            foreach (var mesh in Tank.Model.Meshes)
            {
                foreach (var meshPart in mesh.MeshParts)
                {
                    meshPart.Effect = Effect;
                }
            }
        }

        public void Draw(GameObject Tank, GameTime gameTime, Matrix view, Matrix projection)
        {

            Effect.Parameters["View"].SetValue(view);
            Effect.Parameters["Projection"].SetValue(projection);
            Effect.Parameters["DiffuseColor"].SetValue(Color.DarkRed.ToVector3());
            var scaleMatrix = Matrix.CreateScale(Tank.Scale);
            var rotationMatrix = Matrix.CreateRotationY(MathHelper.ToRadians(Tank.YAxisRotation));
            var translationMatrix = Matrix.CreateTranslation(Tank.Position);
            var world = scaleMatrix * rotationMatrix * translationMatrix;
            Tank.World = world;
            foreach (var mesh in Tank.Model.Meshes)
            {
                world = mesh.ParentBone.Transform * Tank.World;
                Effect.Parameters["World"].SetValue(world);
                mesh.Draw();
            }
        }
    }
}