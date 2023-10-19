using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using TGC.MonoGame.TP.Collisions;
using TGC.MonoGame.TP.Components;
using TGC.MonoGame.TP.Content.Actors;

namespace TGC.MonoGame.TP.Controllers
{
    class PlayerInputComponent : IInputComponent
    {
        private MovementController MovementController { get; set; }
        private ShootingController ShootingController { get; set; }
        private Terrain Terrain { get; set; }
        private MouseState prevMouseState { get; set; }


        public bool isCollision;

        public PlayerInputComponent(float driveSpeed, float rotationSpeed, Terrain terrain, MouseCamera camera)
        {
            MovementController = new MovementController(driveSpeed, rotationSpeed);
            ShootingController = new ShootingController(camera);
            Terrain = terrain;
            prevMouseState = Mouse.GetState();
        }

        public void Update(GameObject Player, GameTime gameTime, List<GameObject> objects, MouseCamera mouseCamera)
        {


            var keyboardState = Keyboard.GetState();
            var deltaTime = Convert.ToSingle(gameTime.ElapsedGameTime.TotalSeconds);

            var X = Player.Position.X;
            var Z = Player.Position.Z;

            var height = Terrain.Height(X, Z);
            var obb = Player.OBB;
            var obbX = Player.OBB.Extents.X;
            var obbY = Player.OBB.Extents.Y;
            var obbZ = Player.OBB.Extents.Z;
            Player.Position = new Vector3(X, height, Z);



            //DETECCION DE CLICK
            if (prevMouseState.LeftButton == ButtonState.Released && Mouse.GetState().LeftButton == ButtonState.Pressed)
            {
                ShootingController.Shoot(objects);
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


            // Create an OBB World-matrix so we can draw a cube representing it
            obb.Center = Player.Position;
            obb.Orientation = Matrix.CreateRotationY((MathHelper.ToRadians(Player.YAxisRotation)));
            Player.OBBWorld = Matrix.CreateScale(new Vector3(obbX * 2f,obbY * 2f, obbZ * 2f)) *
                 Matrix.CreateRotationY(MathHelper.ToRadians(Player.YAxisRotation)) *
                 Matrix.CreateTranslation(Player.Position + new Vector3(0f, 150f, 0f));

            //DETECCION DE COLISIÓN
            isCollision = CheckForCollisions(objects, Player.OBB);
            if (isCollision) {
                MovementController.Stop();
            };


        }
        public static bool CheckForCollisions(List<GameObject> objects, OrientedBoundingBox playerbox)
        {
            for (int i = 0; i < objects.Count; i++)
            {
                if (objects[i].OBB.Intersects(playerbox))
                {
                    System.Diagnostics.Debug.WriteLine(objects[i].OBB.Center);
                    return true;
                }
            }
            return false;
        }

    }

}


