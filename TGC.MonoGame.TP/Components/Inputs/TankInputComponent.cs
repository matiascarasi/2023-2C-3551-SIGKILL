using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using TGC.MonoGame.TP.Components;
using TGC.MonoGame.TP.Controllers;

namespace TGC.MonoGame.TP.Components.Inputs
{
    class TankInputComponent : IInputComponent
    {
        private MovementController MovementController { get; set; }
        private Terrain Terrain { get; set; }
        private MouseState PrevMouseState { get; set; }

        public TankInputComponent(float driveSpeed, float rotationSpeed, Terrain terrain)
        {
            MovementController = new MovementController(driveSpeed, rotationSpeed);
            Terrain = terrain;
            PrevMouseState = Mouse.GetState();
        }

        public void Update(GameObject Player, GameTime gameTime, MouseCamera mouseCamera, bool IsMenuActive)
        {

            if (IsMenuActive) return;

            var keyboardState = Keyboard.GetState();
            var deltaTime = Convert.ToSingle(gameTime.ElapsedGameTime.TotalSeconds);

            var X = Player.Position.X;
            var Z = Player.Position.Z;

            var height = Terrain.Height(X, Z);
            Player.Position = new Vector3(X, height, Z);


            //DETECCION DE CLICK
            if (PrevMouseState.LeftButton == ButtonState.Released && Mouse.GetState().LeftButton == ButtonState.Pressed)
            {
                // Player.ShootProyectile(deltaTime, mouseCamera);
            }

            PrevMouseState = Mouse.GetState();

            //DETECCION DE TECLA
            if (keyboardState.IsKeyDown(Keys.W))
            {
                MovementController.Accelerate();
            }
            else if (keyboardState.IsKeyDown(Keys.S))
            {
                MovementController.Decelerate();
            }
            else
            {
                MovementController.Settle();
            }

            if (keyboardState.IsKeyDown(Keys.A))
            {
                MovementController.TurnLeft(Player, deltaTime);
            }
            else if (keyboardState.IsKeyDown(Keys.D))
            {
                MovementController.TurnRight(Player, deltaTime);
            }

            MovementController.Move(Player, deltaTime);
        }
    }

}


