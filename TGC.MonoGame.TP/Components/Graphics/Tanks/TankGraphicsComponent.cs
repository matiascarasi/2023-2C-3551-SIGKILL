using Microsoft.Xna.Framework;
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

        public TankGraphicsComponent(string model, string defaultEffect, string defaultTexture, Dictionary<string, string> effects, Dictionary<string, string> textures) : base(model, defaultEffect, defaultTexture, effects, textures)
        { }

    }
}
