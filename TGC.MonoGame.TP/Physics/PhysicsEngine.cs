using System;
using System.Collections.Generic;
using BepuPhysics;
using BepuPhysics.Collidables;
using BepuPhysics.Constraints;
using BepuUtilities.Memory;
using Microsoft.Xna.Framework;

namespace TGC.MonoGame.TP.Physics
{
    public class PhysicsEngine
    {
        public Simulation Simulation { get; protected set; }

        public BufferPool BufferPool { get; private set; }

        public SimpleThreadDispatcher ThreadDispatcher { get; private set; }

        private readonly Dictionary<string, IConvexShape> Shapes = new();
        private readonly Dictionary<string, TypedIndex> ShapeIndices = new();
        public void Initialize()
        {
            BufferPool = new BufferPool();
            var targetThreadCount = Math.Max(1,
                Environment.ProcessorCount > 4 ? Environment.ProcessorCount - 2 : Environment.ProcessorCount - 1);
            ThreadDispatcher = new SimpleThreadDispatcher(targetThreadCount);
            Simulation = Simulation.Create(BufferPool, new NarrowPhaseCallbacks(new SpringSettings(30, 1)),
                new PoseIntegratorCallbacks(new System.Numerics.Vector3(0, 0, 0)), new SolveDescription(8, 1));
        }

        public void Update(GameTime gameTime)
        {
            var elapsedTime = Convert.ToSingle(gameTime.ElapsedGameTime.TotalSeconds);
            if (elapsedTime > 0)
            {
                Simulation.Timestep(elapsedTime, ThreadDispatcher);
            }
        }

        public void AddShape<TShape>(string shapeName, TShape shape) where TShape : unmanaged, IConvexShape, IShape
        {
            var newShape = Shapes.TryAdd(shapeName, shape);
            if (newShape)
            {
                var index = Simulation.Shapes.Add(shape);
                ShapeIndices.Add(shapeName, index);
            }
        }

        public BodyHandle AddBody(BodyTypes type, string shapeName, System.Numerics.Vector3 initialPosition, System.Numerics.Quaternion initialOrientation)
        {
            var shapeIndex = ShapeIndices[shapeName];
            switch (type)
            {
                case BodyTypes.Dynamic:
                    return Simulation.Bodies.Add(BodyDescription.CreateDynamic(
                        new RigidPose(initialPosition, initialOrientation),
                        Shapes[shapeName].ComputeInertia(300f),
                        new CollidableDescription(shapeIndex, 0.1f),
                        new BodyActivityDescription(0.01f)));
                // case BodyTypes.Dynamic:
                //     return Simulation.Bodies.Add(BodyDescription.CreateKinematic(
                //         initialPosition,
                //         new BodyVelocity(),
                //         new CollidableDescription(shapeIndex, 0.1f),
                //         new BodyActivityDescription(0.01f)));
                default:
                    throw new ArgumentException("Unknown physics body type: " + type);
            }
        }

        public BodyReference GetBodyReference(BodyHandle handle)
        {
            return Simulation.Bodies.GetBodyReference(handle);
        }
    }
}