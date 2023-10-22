using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using TGC.MonoGame.TP.Collisions;
using TGC.MonoGame.TP.Components;
using TGC.MonoGame.TP.Graphics;

namespace TGC.MonoGame.TP.Content.Actors
{
    public class GameObject
    {
        public Matrix World { get; set; }
        public Vector3 Position { get; set; }
        public float YAxisRotation { get; set; }
        public float Scale { get; set; }
        public float Health { get; set; }
        public float CoolDown { get; set; }
        public IGraphicsComponent GraphicsComponent { get; }
        public Model Model
        {
            get
            {
                return GraphicsComponent.Model;
            }
        }
        private readonly IInputComponent InputComponent;

        private CollisionComponent CollisionComponent;


        public GameObject(IGraphicsComponent graphics, IInputComponent inputComponent, Vector3 position, float yAxisRotation, float scale, float health, float cooldown)
        {
            GraphicsComponent = graphics;
            InputComponent = inputComponent;
            World = Matrix.CreateScale(scale) * Matrix.CreateRotationY(MathHelper.ToRadians(yAxisRotation)) * Matrix.CreateTranslation(position);
            Position = position;
            YAxisRotation = yAxisRotation;
            Scale = scale;
            Health = health;
            CoolDown = cooldown;
        }

        public void LoadContent(GameObject gameObject, ContentManager content)
        {
            InputComponent.LoadContent(content);
            GraphicsComponent.LoadContent(gameObject);
            CollisionComponent = new CollisionComponent(this);
        }
        public void Update(GameTime gameTime, MouseCamera mouseCamera, bool IsMenuActive)
        {
            InputComponent.Update(this, gameTime, mouseCamera, IsMenuActive);
            CollisionComponent.Update(Position, YAxisRotation);
        }
        public void Draw(GameTime gameTime, Matrix view, Matrix projection)
        {
            GraphicsComponent.Draw(this, gameTime, view, projection);
        }
        public void ShootProyectile(float deltatime, MouseCamera camera)
        {
            System.Diagnostics.Debug.WriteLine("CLICK");
            System.Diagnostics.Debug.WriteLine(camera.FollowedPosition);

        }
        public bool CollidesWith(GameObject gameObject)
        {
            return CollisionComponent.CollidesWith(gameObject.CollisionComponent);
        }

        public void DrawBoundingVolume(Gizmos.Gizmos gizmos)
        {
            CollisionComponent.DrawBoundingVolume(gizmos);
        }
    }
}
