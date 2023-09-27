﻿using System;
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

        public IGraphicsComponent GraphicsComponent { get; }
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

        public void LoadContent()
        {
            GraphicsComponent.LoadContent();
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
