using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using System;
using System.Collections.Generic;
using TGC.MonoGame.TP.Actors;
using TGC.MonoGame.TP.Components.Graphics;
using TGC.MonoGame.TP.Helpers;

namespace TGC.MonoGame.TP.Controllers
{
    class ShootingController
    {
        private int _currentBulletIdx = 0;
        private readonly int _maxBulletsAmount;
        private readonly LinkedList<int> ActiveBullets;
        private readonly float _coolDown;
        public float CooldownTimer { get; set; }
        private GameObject[] Bullets { get; }
        private SoundEffectInstance ShootingSoundEffect { get; set; }

        public ShootingController(float cooldown, int maxBulletsAmount)
        {
            _coolDown = cooldown;
            CooldownTimer = cooldown;
            _maxBulletsAmount = maxBulletsAmount;

            Bullets = new GameObject[_maxBulletsAmount];
            ActiveBullets = new LinkedList<int>();

            for (var i = 0; i < _maxBulletsAmount; i++) Bullets[i] = new(new BulletGraphicsComponent(), Vector3.Zero, 0f, 20f, 0f);
        }

        private bool IsShooting()
        {
            return _coolDown > CooldownTimer;
        }

        public void LoadContent(ContentManager Content)
        {
            var soundEffect = Content.Load<SoundEffect>("Audio/cannonFire");
            ShootingSoundEffect = soundEffect.CreateInstance();
            foreach (var bullet in Bullets) bullet.LoadContent(Content);
        }

        public void Update(GameObject gameObject, GameTime gameTime)
        {
            float deltaTime = Convert.ToSingle(gameTime.ElapsedGameTime.TotalSeconds);

            if (IsShooting()) CooldownTimer += deltaTime;

            foreach (var i in ActiveBullets) Bullets[i].Update(gameTime);
        }

        public void Draw(GameTime gameTime, Matrix view, Matrix projection)
        {
            foreach (var i in ActiveBullets) Bullets[i].Draw(gameTime, view, projection);
        }

        public void Shoot(Vector3 position, Vector3 direction, float speed)
        {
            if(!IsShooting())
            {
                // START COOLDOWN
                CooldownTimer = 0;
                
                // STOP SHOOTING SOUND
                ShootingSoundEffect.Stop();

                // ADD BULLET TO QUEUE
                if (ActiveBullets.Count == _maxBulletsAmount) ActiveBullets.RemoveLast();
                ActiveBullets.AddFirst(_currentBulletIdx);

                // GET ROTATIONS ANGLE AND DIRECTION
                var right = Vector3.Cross(direction, Vector3.Up);
                var upDirection = Vector3.Cross(right, direction);

                var rotationAngle = AlgebraHelper.GetAngleBetweenTwoVectors(Vector3.Forward, direction);
                
                // SET BULLET CONFIG
                var bullet = Bullets[_currentBulletIdx];
                bullet.Position = position;
                bullet.RotationDirection = upDirection;
                bullet.RotationAngle = rotationAngle;
                bullet.Velocity = direction * speed;

                // PLAY SHOOTING SOUND
                ShootingSoundEffect.Play();

                _currentBulletIdx++;
                if (_currentBulletIdx == _maxBulletsAmount) _currentBulletIdx = 0;
            }
        }

    }
}
