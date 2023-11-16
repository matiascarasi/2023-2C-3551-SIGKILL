namespace TGC.MonoGame.TP.Components.Graphics
{
    // TODO: esta clase solo representa una configuración, revisar si se puede hacer de otra forma (archivo, etc.)
    class TreeGraphicsComponent : GraphicsComponent
    {
        public TreeGraphicsComponent() : base(
                "Models/TankWars/Miscellaneous/Dumpster/dumpster_fbx",
                "Effects/BasicTexture",
                "Models/TankWars/Miscellaneous/Dumpster/dumpsters_Dumpster_BaseColor"
            )
        { }
    }
}