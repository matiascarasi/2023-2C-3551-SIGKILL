using Microsoft.Xna.Framework;
using TGC.MonoGame.TP.Content.Actors;

namespace TGC.MonoGame.TP.Controllers
{
    class MovementController
    {
        public float Acceleration;
        public float Speed;
        public Vector3 Velocity;
        public float Dampening = 0.5f;
        public float DriveSpeed { get; set; }
        public float RotationSpeed { get; set; }

        public MovementController(float driveSpeed, float rotationSpeed)
        {
            DriveSpeed = driveSpeed;
            RotationSpeed = rotationSpeed;
            Acceleration = 0f;
            Speed = 0f;
            Velocity = Vector3.Zero;
        }

        public void Accelerate()
        {
            Acceleration = DriveSpeed;
        }

        public void Decelerate()
        {
            Acceleration = -DriveSpeed;
        }

        public void TurnRight(GameObject gameObject, float deltaTime)
        {
            gameObject.YAxisRotation -= RotationSpeed * deltaTime;
        }

        public void TurnLeft(GameObject gameObject, float deltaTime)
        {
            gameObject.YAxisRotation += RotationSpeed * deltaTime;
        }

        public void Settle()
        {
            Acceleration = -Speed * Dampening;
        }

        public void Move(GameObject gameObject, float deltaTime)
        {
            Speed += Acceleration * deltaTime;
            Velocity = gameObject.World.Forward * Speed;
            gameObject.Position += Velocity * deltaTime;
        }

    }
}
