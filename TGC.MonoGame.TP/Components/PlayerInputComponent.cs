using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using TGC.MonoGame.TP.Components;
using TGC.MonoGame.TP.Content.Actors;

namespace TGC.MonoGame.TP.Controllers
{
    class PlayerInputComponent : IInputComponent
    {
        private MovementController MovementController { get; set; }
        private Terrain Terrain { get; set; }
        private MouseState prevMouseState { get; set; }


        public bool isCollision;

        public PlayerInputComponent(float driveSpeed, float rotationSpeed, Terrain terrain)
        {
            MovementController = new MovementController(driveSpeed, rotationSpeed);
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
            Player.Position = new Vector3(X, height, Z);


            //DETECCION DE CLICK
            if (prevMouseState.LeftButton == ButtonState.Released && Mouse.GetState().LeftButton == ButtonState.Pressed)
            {
                Player.ShootProyectile(deltaTime, mouseCamera);
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


            //DETECCION DE COLISIÓN
            isCollision = CheckForCollisions(objects, Player.BoundingBox);
            if (isCollision) {
                MovementController.Stop();
            };


        }

        public bool CheckForCollisions(List<GameObject> objects, BoundingBox playerbox)
        {
            for (int i = 0; i < objects.Count ; i++)
            {
                if (objects[i].BoundingBox.Intersects(playerbox)) return true;
            }
            return false;
        }
    }

}


