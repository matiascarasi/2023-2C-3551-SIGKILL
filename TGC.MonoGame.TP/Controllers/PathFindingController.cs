using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using TGC.MonoGame.TP.Actors;
using TGC.MonoGame.TP.Components.Physics;
using TGC.MonoGame.TP.Helpers;

namespace TGC.MonoGame.TP.Controllers
{
    class PathFindingController
    {
        private const float PREDICTION_DISTANCE = 750f;
        private const double PREDICTION_DELAY = 1;
        private double _lastPrediction = -PREDICTION_DELAY;
        private float _rotationAngle = 0f;
        private readonly float _minDistance;
        private Vector3 Direction { get; set; }
        private LinkedList<GameObject> Targets { get; }
        private LinkedListNode<GameObject> Target { get; set; }
        private List<GameObject> Objects { get; }
        private IDynamicPhysicsComponent PhysicsComponent { get; }
        public PathFindingController(List<GameObject> targets, GameObject initialTarget, float minDistance, List<GameObject> objects, IDynamicPhysicsComponent physicsComponent)
        {
            Targets = new LinkedList<GameObject>(targets);
            Target = Targets.Find(initialTarget);
            _minDistance = minDistance;
            Objects = objects;
            Direction = Vector3.Zero;
            PhysicsComponent = physicsComponent;
        }
        public void Update(GameObject gameObject, GameTime gameTime)
        {
            if (Target.Value == null)
            {
                Target = Target.Next;
                return;
            }

            var target = Target.Value;

            Direction = target.Position - gameObject.Position;
            var deltaTime = Convert.ToSingle(gameTime.ElapsedGameTime.TotalSeconds);

            _rotationAngle = AlgebraHelper.GetAngleBetweenTwoVectors(gameObject.World.Forward, Direction);
            
            var distance = Direction.Length();
            if (distance < _minDistance)
            {
                return;
            }
            
            if (_rotationAngle > 0f)
            {
                PhysicsComponent.TurnLeft(deltaTime);
            }
            else
            {
                PhysicsComponent.TurnRight(deltaTime);
            }

            if (gameTime.TotalGameTime.TotalSeconds < _lastPrediction + PREDICTION_DELAY) return;
            _lastPrediction = gameTime.TotalGameTime.TotalSeconds;

            if (PredictCollision(gameObject))
            {
                return;
            }

            PhysicsComponent.Accelerate();

        }

        private bool PredictCollision(GameObject gameObject)
        {
            // var obb = gameObject.PhysicsComponent.DisplacedOBB(gameObject.Position + gameObject.World.Forward * PREDICTION_DISTANCE);
            // foreach (var obj in Objects)
            // {
            //     if (obj.PhysicsComponent.CollidesWith(obb) && obj != gameObject) return true;
            // }
            return false;
        }

        public float GetRotationAngle()
        {
            return _rotationAngle;
        }

        public Vector3 GetDirection()
        {
            return Direction;
        }

    }
}
