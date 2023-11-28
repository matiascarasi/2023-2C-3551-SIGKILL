using System;
using BepuPhysics;
using BepuPhysics.Collidables;
using BepuPhysics.CollisionDetection;
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
        public Action<int, ConvexContactManifold> CustomCollisionCallback { get; set; }
        private readonly BodyHandle BodyHandle;
        private readonly HeightMap _heightMap;

        public DynamicPhysicsComponent(PhysicsEngine physicsEngine, string shapeName, TShape shape, Vector3 initialPosition, Quaternion initialOrientation, HeightMap heightmap)
        {
            Shape = shape;
            PhysicsEngine = physicsEngine;
            PhysicsEngine.AddShape(shapeName, shape);
            BodyHandle = PhysicsEngine.AddBody(this, shapeName, initialPosition.ToNumerics(), initialOrientation.ToNumerics());
            _heightMap = heightmap;
            PhysicsEngine.GetBodyReference(BodyHandle).LocalInertia.InverseInertiaTensor.XX = 0f;
            PhysicsEngine.GetBodyReference(BodyHandle).LocalInertia.InverseInertiaTensor.ZX = 0f;
            PhysicsEngine.GetBodyReference(BodyHandle).LocalInertia.InverseInertiaTensor.ZY = 0f;
            PhysicsEngine.GetBodyReference(BodyHandle).LocalInertia.InverseInertiaTensor.ZZ = 0f;
            CustomCollisionCallback = (collidableNumber, manifold) => {};
        }

        public void Update(GameTime gameTime)
        {
            var bodyReference = PhysicsEngine.GetBodyReference(BodyHandle);
            var bodyPosition = bodyReference.Pose.Position;
            bodyReference.Pose.Position.Y = _heightMap.Height(bodyPosition.X, bodyPosition.Z);
        }

        public void Draw(ShapeDrawer shapeDrawer)
        {
            var bodyReference = PhysicsEngine.GetBodyReference(BodyHandle);
            shapeDrawer.Draw(Shape, bodyReference.Pose);
        }

        public void Accelerate(float deltaTime)
        {
            var bodyReference = PhysicsEngine.GetBodyReference(BodyHandle);
            var poseOrientation = bodyReference.Pose.Orientation;
            var forward = System.Numerics.Vector3.Transform(-System.Numerics.Vector3.UnitZ, poseOrientation);
            var impulse = forward * 50000f * deltaTime;
            bodyReference.Awake = true;
            bodyReference.ApplyLinearImpulse(impulse);
        }

        public void Decelerate(float deltaTime)
        {
            var bodyReference = PhysicsEngine.GetBodyReference(BodyHandle);
            var poseOrientation = bodyReference.Pose.Orientation;
            var forward = System.Numerics.Vector3.Transform(-System.Numerics.Vector3.UnitZ, poseOrientation);
            var impulse = forward * -50000f * deltaTime;
            bodyReference.Awake = true;
            bodyReference.ApplyLinearImpulse(impulse);
        }

        public void TurnLeft(float deltaTime)
        {
            var bodyReference = PhysicsEngine.GetBodyReference(BodyHandle);
            var poseOrientation = bodyReference.Pose.Orientation;
            var left = System.Numerics.Vector3.Transform(System.Numerics.Vector3.UnitY, poseOrientation);
            var impulse = left * 1500000f * deltaTime;
            bodyReference.Awake = true;
            bodyReference.ApplyAngularImpulse(impulse);
        }

        public void TurnRight(float deltaTime)
        {
            var bodyReference = PhysicsEngine.GetBodyReference(BodyHandle);
            var poseOrientation = bodyReference.Pose.Orientation;
            var right = System.Numerics.Vector3.Transform(-System.Numerics.Vector3.UnitY, poseOrientation);
            var impulse = right * 1500000f * deltaTime;
            bodyReference.Awake = true;
            bodyReference.ApplyAngularImpulse(impulse);
        }

        public void CollisionCallback(int collidableNumber, ConvexContactManifold manifold)
        {
            CustomCollisionCallback(collidableNumber, manifold);
        }
    }
}
