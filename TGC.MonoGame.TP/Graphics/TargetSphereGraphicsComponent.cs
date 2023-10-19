using Microsoft.Xna.Framework.Content;

namespace TGC.MonoGame.TP.Graphics
{
    // TODO: esta clase solo representa una configuración, revisar si se puede hacer de otra forma (archivo, etc.)
    class TargetSphereGraphicsComponent : GraphicsComponent
    {
        public TargetSphereGraphicsComponent(ContentManager contentManager) : base(contentManager,
                "3D/geometries/sphere",
                "Effects/BasicTexture",
                "Models/TankWars/Miscellaneous/Rock/Rock07-Base-Diffuse_0"
            )
        { }
    }
}