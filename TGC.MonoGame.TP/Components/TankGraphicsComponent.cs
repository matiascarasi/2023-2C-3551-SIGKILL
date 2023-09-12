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
        public const string ContentFolderEffects = "Effects/";

        private readonly ContentManager Content;
        private readonly string _tankName;
        private Effect Effect { get; set; }
        
        private float Rotation { get; set; }
        private Matrix World { get; set; }
        private Matrix View { get; set; }
        private Matrix Projection { get; set; }


        public TankGraphicsComponent(ContentManager content, string tankName)
        {
            _tankName = tankName;
            Content = content;

        }

        public GraphicsDeviceManager Graphics { get; set; }

        public void LoadContent(GameObject Tank)
        {
            Tank.Model = Content.Load<Model>(PathsService.ContentFolder3D + TankModelsFolder + _tankName + "/" + _tankName);
            Effect = Content.Load<Effect>(ContentFolderEffects + "BasicShader");
            foreach (var mesh in Tank.Model.Meshes)
            {
                // Un mesh puede tener mas de 1 mesh part (cada 1 puede tener su propio efecto).
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
            var rotationMatrix = Matrix.CreateRotationY(Rotation);

            foreach (var mesh in Tank.Model.Meshes)
            {
                World =  Matrix.CreateScale(Tank.Scale) * Matrix.CreateRotationY(MathHelper.ToRadians(Tank.YAxisRotation))*mesh.ParentBone.Transform * rotationMatrix;
                Effect.Parameters["World"].SetValue(World);
                mesh.Draw();
            }
         
        }

    }
}