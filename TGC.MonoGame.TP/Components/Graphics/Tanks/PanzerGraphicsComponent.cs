using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using TGC.MonoGame.TP.Actors;

namespace TGC.MonoGame.TP.Components.Graphics
{
    class PanzerGraphicsComponent : TankGraphicsComponent
    {
        const float MAX_TURRET_ANGLE = -0.25f;
        public PanzerGraphicsComponent() : base(
                "Models/TankWars/Panzer/Panzer",
                "Effects/BasicTexture",
                "Models/TankWars/Panzer/PzVl_Tiger_I_0",
                new Dictionary<string, string> { { "Treadmill1", "Effects/WrapTexture" }, { "Treadmill2", "Effects/WrapTexture" } },
                new Dictionary<string, string> { { "Treadmill1", "Models/TankWars/Panzer/PzVI_Tiger_I_track_0" }, { "Treadmill2", "Models/TankWars/Panzer/PzVI_Tiger_I_track_0" } }
            )
        { }

        public override void LoadContent(GameObject gameObject, ContentManager Content)
        {
            base.LoadContent(gameObject, Content);
            CannonTransform = gameObject.Model.Bones["Cannon"].Transform;
            TurretTransform = gameObject.Model.Bones["Turret"].Transform;
        }

        public override void Update(GameObject gameObject, GameTime gameTime)
        {
            if (CannonRotation > MAX_TURRET_ANGLE) 
                gameObject.Model.Bones["Cannon"].Transform = Matrix.CreateRotationX(-CannonRotation) * CannonTransform;
            gameObject.Model.Bones["Turret"].Transform = Matrix.CreateRotationY(TurretRotation) * TurretTransform;
        }
    }
}