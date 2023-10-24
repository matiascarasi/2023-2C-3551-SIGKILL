namespace TGC.MonoGame.TP.Components.Graphics
{
    // TODO: esta clase solo representa una configuración, revisar si se puede hacer de otra forma (archivo, etc.)
    class FenceGraphicsComponent : GraphicsComponent
    {
        public FenceGraphicsComponent() : base(
                "Models/TankWars/Miscellaneous/Fence/chainLinkFence_low",
                "Effects/BasicTexture",
                "Models/TankWars/Miscellaneous/Fence/chainLinkFence_low_mat_BaseColor"
            )
        { }
    }
}