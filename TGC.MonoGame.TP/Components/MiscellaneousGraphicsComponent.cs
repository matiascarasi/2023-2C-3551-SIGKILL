using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
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


        public GraphicsDeviceManager Graphics { get; set; }
        private Effect ObjectEffect { get; set; }

        public MiscellaneousGraphicsComponent(ContentManager content, string ObjectFolder, string objectName)
        {
            _objectFolder = ObjectFolder;
            _objectName = objectName;
            Content = content;
        }

        public void LoadContent(GameObject Object)
        {
            Object.Model = Content.Load<Model>(PathsService.ContentFolder3D + MiscellaneousFolder + "/" + _objectFolder + "/" + _objectName);

            ObjectEffect = Content.Load<Effect>(PathsService.ContentFolderEffects + "BasicShader");
            foreach (var mesh in Object.Model.Meshes)
            {
                foreach (var meshPart in mesh.MeshParts)
                {
                    meshPart.Effect = ObjectEffect;
                }
            }
        }


        public void Draw(GameObject Object, GameTime gameTime, Matrix view, Matrix projection)
        {
            ObjectEffect.Parameters["View"].SetValue(view);
            ObjectEffect.Parameters["Projection"].SetValue(projection);
            ObjectEffect.Parameters["DiffuseColor"].SetValue(Color.Brown.ToVector3());
            var scaleMatrix = Matrix.CreateScale(Object.Scale);
            var rotationMatrix = Matrix.CreateRotationY(MathHelper.ToRadians(Object.YAxisRotation));
            var translationMatrix = Matrix.CreateTranslation(Object.Position);
            var world = scaleMatrix * rotationMatrix * translationMatrix;
            Object.World = world;
            foreach (var mesh in Object.Model.Meshes)
            {
                world = mesh.ParentBone.Transform * Object.World;
                ObjectEffect.Parameters["World"].SetValue(world);
                mesh.Draw();
            }

        }

    }
}
