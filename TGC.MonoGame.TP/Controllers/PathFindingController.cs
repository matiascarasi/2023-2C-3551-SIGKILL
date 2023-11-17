using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TGC.MonoGame.TP.Actors;
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
        private GameObject Target { get; }
        private List<GameObject> Objects { get; }
        private MovementController MovementController { get; }
        public PathFindingController(GameObject target, float minDistance, List<GameObject> objects, MovementController movementController)
        {
            Target = target;
            _minDistance = minDistance;
            Objects = objects;
            MovementController = movementController;
            Direction = Vector3.Zero;
        }

        public void Update(GameObject gameObject, GameTime gameTime)
        {

            if (Target == null) return;

            Direction = Target.Position - gameObject.Position;
            var deltaTime = Convert.ToSingle(gameTime.ElapsedGameTime.TotalSeconds);

            _rotationAngle = AlgebraHelper.GetAngleBetweenTwoVectors(gameObject.World.Forward, Direction);
            
            var distance = Direction.Length();
            if (distance < _minDistance)
            {
                MovementController.Stop();
                return;
            }
            
            if (_rotationAngle > 0f)
            {
                MovementController.TurnLeft(gameObject, deltaTime);
            }
            else
            {
                MovementController.TurnRight(gameObject, deltaTime);
            }

            if (gameTime.TotalGameTime.TotalSeconds < _lastPrediction + PREDICTION_DELAY) return;
            _lastPrediction = gameTime.TotalGameTime.TotalSeconds;

            if (PredictCollision(gameObject))
            {
                MovementController.Stop();
                return;
            }

            MovementController.Accelerate();

        }

        private bool PredictCollision(GameObject gameObject)
        {
            var obb = gameObject.CollisionComponent.DisplacedOBB(gameObject.Position + gameObject.World.Forward * PREDICTION_DISTANCE);
            foreach (var obj in Objects)
            {
                if (obj.CollisionComponent.CollidesWith(obb) && obj != gameObject) return true;
            }
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
