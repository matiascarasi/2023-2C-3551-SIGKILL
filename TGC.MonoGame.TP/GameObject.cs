﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using TGC.MonoGame.TP.Collisions;
using TGC.MonoGame.TP.Components.Collisions;
using TGC.MonoGame.TP.Components.Graphics;
using TGC.MonoGame.TP.Components.Inputs;

namespace TGC.MonoGame.TP
{
    public class GameObject
    {
        public Matrix World { get; set; }
        public Vector3 Position { get; set; }
        public float YAxisRotation { get; set; }
        public float Scale { get; set; }
        public float Health { get; set; }
        public Model Model { get; set; }
        public IGraphicsComponent GraphicsComponent { get; }

        private readonly IInputComponent InputComponent;

        private readonly CollisionComponent CollisionComponent;


        public GameObject(IGraphicsComponent graphics, IInputComponent inputComponent, CollisionComponent collisionComponent, Vector3 position, float yAxisRotation, float scale, float health)
        {
            GraphicsComponent = graphics;
            InputComponent = inputComponent;
            CollisionComponent = collisionComponent;

            World = Matrix.CreateScale(scale) * Matrix.CreateRotationY(MathHelper.ToRadians(yAxisRotation)) * Matrix.CreateTranslation(position);
            Position = position;
            YAxisRotation = yAxisRotation;
            Scale = scale;
            Health = health;

            Initialize();

        }

        private void Initialize()
        {
        }

        public void LoadContent(ContentManager content)
        {
            GraphicsComponent.LoadContent(this, content);
            CollisionComponent.LoadContent(this);
        }
        public void Update(GameTime gameTime, MouseCamera mouseCamera, bool IsMenuActive)
        {
            GraphicsComponent.Update(this, mouseCamera);
            InputComponent.Update(this, gameTime, mouseCamera, IsMenuActive);
            CollisionComponent.Update(this);

        }
        public void Draw(GameTime gameTime, Matrix view, Matrix projection)
        {
            GraphicsComponent.Draw(this, gameTime, view, projection);
        }

        public void Draw(GameTime gameTime, Matrix view, Matrix projection, Gizmos.Gizmos gizmos)
        {
            GraphicsComponent.Draw(this, gameTime, view, projection);
            CollisionComponent.Draw(gizmos);
        }

        public bool CollidesWith(GameObject gameObject)
        {
            return CollisionComponent.CollidesWith(gameObject.CollisionComponent);
        }
    }
}
