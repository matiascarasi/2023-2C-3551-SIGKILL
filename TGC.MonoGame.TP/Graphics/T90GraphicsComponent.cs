using System.Collections.Generic;
using Microsoft.Xna.Framework.Content;

namespace TGC.MonoGame.TP.Graphics
{
    // TODO: esta clase solo representa una configuración, revisar si se puede hacer de otra forma (archivo, etc.)
    class T90GraphicsComponent : GraphicsComponent
    {
        public T90GraphicsComponent(ContentManager contentManager) : base(contentManager,
                "Models/TankWars/T90/T90",
                "Effects/BasicTexture",
                "Models/TankWars/T90/textures_mod/hullC",
                new Dictionary<string, string> { { "Treadmill1", "Effects/WrapTexture" }, { "Treadmill2", "Effects/WrapTexture" } },
                new Dictionary<string, string> { { "Treadmill1", "Models/TankWars/T90/textures_mod/treadmills" }, 
                    { "Treadmill2", "Models/TankWars/T90/textures_mod/treadmills" } }
            )
        { }
    }
}