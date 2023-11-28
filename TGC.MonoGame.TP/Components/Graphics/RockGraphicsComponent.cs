namespace TGC.MonoGame.TP.Components.Graphics
{
    // TODO: esta clase solo representa una configuración, revisar si se puede hacer de otra forma (archivo, etc.)
    class RockGraphicsComponent : GraphicsComponent
    {
        public RockGraphicsComponent() : base(
                "Models/TankWars/Miscellaneous/Rock/Rock07-Base",
                "Effects/BlinnPhong",
                "Models/TankWars/Miscellaneous/Rock/Rock07-Base-Diffuse_0"
            )
        { }
    }
}