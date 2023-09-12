using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using TGC.MonoGame.TP.Components;

namespace TGC.MonoGame.TP
{
    class GameObject
    {
        public Effect Effect { get; set; }
        public Model Model { get; set; }
        public Matrix World { get; set; }
        public Vector3 Position { get; set; }
        public float YAxisRotation { get; set; }
        public float Scale { get; set; }

        private readonly IGraphicsComponent GraphicsComponent;
        private readonly IInputComponent InputComponent;

        public GameObject(IGraphicsComponent graphics, IInputComponent inputComponent, Vector3 position, float yAxisRotation, float scale)
        {
            GraphicsComponent = graphics;
            InputComponent = inputComponent;
            World = Matrix.CreateScale(scale) * Matrix.CreateRotationY(MathHelper.ToRadians(yAxisRotation)) * Matrix.CreateTranslation(position);
            Position = position;
            YAxisRotation = yAxisRotation;
            Scale = scale;
        }

        public void LoadContent(ContentManager content) 
        {
            GraphicsComponent.LoadContent(this, content);
        }
        public void Update(GameTime gameTime)
        {
            InputComponent.Update(this, gameTime);
        }

        public void Draw(GameTime gameTime, Matrix view, Matrix projection)
        {
            GraphicsComponent.Draw(this, gameTime, view, projection);
        }
    }
}
