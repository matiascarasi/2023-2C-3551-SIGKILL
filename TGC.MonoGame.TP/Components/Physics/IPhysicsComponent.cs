using Microsoft.Xna.Framework;
using TGC.MonoGame.TP.Gizmos;

namespace TGC.MonoGame.TP.Components.Physics
{
    public interface IPhysicsComponent
    {
        public Vector3 Position { get; }
        public Quaternion Orientation { get; }
        public void Draw(ShapeDrawer shapeDrawer);
    }
}
