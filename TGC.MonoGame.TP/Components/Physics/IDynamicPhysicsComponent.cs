namespace TGC.MonoGame.TP.Components.Physics
{
    public interface IDynamicPhysicsComponent : IPhysicsComponent
    {
        public void Accelerate();
        public void Decelerate();
        public void TurnLeft(float deltaTime);
        public void TurnRight(float deltaTime);
    }
}
