using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TGC.MonoGame.TP.Actors;
using TGC.MonoGame.TP.Components.Graphics;
using TGC.MonoGame.TP.Controllers;
using TGC.MonoGame.TP.Helpers;

namespace TGC.MonoGame.TP.Components.AI
{
    class AITankComponent : IComponent
    {
        private const int MAX_BULLETS_AMOUNT = 5;
        const float BULLET_SPEED = 8000f;
        private MovementController MovementController { get; }
        private ShootingController ShootingController { get; }
        private PathFindingController PathFindingController { get; } 
        private Terrain Terrain { get; }
        public AITankComponent(float driveSpeed, float rotationSpeed, float cooldown, float minDistance, List<GameObject> targets, GameObject initialTarget, List<GameObject> objects, Terrain terrain)
        {
            MovementController = new MovementController(driveSpeed, rotationSpeed);
            ShootingController = new ShootingController(cooldown, MAX_BULLETS_AMOUNT);
            PathFindingController = new PathFindingController(targets, initialTarget, minDistance, objects, MovementController);
            Terrain = terrain;
        }
        public void Update(GameObject gameObject, GameTime gameTime)
        {

            if (!(gameObject.GraphicsComponent is TankGraphicsComponent)) return;

            var tankGraphics = gameObject.GraphicsComponent as TankGraphicsComponent;

            var X = gameObject.Position.X;
            var Z = gameObject.Position.Z;
            var height = Terrain.Height(X, Z);
            gameObject.Position = new Vector3(X, height, Z);

            ShootingController.Update(gameObject, gameTime);
            PathFindingController.Update(gameObject, gameTime);
            MovementController.Update(gameObject, gameTime);

            var direction = PathFindingController.GetDirection();
            var forward = new Vector3(direction.X, 0f, direction.Z);
            var terrainRatioXZ = Terrain.GetScaleY() / Terrain.GetScaleXZ();

            var turretAngle = MathHelper.ToRadians(AlgebraHelper.GetAngleBetweenTwoVectors(gameObject.World.Forward, forward));
            var cannonAngle = MathF.Atan2(direction.Y, forward.Length() * terrainRatioXZ);

            tankGraphics.TurretRotation = turretAngle;
            tankGraphics.CannonRotation = TankGraphicsComponent.FixCannonAngle(cannonAngle);

            ShootingController.Shoot(tankGraphics.GetCannonEnd(gameObject), tankGraphics.GetCannonDirection(gameObject), BULLET_SPEED);

        }

        public void Draw(GameObject gameObject, GameTime gameTime, Matrix view, Matrix projection, Vector3 cameraPosition)
        {
            ShootingController.Draw(gameTime, view, projection, cameraPosition);
        }

        public void LoadContent(GameObject gameObject, ContentManager content)
        {
            ShootingController.LoadContent(content);
        }

        
    }
}
