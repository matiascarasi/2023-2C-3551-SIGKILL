using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TGC.MonoGame.TP.Components;
using TGC.MonoGame.TP.Content.Actors;

namespace TGC.MonoGame.TP.Controllers
{
    class PlayerInputComponent : IInputComponent
    {
        private MovementController MovementController { get; set; }

        public PlayerInputComponent(float driveSpeed, float rotationSpeed)
        {
            MovementController = new MovementController(driveSpeed, rotationSpeed);
        }

        public void Update(GameObject Player, GameTime gameTime)
        {
            var keyboardState = Keyboard.GetState();
            var deltaTime = Convert.ToSingle(gameTime.ElapsedGameTime.TotalSeconds);

            if (keyboardState.IsKeyDown(Keys.A))
            {
                MovementController.RotateLeft(Player, deltaTime);
            }
            else if (keyboardState.IsKeyDown(Keys.D))
            {
                MovementController.RotateRight(Player, deltaTime);
            }

            if (keyboardState.IsKeyDown(Keys.W)){
                MovementController.Accelerate(Player);
            } else if (keyboardState.IsKeyDown(Keys.S))
            {
                MovementController.Decelerate(Player);
            } else
            {
                MovementController.Settle(Player);
            }

            MovementController.Move(Player, deltaTime);
        }

    }
}
