using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TGC.MonoGame.TP.Defaults
{
    abstract class PlayerDefaults
    {
        static public readonly Vector3 Position = Vector3.Zero;
        public const float YAxisRotation = 90f;
        public const float Scale = 1f;
        public const string TankName = "T90";
        public const float Health = 50f;
        public const float Damage = 100f;
        public const float DriveSpeed = 100f;
        public const float RotationSpeed = 20f;
    }
}
