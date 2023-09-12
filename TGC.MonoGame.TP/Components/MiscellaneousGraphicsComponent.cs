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
        private const string TankModelsFolder = "TankWars/";
        public const string ContentFolderEffects = "Effects/";
        private readonly ContentManager Content;
        private readonly string _objectFolder;
        private readonly string _objectName;

        public GraphicsDeviceManager Graphics { get; set; }
        private Effect ObjectEffect { get; set; }
        
        private Matrix World { get; set; }


        public MiscellaneousGraphicsComponent(ContentManager content, string ObjectFolder, string objectName)
        {
            _objectFolder = ObjectFolder;
            _objectName = objectName;
            Content = content;
        }

        public void LoadContent(GameObject Object)
        {
            Object.Model = Content.Load<Model>(PathsService.ContentFolder3D + MiscellaneousFolder + "/" + _objectFolder + "/" + _objectName);
            ObjectEffect = Content.Load<Effect>(ContentFolderEffects + "BasicShader");
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
            foreach (var mesh in Object.Model.Meshes)
            {
                World =  Matrix.CreateScale(Object.Scale) * Matrix.CreateRotationY(MathHelper.ToRadians(Object.YAxisRotation)) * Matrix.CreateTranslation(Object.Position)*mesh.ParentBone.Transform;
                ObjectEffect.Parameters["World"].SetValue(World);
                mesh.Draw();
            }
            
            Object.World = Matrix.CreateScale(Object.Scale) * Matrix.CreateRotationY(MathHelper.ToRadians(Object.YAxisRotation)) * Matrix.CreateTranslation(Object.Position);
            Object.Model.Draw(Object.World, view, projection);
        }

    }
}
