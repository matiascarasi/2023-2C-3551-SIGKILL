using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;
using System;
using TGC.MonoGame.TP.Components;
using TGC.MonoGame.TP.Content.Actors;

namespace TGC.MonoGame.TP.Controllers
{
    class PlayerInputComponent : IInputComponent
    {
        private MovementController MovementController { get; set; }
        private Terrain Terrain { get; set; }
        private MouseState prevMouseState { get; set; }
        private SoundEffectInstance Instance { get; set; }
        private SoundEffect SoundEffect { get; set; }
        private float shootingCooldown;


        public PlayerInputComponent(float driveSpeed, float rotationSpeed, Terrain terrain, float shootingCooldown)
        {
            MovementController = new MovementController(driveSpeed, rotationSpeed);
            Terrain = terrain;
            prevMouseState = Mouse.GetState();
            this.shootingCooldown = shootingCooldown;
        }

        public void LoadContent(ContentManager content) {

            SoundEffect = content.Load<SoundEffect>("Audio/cannonFire");
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

            Player.CoolDown += deltaTime;
            //DETECCION DE CLICK
            if (Player.CoolDown > shootingCooldown && prevMouseState.LeftButton == ButtonState.Released && Mouse.GetState().LeftButton == ButtonState.Pressed)
            {
                Player.ShootProyectile(deltaTime, mouseCamera);
                Instance = SoundEffect.CreateInstance();
                Instance.Play();
                Player.CoolDown = 0f;
            }

            prevMouseState = Mouse.GetState();

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


