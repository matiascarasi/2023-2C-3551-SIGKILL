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
        public AITankComponent(float driveSpeed, float rotationSpeed, float cooldown, float minDistance, GameObject target, List<GameObject> objects, Terrain terrain)
        {
            MovementController = new MovementController(driveSpeed, rotationSpeed);
            ShootingController = new ShootingController(cooldown, MAX_BULLETS_AMOUNT);
            PathFindingController = new PathFindingController(target, minDistance, objects, MovementController);
            Terrain = terrain;
        }
        public void Update(GameObject gameObject, GameTime gameTime, GraphicsComponent graphicsComponent)
        {

            if (!(graphicsComponent is TankGraphicsComponent)) return;

            var tankGraphics = graphicsComponent as TankGraphicsComponent;

            var X = gameObject.Position.X;
            var Z = gameObject.Position.Z;
            var height = Terrain.Height(X, Z);
            gameObject.Position = new Vector3(X, height, Z);

            ShootingController.Update(gameObject, gameTime);
            PathFindingController.Update(gameObject, gameTime);
            MovementController.Update(gameObject, gameTime);

            var rotationAngle = MathHelper.ToRadians(PathFindingController.GetRotationAngle());
            tankGraphics.TurretRotation = rotationAngle;
            
            ShootingController.Shoot(tankGraphics.GetCannonEnd(gameObject), tankGraphics.GetCannonDirection(gameObject), BULLET_SPEED);

        }

        public void Draw(GameObject gameObject, GameTime gameTime, Matrix view, Matrix projection)
        {
            ShootingController.Draw(gameTime, view, projection);
        }

        public void LoadContent(GameObject gameObject, ContentManager content)
        {
            ShootingController.LoadContent(content);
        }

        
    }
}
