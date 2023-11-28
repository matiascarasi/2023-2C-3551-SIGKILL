using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using TGC.MonoGame.TP.Components;
using TGC.MonoGame.TP.Components.Physics;
using TGC.MonoGame.TP.Components.Graphics;
using TGC.MonoGame.TP.Gizmos;

namespace TGC.MonoGame.TP.Actors
{
    public class GameObject
    {
        private Vector3 _position;
        private float _rotationAngle;
        private Vector3 _rotationDirection;
        public Matrix World { get; set; }
        public Vector3 Position
        {
            get => PhysicsComponent != null ? PhysicsComponent.Position : _position;
            set
            {
                _position = value;
            }
        }
        public float RotationAngle
        {
            get => PhysicsComponent != null ? PhysicsComponent.Orientation.W : _rotationAngle;
            set
            {
                _rotationAngle = value;
            }
        }
        public Vector3 RotationDirection
        {
            get => PhysicsComponent != null ? new(PhysicsComponent.Orientation.X, PhysicsComponent.Orientation.Y, PhysicsComponent.Orientation.Z) : _rotationDirection;
            set
            {
                _rotationDirection = value;
            }
        }
        public Quaternion Rotation { get => PhysicsComponent != null ? PhysicsComponent.Orientation : Quaternion.CreateFromAxisAngle(RotationDirection, GetRotationAngleInRadians()); }
        public float Scale { get; set; }
        public float Health { get; set; }
        public Vector3 Velocity { get; set; }
        public Model Model { get; set; }
        public Matrix[] Bones { get; set; }
        public List<IComponent> Components { get; }
        public IPhysicsComponent PhysicsComponent { get; }
        public GraphicsComponent GraphicsComponent { get; }
        private readonly ShapeDrawer ShapeDrawer = new();

        public GameObject(List<IComponent> components, GraphicsComponent graphicsComponent, IPhysicsComponent physicsComponent, float scale, float health)
        {
            Components = components;
            PhysicsComponent = physicsComponent;
            GraphicsComponent = graphicsComponent;

            Initialize(scale, health, Vector3.Zero);
        }

        public GameObject(GraphicsComponent graphicsComponent, IPhysicsComponent physicsComponent, float scale, float health)
        {
            Components = new List<IComponent>();
            PhysicsComponent = physicsComponent;
            GraphicsComponent = graphicsComponent;

            Initialize(scale, health, Vector3.Zero);
        }

        public GameObject(GraphicsComponent graphicsComponent, Vector3 position, float rotationAngle, float scale, float health)
        {
            Components = new List<IComponent>();
            GraphicsComponent = graphicsComponent;

            Initialize(position, Vector3.Up, rotationAngle, scale, health, Vector3.Zero);
        }

        private void Initialize(Vector3 position, Vector3 rotationDirection, float rotationAngle, float scale, float health, Vector3 velocity)
        {
            Position = position;
            RotationAngle = rotationAngle;
            RotationDirection = rotationDirection;
            Initialize(scale, health, velocity);
        }

        private void Initialize(float scale, float health, Vector3 velocity)
        {
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
            GraphicsComponent.LoadContent(this, content);
        }

        public void Update(GameTime gameTime)
        {

            float deltaTime = Convert.ToSingle(gameTime.ElapsedGameTime.TotalSeconds);

            foreach (var component in Components)
            {
                component.Update(this, gameTime);
            }

            Position += Velocity * deltaTime;

            GraphicsComponent.Update(this, gameTime);
            PhysicsComponent?.Update(gameTime);
        }

        public void Draw(GameTime gameTime, Matrix view, Matrix projection, Vector3 cameraPosition)
        {
            foreach (var component in Components)
            {
                component.Draw(this, gameTime, view, projection, cameraPosition);
            }
            GraphicsComponent.Draw(this, gameTime, view, projection, cameraPosition);
        }

        public void Draw(GameTime gameTime, Matrix view, Matrix projection, Vector3 cameraPosition, Gizmos.Gizmos gizmos)
        {
            foreach (var component in Components)
            {
                component.Draw(this, gameTime, view, projection, cameraPosition);
            }
            GraphicsComponent.Draw(this, gameTime, view, projection, cameraPosition);
            PhysicsComponent.Draw(ShapeDrawer);
        }

        public void OnCollide(GameObject other)
        {
            //CollisionComponent.OnCollide(this, other);
        }

        public float GetRotationAngleInRadians()
        {
            return MathHelper.ToRadians(RotationAngle);
        }

        public Matrix GetRotationMatrix()
        {
            return Matrix.CreateFromQuaternion(Rotation);
        }

        public void AddComponent(IComponent Component)
        {
            Components.Add(Component);
        }

    }
}
