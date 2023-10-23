using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;

namespace TGC.MonoGame.TP.Components.Graphics
{
    class PanzerGraphicsComponent : GraphicsComponent
    {
        const float MAX_TURRET_ANGLE = -0.25f;
        private Matrix TurretTransform { get; set; }
        private Matrix CannonTransform { get; set; }
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

        public override void Update(GameObject gameObject, MouseCamera mouseCamera)
        {

            if (mouseCamera.UpDownRotation > MAX_TURRET_ANGLE) 
                gameObject.Model.Bones["Cannon"].Transform = Matrix.CreateRotationX(-mouseCamera.UpDownRotation) * CannonTransform;
            gameObject.Model.Bones["Turret"].Transform = Matrix.CreateRotationY(mouseCamera.LeftRightRotation) * TurretTransform;
        }
    }
}