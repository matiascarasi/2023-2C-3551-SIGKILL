using Microsoft.Xna.Framework;
using TGC.MonoGame.TP.Gizmos;

namespace TGC.MonoGame.TP.Components.Physics
{
    public interface IPhysicsComponent
    {
        public Vector3 Position { get; }
        public Quaternion Orientation { get; }

        public void Accelerate();
        public void Decelerate();
        public void TurnLeft(float deltaTime);
        public void TurnRight(float deltaTime);
        public void Draw(ShapeDrawer shapeDrawer);
    }
}
