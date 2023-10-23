using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;
using System;
using TGC.MonoGame.TP.Components;
using TGC.MonoGame.TP.Controllers;

namespace TGC.MonoGame.TP.Components.Inputs
{
    class TankInputComponent : IInputComponent
    {
        private MovementController MovementController { get; set; }
        private MouseState PrevMouseState { get; set; }
        private SoundEffectInstance Instance { get; set; }
        private SoundEffect SoundEffect { get; set; }
        private readonly float _shootingCooldown;

        public TankInputComponent(float driveSpeed, float rotationSpeed, float shootingCooldown)
        {
            MovementController = new MovementController(driveSpeed, rotationSpeed);
            PrevMouseState = Mouse.GetState();
            _shootingCooldown = shootingCooldown;
        }

        public void LoadContent(ContentManager content) {
            SoundEffect = content.Load<SoundEffect>("Audio/cannonFire");
        }

        public void Update(GameObject Player, GameTime gameTime, MouseCamera mouseCamera, Terrain Terrain, bool IsMenuActive)
        {

            if (IsMenuActive) return;

            var keyboardState = Keyboard.GetState();
            var deltaTime = Convert.ToSingle(gameTime.ElapsedGameTime.TotalSeconds);

            var X = Player.Position.X;
            var Z = Player.Position.Z;
            var height = Terrain.Height(X, Z);
            Player.Position = new Vector3(X, height, Z);

            Player.CoolDown += deltaTime;
            //DETECCION DE CLICK
            if (Player.CoolDown > _shootingCooldown && PrevMouseState.LeftButton == ButtonState.Released && Mouse.GetState().LeftButton == ButtonState.Pressed)
            {
                // Player.ShootProyectile(deltaTime, mouseCamera);
                Instance = SoundEffect.CreateInstance();
                Instance.Play();
                Player.CoolDown = 0f;
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


