using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using TGC.MonoGame.TP.Actors;

namespace TGC.MonoGame.TP.Components.Graphics
{
    class T90GraphicsComponent : TankGraphicsComponent
    {
        public T90GraphicsComponent() : base(
                "Models/TankWars/T90/T90",
                "Effects/BlinnPhong",
                "Models/TankWars/T90/textures_mod/normal",
                "Models/TankWars/T90/textures_mod/hullC",
                new Dictionary<string, string> { { "Treadmill1", "Effects/WrapTexture" }, { "Treadmill2", "Effects/WrapTexture" } },
                new Dictionary<string, string> { { "Treadmill1", "Models/TankWars/T90/textures_mod/treadmills_normal" }, 
                    { "Treadmill2", "Models/TankWars/T90/textures_mod/treadmills_normal" } },
                new Dictionary<string, string> { { "Treadmill1", "Models/TankWars/T90/textures_mod/treadmills" },
                    { "Treadmill2", "Models/TankWars/T90/textures_mod/treadmills" } },
                "NormalMapping"
            )
        {
            CannonLength = 620f;
            CannonHeight = 150f;
        }

        public override Vector3 GetCannonDirection(GameObject gameObject)
        {
            var turretMatrix = gameObject.Bones[gameObject.Model.Bones["Turret"].Index] * gameObject.GetRotationMatrix();
            var cannonMatrix = gameObject.Bones[gameObject.Model.Bones["Cannon"].Index] * gameObject.GetRotationMatrix();
            return -Vector3.Normalize(turretMatrix.Up + cannonMatrix.Up);
        }

        public override void LoadContent(GameObject gameObject, ContentManager Content)
        {
            base.LoadContent(gameObject, Content);
            CannonTransform = gameObject.Bones[gameObject.Model.Bones["Cannon"].Index];
            TurretTransform = gameObject.Bones[gameObject.Model.Bones["Turret"].Index];
        }

        public override void Update(GameObject gameObject, GameTime gameTime)
        {
            var turretRotation = Matrix.CreateRotationZ(TurretRotation);
            gameObject.Bones[gameObject.Model.Bones["Cannon"].Index] = Matrix.CreateRotationX(-CannonRotation) * CannonTransform * Matrix.CreateRotationY(TurretRotation);
            gameObject.Bones[gameObject.Model.Bones["Turret"].Index] = turretRotation * TurretTransform;
        }
    }
}