using Microsoft.Xna.Framework;

namespace TGC.MonoGame.TP.Defaults
{
    abstract class PlayerDefaults
    {
        static public readonly Vector3 Position = Vector3.Zero;
        public const float YAxisRotation = 0f;
        public const float Scale = 1f;
        public const string TankName = "Panzer";
        public const float Health = 50f;
        public const float Damage = 100f;
        public const float DriveSpeed = 300f;
        public const float RotationSpeed = 20f;
    }
}
