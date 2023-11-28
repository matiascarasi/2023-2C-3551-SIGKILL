using System;
using System.Collections.Generic;
using BepuPhysics;
using BepuPhysics.Collidables;
using BepuPhysics.CollisionDetection;
using BepuPhysics.Constraints;
using BepuUtilities.Memory;
using Microsoft.Xna.Framework;
using TGC.MonoGame.TP.Components.Physics;

namespace TGC.MonoGame.TP.Physics
{
    public class PhysicsEngine
    {
        private Simulation _simulation;

        private BufferPool _bufferPool;

        private SimpleThreadDispatcher _threadDispatcher;

        private readonly Dictionary<string, IConvexShape> _shapes = new();
        private readonly Dictionary<string, TypedIndex> _shapeIndices = new();
        private readonly Dictionary<StaticHandle, IPhysicsComponent> _staticObjects = new();
        private readonly Dictionary<BodyHandle, IPhysicsComponent> _dynamicObjects = new();
        public void Initialize()
        {
            _bufferPool = new BufferPool();
            var targetThreadCount = Math.Max(1,
                Environment.ProcessorCount > 4 ? Environment.ProcessorCount - 2 : Environment.ProcessorCount - 1);
            _threadDispatcher = new SimpleThreadDispatcher(targetThreadCount);
            _simulation = Simulation.Create(
                _bufferPool,
                new NarrowPhaseCallbacks(
                    new SpringSettings(30, 1),
                    CustomCallback
                ),
                new PoseIntegratorCallbacks(new System.Numerics.Vector3(0, 0, 0), 0.1f, 0.1f), new SolveDescription(8, 1));
        }

        public void Update(GameTime gameTime)
        {
            var elapsedTime = Convert.ToSingle(gameTime.ElapsedGameTime.TotalSeconds);
            if (elapsedTime > 0)
            {
                _simulation.Timestep(elapsedTime, _threadDispatcher);
            }
        }

        public void AddShape<TShape>(string shapeName, TShape shape) where TShape : unmanaged, IConvexShape, IShape
        {
            var newShape = _shapes.TryAdd(shapeName, shape);
            if (newShape)
            {
                var index = _simulation.Shapes.Add(shape);
                _shapeIndices.Add(shapeName, index);
            }
        }

        public BodyHandle AddBody(IPhysicsComponent physicsComponent, string shapeName, System.Numerics.Vector3 initialPosition, System.Numerics.Quaternion initialOrientation)
        {
            var shapeIndex = _shapeIndices[shapeName];
            var handle = _simulation.Bodies.Add(BodyDescription.CreateDynamic(
                new RigidPose(initialPosition, initialOrientation),
                _shapes[shapeName].ComputeInertia(200f),
                new CollidableDescription(shapeIndex, 0.1f),
                new BodyActivityDescription(0.01f)));
            _dynamicObjects.Add(handle, physicsComponent);
            return handle;
        }

        public StaticHandle AddStatic(IPhysicsComponent physicsComponent, string shapeName, System.Numerics.Vector3 initialPosition, System.Numerics.Quaternion initialOrientation)
        {
            var shapeIndex = _shapeIndices[shapeName];
            var handle = _simulation.Statics.Add(
                new StaticDescription(new RigidPose(initialPosition, initialOrientation),
                shapeIndex)
            );
            _staticObjects.Add(handle, physicsComponent);
            return handle;
        }

        public BodyReference GetBodyReference(BodyHandle handle)
        {
            return _simulation.Bodies.GetBodyReference(handle);
        }

        public StaticReference GetStaticReference(StaticHandle handle)
        {
            return _simulation.Statics.GetStaticReference(handle);
        }

        private void CustomCallback(CollidablePair pair, Object manifold)
        {
            var convexManifold = (ConvexContactManifold)manifold;
            switch (pair.A.Mobility)
            {
                case CollidableMobility.Dynamic:
                    _dynamicObjects[pair.A.BodyHandle].CollisionCallback(1, convexManifold);
                    break;
                case CollidableMobility.Static:
                    _staticObjects[pair.A.StaticHandle].CollisionCallback(1, convexManifold);
                    break;
                default:
                    throw new Exception("Unhandled mobility type: " + pair.A.Mobility.ToString());
            }
            switch (pair.B.Mobility)
            {
                case CollidableMobility.Dynamic:
                    _dynamicObjects[pair.B.BodyHandle].CollisionCallback(2, convexManifold);
                    break;
                case CollidableMobility.Static:
                    _staticObjects[pair.B.StaticHandle].CollisionCallback(2, convexManifold);
                    break;
                default:
                    throw new Exception("Unhandled mobility type: " + pair.B.Mobility.ToString());
            }
        }
    }
}