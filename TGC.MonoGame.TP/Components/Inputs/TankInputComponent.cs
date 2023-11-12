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
        const float MAX_TURRET_ANGLE = -0.05f;
        const int MAX_BULLETS_AMOUNT = 3;
        private MovementController MovementController { get; set; }
        private ShootingController ShootingController { get; set; }
        private MouseState PrevMouseState { get; set; }
        private MouseCamera MouseCamera { get; }
        private Terrain Terrain { get; }
        private TankGraphicsComponent GraphicsComponent { get; }
        private HUDComponent HUDComponent { get; }

        public TankInputComponent(float driveSpeed, float rotationSpeed, float shootingCooldown, MouseCamera mouseCamera, Terrain terrain, HUDComponent hudComponent, TankGraphicsComponent graphicsComponent)
        {
            MovementController = new MovementController(driveSpeed, rotationSpeed);
            ShootingController = new ShootingController(shootingCooldown, MAX_BULLETS_AMOUNT);
            PrevMouseState = Mouse.GetState();

            MouseCamera = mouseCamera;
            Terrain = terrain;
            GraphicsComponent = graphicsComponent;
            HUDComponent = hudComponent;
        }

        public void Update(GameObject Player, GameTime gameTime)
        {

            ShootingController.Update(Player, gameTime);
            MovementController.Update(Player, gameTime);

            var keyboardState = Keyboard.GetState();
            var deltaTime = Convert.ToSingle(gameTime.ElapsedGameTime.TotalSeconds);

            var X = Player.Position.X;
            var Z = Player.Position.Z; 
            var height = MathF.Max(Terrain.Height(X, Z), Player.Position.Y);
            Player.Position = new Vector3(X, height, Z);

            //DETECCION DE MOVIMIENTO DEL MOUSE
            GraphicsComponent.CannonRotation = MathF.Max(MouseCamera.UpDownRotation, MAX_TURRET_ANGLE);
            GraphicsComponent.TurretRotation = MouseCamera.LeftRightRotation;

            //DETECCION DE CLICK
            if (Mouse.GetState().LeftButton == ButtonState.Pressed && PrevMouseState.LeftButton == ButtonState.Released)
            {
                var direction = GraphicsComponent.GetCannonDirection(Player);
                ShootingController.Shoot(Player.Position, direction, 8000f);
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

        public void Draw(GameObject gameObject, GameTime gameTime, Matrix view, Matrix projection)
        {
            ShootingController.Draw(gameTime, view, projection);
        }
    }

}


