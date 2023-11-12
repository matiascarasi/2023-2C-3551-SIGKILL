using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using TGC.MonoGame.TP.Components;
using TGC.MonoGame.TP.Components.Collisions;

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
        public List<IComponent> Components { get; }
        public ICollisionComponent CollisionComponent { get; }

        public GameObject(List<IComponent> components, ICollisionComponent collisionComponent, Vector3 position, float rotationAngle, Vector3 rotationDirection, float scale, float health)
        {
            Components = components;
            CollisionComponent = collisionComponent;

            Initialize(position, rotationDirection, rotationAngle, scale, health, Vector3.Zero);
        }

        public GameObject(List<IComponent> components, Vector3 position, float rotationAngle, Vector3 rotationDirection, float scale, float health)
        {
            Components = components;
            CollisionComponent = new Components.Collisions.OrientedBoundingBoxComponent();

            Initialize(position, rotationDirection, rotationAngle, scale, health, Vector3.Zero);
        }

        public GameObject(IComponent component)
        {
            Components = new List<IComponent> { component };
            CollisionComponent = new Components.Collisions.OrientedBoundingBoxComponent();

            Initialize(Vector3.Zero, Vector3.Up, 0f, 1f, 0f, Vector3.Zero);
        }

        public GameObject(IComponent component, Vector3 position, float rotationAngle, Vector3 rotationDirection, float scale, float health)
        {
            Components = new List<IComponent> { component };
            CollisionComponent = new Components.Collisions.OrientedBoundingBoxComponent();

            Initialize(position, rotationDirection, rotationAngle, scale, health, Vector3.Zero);
        }

        public GameObject(IComponent component, Vector3 position, float rotationAngle, float scale, float health)
        {
            Components = new List<IComponent> { component };
            CollisionComponent = new Components.Collisions.OrientedBoundingBoxComponent();

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
    }
}
