using BepuPhysics.Constraints;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Reflection.Metadata;
using System.Text;

namespace TGC.MonoGame.TP
{
    /// <summary>
    /// Una camara que sigue objetos
    /// </summary>
    class MouseCamera
    {
        private const float AxisDistanceToTarget = 1800f;
        private const float UpVectorDistance =  500f;
        private const float AngleFollowSpeed = 3f;
        private float BackVectorInterpolator { get; set; } = 0f;
        private float LeftRightRotation { get; set; } = 0f;
        private float UpDownRotation { get; set; } = 0f;

        private Quaternion LeftRightQuat { get; set; }
        private Quaternion UpDownQuat { get; set; }
        private Quaternion rotation { get; set; }
        private Vector3 UpCamera { get; set; } = Vector3.Up / 3;

        private Matrix cameraAngle { get; set; } = Matrix.Identity;

        private MouseState mouse { get; set; }

        public Matrix Projection { get; private set; }

        public Matrix View { get; private set; }

        private Vector3 FrontVector{ get; set; }

        private Vector2 screenCenterCoordinates{ get; set; }
        private float mouseSensibility { get; set; } = 10f;


        /// <summary>
        /// Crea una FollowCamera que sigue a una matriz de mundo
        /// </summary>
        /// <param name="aspectRatio"></param>
        public MouseCamera(GraphicsDevice GraphicsDevice)
        {
            // Orthographic camera
            // Projection = Matrix.CreateOrthographic(screenWidth, screenHeight, 0.01f, 10000f);
            var aspectRatio = GraphicsDevice.Viewport.AspectRatio;
            screenCenterCoordinates = new Vector2 ((GraphicsDevice.Viewport.Width / 2), (GraphicsDevice.Viewport.Height / 2) );

            // Perspective camera
            // Uso 60° como FOV, aspect ratio, pongo las distancias a near plane y far plane en 0.1 y 100000 (mucho) respectivamente
            

            Projection = Matrix.CreatePerspectiveFieldOfView(MathF.PI / 3.5f, aspectRatio, 0.1f, 100000f);
        }

        /// <summary>
        /// Actualiza la Camara usando una matriz de mundo actualizada para seguirla
        /// </summary>
        /// <param name="gameTime">The Game Time to calculate framerate-independent movement</param>
        /// <param name="followedWorld">The World matrix to follow</param>
        public void Update(GameTime gameTime, Matrix followedWorld)
        {


            var elapsedTime = Convert.ToSingle(gameTime.ElapsedGameTime.TotalSeconds);


            var mouseDifference = new Vector2(0f,0f);
            mouse = Mouse.GetState();

            if (mouse.X != screenCenterCoordinates.X || mouse.Y != screenCenterCoordinates.Y)
            {
                mouseDifference.X = mouse.X - screenCenterCoordinates.X;
                mouseDifference.Y =  mouse.Y - screenCenterCoordinates.Y ;

                UpDownRotation = Math.Min(Math.Max(UpDownRotation - mouseDifference.Y * AngleFollowSpeed * elapsedTime, -0.75f), 0.25f);
                LeftRightRotation -= mouseDifference.X * AngleFollowSpeed * elapsedTime;

                LeftRightQuat = Quaternion.CreateFromAxisAngle(followedWorld.Up, LeftRightRotation);
                UpDownQuat = Quaternion.CreateFromAxisAngle(followedWorld.Right, UpDownRotation);
                Mouse.SetPosition((int)screenCenterCoordinates.X, (int)screenCenterCoordinates.Y);
            }


            cameraAngle = Matrix.CreateFromQuaternion(UpDownQuat) * Matrix.CreateFromQuaternion(LeftRightQuat);

            var followedPosition = Vector3.Transform(followedWorld.Forward * AxisDistanceToTarget + UpCamera * AxisDistanceToTarget , cameraAngle * Matrix.CreateTranslation(followedWorld.Translation));

            //var offsetedPosition = Vector3.Transform(CurrentTargetPosition, cameraAngle);
            var offsetedPosition = Vector3.Transform(followedWorld.Backward * AxisDistanceToTarget + UpCamera * AxisDistanceToTarget, cameraAngle * Matrix.CreateTranslation(followedWorld.Translation)) ;

            var forward = (followedWorld.Translation - offsetedPosition);
            forward.Normalize();
            var right = Vector3.Cross(forward, Vector3.Up);
            var cameraCorrectUp = Vector3.Cross(right, forward);

           

            View = Matrix.CreateLookAt(offsetedPosition, followedPosition, cameraCorrectUp);


        }
    }
}
