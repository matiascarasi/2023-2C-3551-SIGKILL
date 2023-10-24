﻿using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;

namespace TGC.MonoGame.TP.Components.Graphics
{
    class T90GraphicsComponent : GraphicsComponent
    {
        const float MAX_TURRET_ANGLE = -0.25f;
        private Matrix TurretTransform { get; set; }
        private Matrix CannonTransform { get; set; }
        public T90GraphicsComponent() : base(
                "Models/TankWars/T90/T90",
                "Effects/BasicTexture",
                "Models/TankWars/T90/textures_mod/hullC",
                new Dictionary<string, string> { { "Treadmill1", "Effects/WrapTexture" }, { "Treadmill2", "Effects/WrapTexture" } },
                new Dictionary<string, string> { { "Treadmill1", "Models/TankWars/T90/textures_mod/treadmills" }, 
                    { "Treadmill2", "Models/TankWars/T90/textures_mod/treadmills" } }
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