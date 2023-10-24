using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;
using System;
using System.Reflection.Metadata;
using TGC.MonoGame.TP.Components;
using TGC.MonoGame.TP.Components.Collisions;
using TGC.MonoGame.TP.Components.Graphics;
using TGC.MonoGame.TP.Controllers;
using TGC.MonoGame.TP.Gizmos;

namespace TGC.MonoGame.TP.Components.Inputs
{
    class TankInputComponent : IInputComponent
    {
        private MovementController MovementController { get; set; }
        private MouseState PrevMouseState { get; set; }
        private SoundEffectInstance Instance { get; set; }
        private SoundEffect SoundEffect { get; set; }
        private readonly float _shootingCooldown;
        private GameObject Bullet;
        private Vector3 _shootFrom;
        private Vector3 _shootTo;

        public TankInputComponent(float driveSpeed, float rotationSpeed, float shootingCooldown)
        {
            MovementController = new MovementController(driveSpeed, rotationSpeed);
            PrevMouseState = Mouse.GetState();
            _shootingCooldown = shootingCooldown;
            Bullet = new GameObject(
                    new BulletGraphicsComponent(),
                    new NoInputComponent(),
                    new CollisionComponent(),
                    new Vector3(0, 0, 0),
                    0f,
                    20f,
                    1f,
                    0f
                );

        }

        public void LoadContent(ContentManager content) {
            SoundEffect = content.Load<SoundEffect>("Audio/cannonFire");
            Bullet.LoadContent(content);
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

                Bullet.Position = Player.Position + new Vector3(0f, 100f, 0f);
                Bullet.Update(gameTime, mouseCamera, Terrain, IsMenuActive);
                _shootFrom = mouseCamera.OffsetPosition;
                _shootTo = mouseCamera.FollowedPosition;

                Instance = SoundEffect.CreateInstance();
                Instance.Play();
                Player.CoolDown = 0f;
            }
            else if (Player.CoolDown < _shootingCooldown)
            {
                var aux = _shootTo - _shootFrom;
                aux.Normalize();
                Bullet.Position += aux * 10000f * deltaTime;
                Bullet.Update(gameTime, mouseCamera, Terrain, IsMenuActive);

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
        public void Draw(GameTime gameTime, Matrix view, Matrix projection)
        {
            Bullet.Draw(gameTime, view, projection);
        }
        public void Draw(GameTime gameTime, Matrix view, Matrix projection, Gizmos.Gizmos gizmos) {
            Bullet.Draw(gameTime, view, projection, gizmos);
        }
       
    }

}


