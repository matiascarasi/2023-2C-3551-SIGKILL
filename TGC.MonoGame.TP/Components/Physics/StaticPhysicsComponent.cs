using BepuPhysics;
using BepuPhysics.Collidables;
using Microsoft.Xna.Framework;
using TGC.MonoGame.TP.Gizmos;
using TGC.MonoGame.TP.Physics;

namespace TGC.MonoGame.TP.Components.Physics
{
    public class StaticPhysicsComponent<TShape> : IPhysicsComponent where TShape : unmanaged, IConvexShape, IShape
    {
        private readonly PhysicsEngine PhysicsEngine;
        private readonly TShape Shape;
        public Vector3 Position { get => PhysicsEngine.GetStaticReference(_staticHandle).Pose.Position; }
        public Quaternion Orientation { get => PhysicsEngine.GetStaticReference(_staticHandle).Pose.Orientation; }
        private readonly StaticHandle _staticHandle;

        public StaticPhysicsComponent(PhysicsEngine physicsEngine, string shapeName, TShape shape, Vector3 initialPosition, Quaternion initialOrientation)
        {
            Shape = shape;
            PhysicsEngine = physicsEngine;
            PhysicsEngine.AddShape(shapeName, shape);
            _staticHandle = PhysicsEngine.AddStatic(shapeName, initialPosition.ToNumerics(), initialOrientation.ToNumerics());
        }

        public void Draw(ShapeDrawer shapeDrawer)
        {
            var reference = PhysicsEngine.GetStaticReference(_staticHandle);
            shapeDrawer.Draw(Shape, reference.Pose);
        }

        public void Update(GameTime gameTime)
        {
        }
    }
}
