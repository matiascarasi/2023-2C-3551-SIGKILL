using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using TGC.MonoGame.TP.Actors;

namespace TGC.MonoGame.TP.Components.Graphics
{
    class PanzerGraphicsComponent : TankGraphicsComponent
    {
        public PanzerGraphicsComponent() : base(
                "Models/TankWars/Panzer/Panzer",
                "Effects/BasicTexture",
                "Models/TankWars/Panzer/PzVl_Tiger_I_0",
                new Dictionary<string, string> { { "Treadmill1", "Effects/WrapTexture" }, { "Treadmill2", "Effects/WrapTexture" } },
                new Dictionary<string, string> { { "Treadmill1", "Models/TankWars/Panzer/PzVI_Tiger_I_track_0" }, { "Treadmill2", "Models/TankWars/Panzer/PzVI_Tiger_I_track_0" } }
            )
        { }

        public override Vector3 GetCannonDirection(GameObject gameObject)
        {
            var cannonMatrix = gameObject.Bones[gameObject.Model.Bones["Cannon"].Index] * gameObject.GetRotationMatrix();
            return -Vector3.Normalize(cannonMatrix.Forward);
        }

        public override void LoadContent(GameObject gameObject, ContentManager Content)
        {
            base.LoadContent(gameObject, Content);
            CannonTransform = gameObject.Bones[gameObject.Model.Bones["Cannon"].Index];
            TurretTransform = gameObject.Bones[gameObject.Model.Bones["Turret"].Index];
        }

        public override void Update(GameObject gameObject, GameTime gameTime)
        {
            var turretRotation = Matrix.CreateRotationY(TurretRotation);
            gameObject.Bones[gameObject.Model.Bones["Cannon"].Index] =  Matrix.CreateRotationX(-CannonRotation) * CannonTransform * turretRotation;
            gameObject.Bones[gameObject.Model.Bones["Turret"].Index] = turretRotation * TurretTransform;
        }
    }
}