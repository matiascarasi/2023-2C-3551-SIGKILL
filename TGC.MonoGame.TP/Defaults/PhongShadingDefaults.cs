using Microsoft.Xna.Framework;

namespace TGC.MonoGame.TP.Defaults
{
    abstract class PhongShadingDefaults
    {
        static public Vector3 ambientColor = new Vector3(.735f, .735f, .735f);
        static public Vector3 diffuseColor = new Vector3(1f, 1f, 1f);
        static public Vector3 specularColor = new Vector3(1f, 1f, 1f);
        public const float KA = 0.9f;
        public const float KD = 0.2f;
        public const float KS = 0.1f;
        public const float Shininess = 1f;
    }
}
