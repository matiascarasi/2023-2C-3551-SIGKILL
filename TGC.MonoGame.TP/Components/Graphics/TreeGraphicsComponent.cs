using System.Collections.Generic;

namespace TGC.MonoGame.TP.Components.Graphics
{
    // TODO: esta clase solo representa una configuración, revisar si se puede hacer de otra forma (archivo, etc.)
    class TreeGraphicsComponent : GraphicsComponent
    {
        public TreeGraphicsComponent() : base(
                "Models/TankWars/Miscellaneous/Trees/Chestnut_Good",
                "Effects/BasicTexture",
                "Models/TankWars/Miscellaneous/Trees/tileable_tree_bark_texture_by_ftourini-d3l69hz",
                new Dictionary<string, string> { { "tree", "Effects/BasicTexture" }, { "leaves", "Effects/Leaves" } },
                new Dictionary<string, string> { { "tree", "Models/TankWars/Miscellaneous/Trees/tileable_tree_bark_texture_by_ftourini-d3l69hz" },
                    { "leaves","Models/TankWars/Miscellaneous/Trees/TexturesCom_Branches0018_1_alphamasked_S" } }
            )
        { }
    }
}