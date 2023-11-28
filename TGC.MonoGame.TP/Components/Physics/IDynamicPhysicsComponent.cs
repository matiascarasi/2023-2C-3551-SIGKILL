namespace TGC.MonoGame.TP.Components.Physics
{
    public interface IDynamicPhysicsComponent : IPhysicsComponent
    {
        public void Accelerate(float deltaTime);
        public void Decelerate(float deltaTime);
        public void TurnLeft(float deltaTime);
        public void TurnRight(float deltaTime);
    }
}
