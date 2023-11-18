using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TGC.MonoGame.TP.Actors;

namespace TGC.MonoGame.TP.Components.Graphics
{
    abstract class TankGraphicsComponent : GraphicsComponent
    {
        protected Matrix TurretTransform { get; set; }
        protected Matrix CannonTransform { get; set; }
        protected Matrix[] WheelsTransforms { get; set; }
        protected Terrain Terrain { get; set; }

        public float CannonRotation = 0f;
        public float TurretRotation = 0f;
        protected float CannonHeight;
        protected float CannonLength;
        protected int WheelsAmount;

        private const float MIN_TURRET_ANGLE = -0.05f;
        private const float MAX_TURRET_ANGLE = 0.25f;

        public TankGraphicsComponent(string model, string defaultEffect, string defaultTexture, Dictionary<string, string> effects, Dictionary<string, string> textures, Terrain terrain) : base(model, defaultEffect, defaultTexture, effects, textures)
        {
            Terrain = terrain;
        }
        abstract public Vector3 GetCannonDirection(GameObject gameObject);
        abstract protected void RotateTurretAndCannon(GameObject gameObject);
        abstract protected void RotateWheels(GameObject gameObject);
        protected override void SetCustomEffectParameters(Effect effect, GameObject gameObject, string meshName)
        {
            if (meshName.StartsWith("Treadmill")) {
                effect.Parameters["Velocity"].SetValue(gameObject.Velocity.Length());
            }
        }
        public Vector3 GetCannonEnd(GameObject gameObject)
        {
            return gameObject.Position + Vector3.Up * CannonHeight + GetCannonDirection(gameObject) * CannonLength;
        }

        public static float FixCannonAngle(float angle)
        {
            return MathF.Min(MathF.Max(angle, MIN_TURRET_ANGLE), MAX_TURRET_ANGLE);
        }
        public override void LoadContent(GameObject gameObject, ContentManager Content)
        {
            base.LoadContent(gameObject, Content);
            
            CannonTransform = gameObject.Bones[gameObject.Model.Bones["Cannon"].Index];
            TurretTransform = gameObject.Bones[gameObject.Model.Bones["Turret"].Index];
            
            WheelsTransforms = new Matrix[WheelsAmount];
            for (var i = 0; i < WheelsAmount; i++) WheelsTransforms[i] = gameObject.Bones[gameObject.Model.Bones["Wheel" + (i+1)].Index];
        }

        public override void Update(GameObject gameObject, GameTime gameTime)
        {
            RotateTurretAndCannon(gameObject);
            RotateWheels(gameObject);
        }
    }
}
