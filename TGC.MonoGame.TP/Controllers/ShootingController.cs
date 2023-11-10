using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TGC.MonoGame.TP.Actors;
using TGC.MonoGame.TP.Components;
using TGC.MonoGame.TP.Components.Collisions;
using TGC.MonoGame.TP.Components.Graphics;
using TGC.MonoGame.TP.Components.Inputs;

namespace TGC.MonoGame.TP.Controllers
{
    class ShootingController
    {
        private const int MAX_BULLTES_AMOUNT = 5;

        private int currentBulletIdx = 0;
        private readonly Queue<int> ActiveBullets;
        private float CoolDown { get; }
        public float _cooldownTimer;
        private GameObject[] Bullets { get; }
        private SoundEffectInstance ShootingSoundEffect { get; set; }

        public ShootingController(float cooldown)
        {
            CoolDown = cooldown;
            _cooldownTimer = cooldown;

            Bullets = new GameObject[MAX_BULLTES_AMOUNT];
            ActiveBullets = new Queue<int>(MAX_BULLTES_AMOUNT);

            for (var i = 0; i < MAX_BULLTES_AMOUNT; i++) Bullets[i] = new(new BushGraphicsComponent());
        }

        private bool IsShooting()
        {
            return CoolDown > _cooldownTimer;
        }

        public void LoadContent(ContentManager Content)
        {
            var soundEffect = Content.Load<SoundEffect>("Audio/cannonFire");
            ShootingSoundEffect = soundEffect.CreateInstance();
            foreach (var bullet in Bullets) bullet.LoadContent(Content);
        }

        public void Update(GameTime gameTime)
        {
            float deltaTime = Convert.ToSingle(gameTime.ElapsedGameTime.TotalSeconds);

            if (IsShooting()) _cooldownTimer += deltaTime;

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
                _cooldownTimer = 0;

                // ADD BULLET TO QUEUE
                if (ActiveBullets.Count == MAX_BULLTES_AMOUNT) ActiveBullets.Dequeue();
                ActiveBullets.Enqueue(currentBulletIdx);

                // SET BULLET CONFIG
                var bullet = Bullets[currentBulletIdx];
                bullet.Position = position;
                bullet.RotationDirection = direction;
                bullet.Velocity = direction * speed;

                // PLAY SHOOTING SOUND
                ShootingSoundEffect.Play();

                currentBulletIdx++;
            }
        }

    }
}
