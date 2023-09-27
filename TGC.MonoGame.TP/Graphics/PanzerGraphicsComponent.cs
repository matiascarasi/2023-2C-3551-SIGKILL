using System.Collections.Generic;
using Microsoft.Xna.Framework.Content;

namespace TGC.MonoGame.TP.Graphics
{
    // TODO: esta clase solo representa una configuración, revisar si se puede hacer de otra forma (archivo, etc.)
    class PanzerGraphicsComponent : GraphicsComponent
    {
        public PanzerGraphicsComponent(ContentManager contentManager) : base(contentManager,
                "Models/TankWars/Panzer/Panzer",
                "Effects/BasicTexture",
                "Models/TankWars/Panzer/PzVl_Tiger_I_0",
                new Dictionary<string, string> { { "Treadmill1", "Effects/WrapTexture" }, { "Treadmill2", "Effects/WrapTexture" } },
                new Dictionary<string, string> { { "Treadmill1", "Models/TankWars/Panzer/PzVI_Tiger_I_track_0" }, { "Treadmill2", "Models/TankWars/Panzer/PzVI_Tiger_I_track_0" } }
            )
        { }
    }
}