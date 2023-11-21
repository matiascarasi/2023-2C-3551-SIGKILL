namespace TGC.MonoGame.TP.Components.Graphics
{
    // TODO: esta clase solo representa una configuración, revisar si se puede hacer de otra forma (archivo, etc.)
    class BulletGraphicsComponent : GraphicsComponent
    {
        public BulletGraphicsComponent() : base(
                "Models/TankWars/Miscellaneous/Bullet/bullet",
                "Effects/BlinnPhong",
                "Models/TankWars/Miscellaneous/Bullet/main_texture"
            )
        { }
    }
}