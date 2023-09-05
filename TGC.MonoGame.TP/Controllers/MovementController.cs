using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TGC.MonoGame.TP.Content.Actors;

namespace TGC.MonoGame.TP.Controllers
{
    class MovementController
    {
        private Vector3 Acceleration;
        private Vector3 Velocity;
        private const float Damping = 2f;
        private float DriveSpeed { get; set; }
        private float RotationSpeed { get; set; }

        public MovementController(float driveSpeed, float rotationSpeed)
        {
            DriveSpeed = driveSpeed;
            RotationSpeed = rotationSpeed;
            Acceleration = Vector3.Zero;
            Velocity = Vector3.Zero;
        }

        public void Accelerate(GameObject gameObject)
        {
            Acceleration = gameObject.World.Forward * DriveSpeed;
        }

        public void Decelerate(GameObject gameObject)
        {
            Acceleration = gameObject.World.Backward * DriveSpeed;
        }

        public void TurnRight(GameObject gameObject, float deltaTime)
        {
            gameObject.YAxisRotation -= RotationSpeed * deltaTime;
            Acceleration -= Velocity * Damping;
        }

        public void TurnLeft(GameObject gameObject, float deltaTime)
        {
            gameObject.YAxisRotation += RotationSpeed * deltaTime;
            Acceleration -= Velocity * Damping;
        }

        public void Settle(GameObject gameObject)
        {
            Acceleration = gameObject.World.Forward - Velocity * Damping;
        }

        public void Move(GameObject gameObject, float deltaTime)
        {
            Velocity += Acceleration * deltaTime;
            gameObject.Position += Velocity * deltaTime;
        }

    }
}
