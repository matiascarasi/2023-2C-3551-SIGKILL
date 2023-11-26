using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;
using System;
using TGC.MonoGame.TP.Actors;
using TGC.MonoGame.TP.Components.Graphics;
using TGC.MonoGame.TP.Components.Physics;
using TGC.MonoGame.TP.Controllers;
using TGC.MonoGame.TP.HUD;

namespace TGC.MonoGame.TP.Components.Inputs
{
    class TankInputComponent : IComponent
    {
        const int MAX_BULLETS_AMOUNT = 3;
        const float BULLET_SPEED = 8000f;
        private IDynamicPhysicsComponent PhysicsComponent { get; set; }
        private ShootingController ShootingController { get; set; }
        private MouseState PrevMouseState { get; set; }
        private MouseCamera MouseCamera { get; }
        private Terrain Terrain { get; }
        private HUDComponent HUDComponent { get; }

        public TankInputComponent(float driveSpeed, float rotationSpeed, float shootingCooldown, MouseCamera mouseCamera, Terrain terrain, HUDComponent hudComponent, IDynamicPhysicsComponent physicsComponent)
        {
            PhysicsComponent = physicsComponent;
            ShootingController = new ShootingController(shootingCooldown, MAX_BULLETS_AMOUNT);
            PrevMouseState = Mouse.GetState();

            MouseCamera = mouseCamera;
            Terrain = terrain;
            HUDComponent = hudComponent;
        }

        public void Update(GameObject Player, GameTime gameTime, GraphicsComponent graphicsComponent)
        {

            if (!(graphicsComponent is TankGraphicsComponent)) return;

            var tankGraphics = graphicsComponent as TankGraphicsComponent;

            ShootingController.Update(Player, gameTime);

            var keyboardState = Keyboard.GetState();
            var deltaTime = Convert.ToSingle(gameTime.ElapsedGameTime.TotalSeconds);

            var X = Player.Position.X;
            var Z = Player.Position.Z; 
            var height = Terrain.Height(X, Z);

            //DETECCION DE MOVIMIENTO DEL MOUSE
            tankGraphics.CannonRotation = tankGraphics.FixCannonAngle(MouseCamera.UpDownRotation);
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
                PhysicsComponent.Accelerate();
            }
            else if (keyboardState.IsKeyDown(Keys.S))
            {
                PhysicsComponent.Decelerate();
            }

            if (keyboardState.IsKeyDown(Keys.A))
            {
                PhysicsComponent.TurnLeft(deltaTime);
            }
            else if (keyboardState.IsKeyDown(Keys.D))
            {
                PhysicsComponent.TurnRight(deltaTime);
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


