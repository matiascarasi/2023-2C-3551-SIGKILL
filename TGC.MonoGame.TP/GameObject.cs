using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using TGC.MonoGame.TP.Components;
using TGC.MonoGame.TP.Graphics;

namespace TGC.MonoGame.TP.Content.Actors
{
    class GameObject
    {
        public Matrix World { get; set; }
        public Vector3 Position { get; set; }
        public float YAxisRotation { get; set; }
        public float Scale { get; set; }
        public BoundingBox BoundingBox { get; set; }

        public IGraphicsComponent GraphicsComponent { get; }
        private readonly IInputComponent InputComponent;


        public GameObject(IGraphicsComponent graphics, IInputComponent inputComponent, Vector3 position, float yAxisRotation, float scale, BoundingBox box)
        {
            GraphicsComponent = graphics;
            InputComponent = inputComponent;
            World = Matrix.CreateScale(scale) * Matrix.CreateRotationY(MathHelper.ToRadians(yAxisRotation)) * Matrix.CreateTranslation(position);
            Position = position;
            YAxisRotation = yAxisRotation;
            Scale = scale;
            BoundingBox = box;
        }

        public void LoadContent(GameObject gameObject)
        {
            GraphicsComponent.LoadContent(gameObject);
        }
        public void Update(GameTime gameTime, List<GameObject> objects, MouseCamera mouseCamera)
        {
            InputComponent.Update(this, gameTime, objects, mouseCamera);
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
    }
}
