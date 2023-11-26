using BepuPhysics;
using BepuPhysics.Collidables;
using Microsoft.Xna.Framework;
using TGC.MonoGame.TP.Gizmos;
using TGC.MonoGame.TP.Physics;

namespace TGC.MonoGame.TP.Components.Physics
{
    public class DynamicPhysicsComponent<TShape> : IDynamicPhysicsComponent where TShape : unmanaged, IConvexShape, IShape
    {
        private readonly PhysicsEngine PhysicsEngine;
        private readonly TShape Shape;
        public Vector3 Position { get => PhysicsEngine.GetBodyReference(BodyHandle).Pose.Position; }
        public Quaternion Orientation { get => PhysicsEngine.GetBodyReference(BodyHandle).Pose.Orientation; }
        private readonly BodyHandle BodyHandle;

        public DynamicPhysicsComponent(PhysicsEngine physicsEngine, string shapeName, TShape shape, Vector3 initialPosition, Quaternion initialOrientation)
        {
            Shape = shape;
            PhysicsEngine = physicsEngine;
            PhysicsEngine.AddShape(shapeName, shape);
            BodyHandle = PhysicsEngine.AddBody(shapeName, initialPosition.ToNumerics(), initialOrientation.ToNumerics());
        }

        public void Draw(ShapeDrawer shapeDrawer)
        {
            var bodyReference = PhysicsEngine.GetBodyReference(BodyHandle);
            shapeDrawer.Draw(Shape, bodyReference.Pose);
        }

        public void Accelerate()
        {
            var bodyReference = PhysicsEngine.GetBodyReference(BodyHandle);
            var poseOrientation = bodyReference.Pose.Orientation;
            var forward = System.Numerics.Vector3.Transform(-System.Numerics.Vector3.UnitZ, poseOrientation);
            var impulse = forward * 1000f;
            bodyReference.Awake = true;
            bodyReference.ApplyLinearImpulse(impulse);
        }

        public void Decelerate()
        {
            var bodyReference = PhysicsEngine.GetBodyReference(BodyHandle);
            var poseOrientation = bodyReference.Pose.Orientation;
            var forward = System.Numerics.Vector3.Transform(-System.Numerics.Vector3.UnitZ, poseOrientation);
            var impulse = forward * -1000f;
            bodyReference.Awake = true;
            bodyReference.ApplyLinearImpulse(impulse);
        }

        public void TurnLeft(float deltaTime)
        {
            var bodyReference = PhysicsEngine.GetBodyReference(BodyHandle);
            var poseOrientation = bodyReference.Pose.Orientation;
            var left = System.Numerics.Vector3.Transform(System.Numerics.Vector3.UnitY, poseOrientation);
            var impulse = left * 50000f;
            bodyReference.Awake = true;
            bodyReference.ApplyAngularImpulse(impulse);
        }

        public void TurnRight(float deltaTime)
        {
            var bodyReference = PhysicsEngine.GetBodyReference(BodyHandle);
            var poseOrientation = bodyReference.Pose.Orientation;
            var right = System.Numerics.Vector3.Transform(-System.Numerics.Vector3.UnitY, poseOrientation);
            var impulse = right * 50000f;
            bodyReference.Awake = true;
            bodyReference.ApplyAngularImpulse(impulse);
        }
    }
}
