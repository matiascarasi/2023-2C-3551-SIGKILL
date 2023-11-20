﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
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
      
        public float CannonRotation = 0f;
        public float TurretRotation = 0f;
        protected float CannonHeight;
        protected float CannonLength;

        private const float MIN_TURRET_ANGLE = -0.05f;
        private const float MAX_TURRET_ANGLE = 0.25f;

        public TankGraphicsComponent(
            string model, 
            string defaultEffect, 
            string defaultNormal,
            string defaultTexture, 
            Dictionary<string, string> effects, 
            Dictionary<string, string> normals,
            Dictionary<string, string> textures,
            string blinnPhongType)
            : 
            base(model, defaultEffect, defaultNormal, defaultTexture, effects, normals, textures, blinnPhongType)
        { }
        abstract public Vector3 GetCannonDirection(GameObject gameObject);
        public Vector3 GetCannonEnd(GameObject gameObject)
        {
            return gameObject.Position + Vector3.Up * CannonHeight + GetCannonDirection(gameObject) * CannonLength;
        }

        public float FixCannonAngle(float angle)
        {
            return MathF.Min(MathF.Max(angle, MIN_TURRET_ANGLE), MAX_TURRET_ANGLE);
        }


    }
}
