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
        private Effect TextureEffect;
        private Effect TextureWrapEffect;
        private Texture HullTexture;
        private Texture TreadTexture;

        private readonly string HullTexturePath;
        private readonly string TreadTexturePath;

        public TankGraphicsComponent(ContentManager content, string tankName, string hullTexturePath, string treadTexturePath)
        {
            _tankName = tankName;
            Content = content;
            HullTexturePath = hullTexturePath;
            TreadTexturePath = treadTexturePath;
        }

        public GraphicsDeviceManager Graphics { get; set; }

        public void LoadContent(GameObject Tank)
        {
            Tank.Model = Content.Load<Model>(PathsService.ContentFolder3D + TankModelsFolder + _tankName + "/" + _tankName);
            HullTexture = Content.Load<Texture>(HullTexturePath);
            TreadTexture = Content.Load<Texture>(TreadTexturePath);
            TextureEffect = Content.Load<Effect>(PathsService.ContentFolderEffects + "BasicTexture");
            TextureWrapEffect = Content.Load<Effect>(PathsService.ContentFolderEffects + "WrapTexture");
            foreach (var mesh in Tank.Model.Meshes)
            {
                foreach (var meshPart in mesh.MeshParts)
                {
                    if (mesh.Name == "Treadmill1" || mesh.Name == "Treadmill2")
                    {
                        meshPart.Effect = TextureWrapEffect;
                    }
                    else
                    {
                        meshPart.Effect = TextureEffect;
                    }
                }
            }
        }

        public void Draw(GameObject Tank, GameTime gameTime, Matrix view, Matrix projection)
        {

            var scaleMatrix = Matrix.CreateScale(Tank.Scale);
            var rotationMatrix = Matrix.CreateRotationY(MathHelper.ToRadians(Tank.YAxisRotation));
            var translationMatrix = Matrix.CreateTranslation(Tank.Position);
            var world = scaleMatrix * rotationMatrix * translationMatrix;
            Matrix[] matrices = new Matrix[Tank.Model.Bones.Count];
            Tank.Model.CopyAbsoluteBoneTransformsTo(matrices);
            Tank.World = world;
            Effect effect;
            foreach (var mesh in Tank.Model.Meshes)
            {
                if (mesh.Name == "Treadmill1" || mesh.Name == "Treadmill2")
                {
                    effect = TextureWrapEffect;
                    effect.Parameters["Texture"].SetValue(TreadTexture);
                }
                else
                {
                    effect = TextureEffect;
                    effect.Parameters["Texture"].SetValue(HullTexture);
                }
                effect.Parameters["View"].SetValue(view);
                effect.Parameters["Projection"].SetValue(projection);
                world = matrices[mesh.ParentBone.Index] * Tank.World;
                effect.Parameters["World"].SetValue(world);
                mesh.Draw();
            }
        }
    }
}