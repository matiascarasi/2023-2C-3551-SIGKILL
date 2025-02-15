﻿using Microsoft.Xna.Framework;
using TGC.MonoGame.TP.Actors;
using System;

namespace TGC.MonoGame.TP.Controllers
{
    class MovementController
    {
        public float Acceleration;
        public float Speed;
        public float Dampening = 0.5f;
        public float DriveSpeed { get; set; }
        public float RotationSpeed { get; set; }

        public MovementController(float driveSpeed, float rotationSpeed)
        {
            DriveSpeed = driveSpeed;
            RotationSpeed = rotationSpeed;
            Acceleration = 0f;
            Speed = 0f;
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
            gameObject.RotationAngle -= RotationSpeed * deltaTime;
        }

        public void TurnLeft(GameObject gameObject, float deltaTime)
        {
            gameObject.RotationAngle += RotationSpeed * deltaTime;
        }

        public void Settle()
        {
            Acceleration = -Speed * Dampening;
        }

        public void Stop()
        {
            Speed = 0f;
            Acceleration = 0f;
        }

        public void Update(GameObject gameObject, GameTime gameTime)
        {
            var deltaTime = Convert.ToSingle(gameTime.ElapsedGameTime.TotalSeconds);
            Speed += Acceleration * deltaTime;
            gameObject.Velocity = gameObject.World.Forward * Speed;
        }

    }
}
