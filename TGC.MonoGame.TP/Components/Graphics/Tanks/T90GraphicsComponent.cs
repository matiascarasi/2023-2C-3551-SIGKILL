using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using TGC.MonoGame.TP.Actors;

namespace TGC.MonoGame.TP.Components.Graphics
{
    class T90GraphicsComponent : TankGraphicsComponent
    {
        public T90GraphicsComponent(Terrain terrain) : base(
                "Models/TankWars/T90/T90",
                "Effects/BasicTexture",
                "Models/TankWars/T90/textures_mod/hullB",
                new Dictionary<string, string> { { "Treadmill1", "Effects/WrapTexture" }, { "Treadmill2", "Effects/WrapTexture" } },
                new Dictionary<string, string> { { "Treadmill1", "Models/TankWars/T90/textures_mod/treadmills" }, 
                    { "Treadmill2", "Models/TankWars/T90/textures_mod/treadmills" } },
                terrain
            )
        {
            CannonLength = 620f;
            CannonHeight = 150f;
            WheelsAmount = 16;
        }

        protected override void RotateTurretAndCannon(GameObject gameObject)
        {
            gameObject.Bones[gameObject.Model.Bones["Cannon"].Index] = Matrix.CreateRotationX(-CannonRotation) * CannonTransform * Matrix.CreateRotationY(TurretRotation);
            gameObject.Bones[gameObject.Model.Bones["Turret"].Index] = Matrix.CreateRotationZ(TurretRotation) * TurretTransform;
        }

        protected override void RotateWheels(GameObject gameObject)
        {

            var velocity = gameObject.Velocity.Length();

            for (var i = 0; i < WheelsAmount; i++) 
                gameObject.Bones[gameObject.Model.Bones["Wheel" + (i + 1)].Index] = Matrix.CreateRotationX(MathHelper.ToRadians(velocity)) * WheelsTransforms[i];

        }

        public override Vector3 GetCannonDirection(GameObject gameObject)
        {
            var turretMatrix = gameObject.Bones[gameObject.Model.Bones["Turret"].Index] * gameObject.GetRotationMatrix();
            var cannonMatrix = gameObject.Bones[gameObject.Model.Bones["Cannon"].Index] * gameObject.GetRotationMatrix();
            return -Vector3.Normalize(turretMatrix.Up + cannonMatrix.Up);
        }
    }
}