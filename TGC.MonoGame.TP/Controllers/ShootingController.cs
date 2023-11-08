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
        private float CoolDown { get; }
        private float _cooldownTimer;
        private List<GameObject> Bullets { get; }
        private SoundEffectInstance ShootingSoundEffect { get; set; }

        public ShootingController(float cooldown)
        {
            CoolDown = cooldown;
            _cooldownTimer = cooldown;
            Bullets = new List<GameObject>();
        }

        private bool IsShooting()
        {
            return CoolDown > _cooldownTimer;
        }

        public void LoadContent(ContentManager Content)
        {
            var soundEffect = Content.Load<SoundEffect>("Audio/cannonFire");
            ShootingSoundEffect = soundEffect.CreateInstance();
        }

        public void Update(GameTime gameTime, MouseCamera mouseCamera, Terrain Terrain, bool IsMenuActive)
        {

            float deltaTime = Convert.ToSingle(gameTime.ElapsedGameTime.TotalSeconds);

            _cooldownTimer += deltaTime;
            foreach (var bullet in Bullets)
            {
                bullet.Update(gameTime);
            }
        }

        public void Draw(GameTime gameTime, Matrix view, Matrix projection)
        {
            foreach (var bullet in Bullets)
            {
                bullet.Draw(gameTime, view, projection);
            }
        }

        public void Shoot(Vector3 position, Vector3 direction, float speed)
        {
            if(!IsShooting())
            {

                _cooldownTimer = 0;

                var bullet = new GameObject(
                    new List<IComponent> { new BulletGraphicsComponent() },
                    position,
                    0f,
                    Vector3.Up,
                    20f,
                    1f
                );

                Bullets.Add(bullet);

                ShootingSoundEffect.Play();

            }
        }

    }
}
