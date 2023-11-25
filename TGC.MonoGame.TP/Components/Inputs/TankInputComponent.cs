using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Reflection.Metadata;
using TGC.MonoGame.TP.Actors;
using TGC.MonoGame.TP.Components;
using TGC.MonoGame.TP.Components.Collisions;
using TGC.MonoGame.TP.Components.Graphics;
using TGC.MonoGame.TP.Controllers;
using TGC.MonoGame.TP.Gizmos;
using TGC.MonoGame.TP.HUD;

namespace TGC.MonoGame.TP.Components.Inputs
{
    class TankInputComponent : IComponent
    {
        const int MAX_BULLETS_AMOUNT = 3;
        const float BULLET_SPEED = 8000f;
        private MovementController MovementController { get; set; }
        private ShootingController ShootingController { get; set; }
        private MouseState PrevMouseState { get; set; }
        private MouseCamera MouseCamera { get; }
        private HeightMap HeightMap { get; }
        private HUDComponent HUDComponent { get; }

        public TankInputComponent(float driveSpeed, float rotationSpeed, float shootingCooldown, MouseCamera mouseCamera, HeightMap heightMap, HUDComponent hudComponent)
        {
            MovementController = new MovementController(driveSpeed, rotationSpeed);
            ShootingController = new ShootingController(shootingCooldown, MAX_BULLETS_AMOUNT);
            PrevMouseState = Mouse.GetState();

            MouseCamera = mouseCamera;
            HeightMap = heightMap;
            HUDComponent = hudComponent;
        }

        public void Update(GameObject Player, GameTime gameTime)
        {

            if (!(Player.GraphicsComponent is TankGraphicsComponent)) return;

            var tankGraphics = Player.GraphicsComponent as TankGraphicsComponent;

            ShootingController.Update(Player, gameTime);
            MovementController.Update(Player, gameTime);

            var keyboardState = Keyboard.GetState();
            var deltaTime = Convert.ToSingle(gameTime.ElapsedGameTime.TotalSeconds);

            var X = Player.Position.X;
            var Z = Player.Position.Z; 
            var height = HeightMap.Height(X, Z);
            Player.Position = new Vector3(X, height, Z);

            //DETECCION DE MOVIMIENTO DEL MOUSE
            tankGraphics.CannonRotation = TankGraphicsComponent.FixCannonAngle(MouseCamera.UpDownRotation);
            tankGraphics.TurretRotation = MouseCamera.LeftRightRotation;

            //DETECCION DE CLICK
            if (Mouse.GetState().LeftButton == ButtonState.Pressed && PrevMouseState.LeftButton == ButtonState.Released)
            {
                var direction = tankGraphics.GetCannonDirection(Player);
                var position = tankGraphics.GetCannonEnd(Player);
                ShootingController.Shoot(position, direction, BULLET_SPEED);
            }
            HUDComponent.UpdatePlayerCooldown(ShootingController.CooldownTimer);

            //DETECCION DE TECLA
            PrevMouseState = Mouse.GetState();

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

        }

        public void LoadContent(GameObject gameObject, ContentManager content)
        {
            ShootingController.LoadContent(content);
        }

        public void Draw(GameObject gameObject, GameTime gameTime, Matrix view, Matrix projection, Vector3 cameraPosition)
        {
            ShootingController.Draw(gameTime, view, projection, cameraPosition);
        }
    }

}


