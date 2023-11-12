﻿using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using TGC.MonoGame.TP.Collisions;
using TGC.MonoGame.TP.Components;
using TGC.MonoGame.TP.Components.Collisions;
using TGC.MonoGame.TP.Components.Graphics;
using TGC.MonoGame.TP.Components.Inputs;
using TGC.MonoGame.TP.Gizmos;
using TGC.MonoGame.TP.HUD;

namespace TGC.MonoGame.TP.Actors
{
    public class GameObject
    {
        public Matrix World { get; set; }
        public Vector3 Position { get; set; }
        public float RotationAngle { get; set; }
        public Vector3 RotationDirection { get; set; }
        public float Scale { get; set; }
        public float Health { get; set; }
        public Vector3 Velocity { get; set; }
        public Model Model { get; set; }
        public Matrix[] Bones { get; set; }
        public List<IComponent> Components { get; }
        public CollisionComponent CollisionComponent { get; }

        public GameObject(List<IComponent> components, CollisionComponent collisionComponent, Vector3 position, float rotationAngle, Vector3 rotationDirection, float scale, float health)
        {
            Components = components;
            CollisionComponent = collisionComponent;

            Initialize(position, rotationDirection, rotationAngle, scale, health, Vector3.Zero);
        }

        public GameObject(IComponent component, CollisionComponent collisionComponent, Vector3 position, float rotationAngle, float scale, float health)
        {
            Components = new List<IComponent> { component };
            CollisionComponent = collisionComponent;

            Initialize(position, Vector3.Up, rotationAngle, scale, health, Vector3.Zero);
        }

        public GameObject(IComponent component, Vector3 position, float rotationAngle, float scale, float health)
        {
            Components = new List<IComponent> { component };
            CollisionComponent = new CollisionComponent();

            Initialize(position, Vector3.Up, rotationAngle, scale, health, Vector3.Zero);
        }

        private void Initialize(Vector3 position, Vector3 rotationDirection, float rotationAngle, float scale, float health, Vector3 velocity)
        {
            Position = position;
            RotationAngle = rotationAngle;
            RotationDirection = rotationDirection;
            Scale = scale;
            Health = health;
            Velocity = velocity;
            World = Matrix.CreateScale(Scale) * Matrix.CreateFromQuaternion(Quaternion.CreateFromAxisAngle(RotationDirection, MathHelper.ToRadians(RotationAngle))) * Matrix.CreateTranslation(Position);
        }

        public void LoadContent(ContentManager content)
        {
            foreach (var component in Components)
            {
                component.LoadContent(this, content);
            }
            CollisionComponent.LoadContent(this);
        }

        public void Update(GameTime gameTime)
        {

            float deltaTime = Convert.ToSingle(gameTime.ElapsedGameTime.TotalSeconds);

            foreach (var component in Components)
            {
                component.Update(this, gameTime);
            }

            Position += Velocity * deltaTime;

            CollisionComponent.Update(this);
        }

        public void Draw(GameTime gameTime, Matrix view, Matrix projection)
        {
            foreach (var component in Components)
            {
                component.Draw(this, gameTime, view, projection);
            }
        }

        public void Draw(GameTime gameTime, Matrix view, Matrix projection, Gizmos.Gizmos gizmos)
        {
            foreach (var component in Components)
            {
                component.Draw(this, gameTime, view, projection);
            }
            CollisionComponent.Draw(gizmos);
        }

        public bool CollidesWith(GameObject other)
        {
            return CollisionComponent.CollidesWith(other.CollisionComponent);
        }

        public float GetRotationAngleInRadians()
        {
            return MathHelper.ToRadians(RotationAngle);
        }

        public Matrix GetRotationMatrix()
        {
            return Matrix.CreateFromQuaternion(Quaternion.CreateFromAxisAngle(RotationDirection, GetRotationAngleInRadians()));
        }

        public void AddComponent(IComponent Component)
        {
            Components.Add(Component);
        }

    }
}
