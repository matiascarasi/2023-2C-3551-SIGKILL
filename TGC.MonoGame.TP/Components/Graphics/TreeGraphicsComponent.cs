using System.Collections.Generic;

namespace TGC.MonoGame.TP.Components.Graphics
{
    // TODO: esta clase solo representa una configuración, revisar si se puede hacer de otra forma (archivo, etc.)
    class TreeGraphicsComponent : GraphicsComponent
    {
        public TreeGraphicsComponent() : base(
                "Models/TankWars/Miscellaneous/Trees/Chestnut_Good",
                "Effects/Leaves",
                "",
                "Models/TankWars/Miscellaneous/Trees/bark-texture",
                new Dictionary<string, string> { { "tree", "Effects/Leaves" }, { "leaves", "Effects/Leaves" } },
                new Dictionary<string, string> { { "tree", "Models/TankWars/Miscellaneous/Trees/bark-texture" },
                    { "leaves","Models/TankWars/Miscellaneous/Trees/TexturesCom_Branches0018_1_alphamasked_S" } },
                new Dictionary<string, string> { { "tree", "Models/TankWars/Miscellaneous/Trees/bark-texture" },
                    { "leaves","Models/TankWars/Miscellaneous/Trees/TexturesCom_Branches0018_1_alphamasked_S" } },  
                "Default"
            )
        { }
    }
}