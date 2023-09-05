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
        public Vector3 Acceleration;
        public Vector3 Velocity;
        public float Dampening = 0.01f;
        public float DriveSpeed { get; set; }
        public float RotationSpeed { get; set; }

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

        public void RotateRight(GameObject gameObject, float deltaTime)
        {
            gameObject.YAxisRotation -= RotationSpeed * deltaTime;
        }

        public void RotateLeft(GameObject gameObject, float deltaTime)
        {
            gameObject.YAxisRotation += RotationSpeed * deltaTime;
        }

        public void Settle(GameObject gameObject)
        {
            Acceleration = gameObject.World.Forward * - Velocity * Dampening;
        }

        public void Move(GameObject gameObject, float deltaTime)
        {
            Velocity += Acceleration * deltaTime;
            gameObject.Position += Velocity * deltaTime;
        }

    }
}
