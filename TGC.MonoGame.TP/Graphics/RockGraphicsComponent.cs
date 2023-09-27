using Microsoft.Xna.Framework.Content;

namespace TGC.MonoGame.TP.Graphics
{
    // TODO: esta clase solo representa una configuración, revisar si se puede hacer de otra forma (archivo, etc.)
    class RockGraphicsComponent : GraphicsComponent
    {
        public RockGraphicsComponent(ContentManager contentManager) : base(contentManager,
                "Models/TankWars/Miscellaneous/Rock/Rock07-Base",
                "Effects/BasicTexture",
                "Models/TankWars/Miscellaneous/Rock/Rock07-Base-Diffuse_0"
            )
        { }
    }
}