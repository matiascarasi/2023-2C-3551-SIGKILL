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
                "Effects/BlinnPhong",
                "Models/TankWars/Panzer/PzVI_Tiger_I_NM",
                "Models/TankWars/Panzer/PzVl_Tiger_I",
                new Dictionary<string, string> { { "Treadmill1", "Effects/WrapTexture" }, { "Treadmill2", "Effects/WrapTexture" } },
                new Dictionary<string, string> { { "Treadmill1", "Models/TankWars/Panzer/PzVI_Tiger_I_track_NM" }, { "Treadmill2", "Models/TankWars/Panzer/PzVI_Tiger_I_track_NM" } },
                new Dictionary<string, string> { { "Treadmill1", "Models/TankWars/Panzer/PzVI_Tiger_I_track" }, { "Treadmill2", "Models/TankWars/Panzer/PzVI_Tiger_I_track" } },
                "NormalMapping"
            )
        {
            CannonLength = 650f;
            CannonHeight = 200f;
            WheelsAmount = 20;
        }
        protected override void RotateTurretAndCannon(GameObject gameObject)
        {
            var turretRotation = Matrix.CreateRotationY(TurretRotation);
            gameObject.Bones[gameObject.Model.Bones["Cannon"].Index] = Matrix.CreateRotationX(-CannonRotation) * CannonTransform * turretRotation;
            gameObject.Bones[gameObject.Model.Bones["Turret"].Index] = turretRotation * TurretTransform;
        }
        protected override void RotateWheels(GameObject gameObject)
        {
            var velocity = gameObject.Velocity.Length();

            for (var i = 0; i < WheelsAmount; i++) 
                gameObject.Bones[gameObject.Model.Bones["Wheel"+(i+1)].Index] = Matrix.CreateRotationX(-MathHelper.ToRadians(velocity)) * WheelsTransforms[i];
        }

        public override Vector3 GetCannonDirection(GameObject gameObject)
        {
            var cannonMatrix = gameObject.Bones[gameObject.Model.Bones["Cannon"].Index] * gameObject.GetRotationMatrix();
            return -Vector3.Normalize(cannonMatrix.Forward);
        }
    }
}