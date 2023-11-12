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
        private MovementController MovementController { get; }
        private ShootingController ShootingController { get; }
        private PathFindingController PathFindingController { get; } 
        private TankGraphicsComponent GraphicsComponent { get; }
        private Terrain Terrain { get; }
        public AITankComponent(float driveSpeed, float rotationSpeed, float cooldown, float minDistance, GameObject target, List<GameObject> objects, Terrain terrain, TankGraphicsComponent graphicsComponent)
        {
            MovementController = new MovementController(driveSpeed, rotationSpeed);
            ShootingController = new ShootingController(cooldown, MAX_BULLETS_AMOUNT);
            PathFindingController = new PathFindingController(target, minDistance, objects, MovementController);
            GraphicsComponent = graphicsComponent;
            Terrain = terrain;
        }
        public void Update(GameObject gameObject, GameTime gameTime)
        {
            var X = gameObject.Position.X;
            var Z = gameObject.Position.Z;
            var height = MathF.Max(Terrain.Height(X, Z), gameObject.Position.Y);
            gameObject.Position = new Vector3(X, height, Z);

            ShootingController.Update(gameObject, gameTime);
            PathFindingController.Update(gameObject, gameTime);
            MovementController.Update(gameObject, gameTime);

            var rotationAngle = MathHelper.ToRadians(PathFindingController.GetRotationAngle());
            GraphicsComponent.TurretRotation = rotationAngle;
            
            ShootingController.Shoot(gameObject.Position, GraphicsComponent.GetCannonDirection(gameObject), 800f);

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
