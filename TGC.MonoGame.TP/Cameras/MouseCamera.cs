using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace TGC.MonoGame.TP
{
    /// <summary>
    /// Una camara que sigue objetos
    /// </summary>
    public class MouseCamera
    {
        private const float UpVectorDistance = 500f;
        private const float AngleFollowSpeed = .25f;
        private const float AngleThreshold = 0.85f;
        private float AxisDistanceToTarget = 10000f;

        private float zoomVal = 1f;
        private float BackVectorInterpolator { get; set; } = 0f;
        public float LeftRightRotation { get; set; } = 0f;
        public float UpDownRotation { get; set; } = 0f;
        private Vector3 PastBackwardVector { get; set; } = Vector3.Backward;
        private Vector3 CurrentBackwarVector { get; set; } = Vector3.Backward;
        private Quaternion LeftRightQuat { get; set; }
        private Quaternion UpDownQuat { get; set; }
        private Vector3 UpCamera { get; set; } = Vector3.Up / 3;
        public Matrix cameraAngle { get; set; } = Matrix.Identity;

        private MouseState mouse { get; set; }

        public Matrix Projection { get; private set; }

        public Matrix View { get; private set; }

        public Vector3 FollowedPosition { get; set; }

        public Vector3 OffsetPosition { get; set; }

        private Vector2 screenCenterCoordinates { get; set; }
        private float mouseSensibility { get; set; } = 10f;

        /// <summary>
        /// Crea una FollowCamera que sigue a una matriz de mundo
        /// </summary>
        /// <param name="aspectRatio"></param>
        public MouseCamera(GraphicsDevice GraphicsDevice)
        {
            var aspectRatio = GraphicsDevice.Viewport.AspectRatio;
            screenCenterCoordinates = new Vector2((GraphicsDevice.Viewport.Width / 2), (GraphicsDevice.Viewport.Height / 2));
            Projection = Matrix.CreatePerspectiveFieldOfView(MathF.PI / 3.5f, aspectRatio, 0.1f, 500000f);
        }

        public void Update(GameTime gameTime, Matrix playerWorld, bool IsMenuActive)
        {

            var elapsedTime = Convert.ToSingle(gameTime.ElapsedGameTime.TotalSeconds);

            if (IsMenuActive) SetMenuCamera(playerWorld); 
            else SetPlayerCamera(elapsedTime, playerWorld);

            var forward = (playerWorld.Translation - OffsetPosition);
            forward.Normalize();
            var right = Vector3.Cross(forward, Vector3.Up);
            var cameraCorrectUp = Vector3.Cross(right, forward);

            View = Matrix.CreateLookAt(OffsetPosition, FollowedPosition, cameraCorrectUp);

        }

        public void SetPlayerCamera(float elapsedTime, Matrix playerWorld)
        {
            AxisDistanceToTarget = 1800f;

            var mouseDifference = new Vector2(0f, 0f);

            mouse = Mouse.GetState();

            mouseDifference.X = mouse.X - screenCenterCoordinates.X;
            mouseDifference.Y = mouse.Y - screenCenterCoordinates.Y;

            UpDownRotation = Math.Min(Math.Max(UpDownRotation - mouseDifference.Y * AngleFollowSpeed * elapsedTime, -0.75f), 0.25f);
            LeftRightRotation -= mouseDifference.X * AngleFollowSpeed * elapsedTime;

            LeftRightQuat = Quaternion.CreateFromAxisAngle(playerWorld.Up, LeftRightRotation);
            UpDownQuat = Quaternion.CreateFromAxisAngle(playerWorld.Right, UpDownRotation);
            Mouse.SetPosition((int)screenCenterCoordinates.X, (int)screenCenterCoordinates.Y);

            cameraAngle = Matrix.CreateFromQuaternion(UpDownQuat) * Matrix.CreateFromQuaternion(LeftRightQuat);

            FollowedPosition = Vector3.Transform(playerWorld.Forward * AxisDistanceToTarget * zoomVal + UpCamera * AxisDistanceToTarget, cameraAngle * Matrix.CreateTranslation(playerWorld.Translation));
            OffsetPosition = Vector3.Transform(playerWorld.Backward * AxisDistanceToTarget * zoomVal + UpCamera * AxisDistanceToTarget, cameraAngle * Matrix.CreateTranslation(playerWorld.Translation));

        }

        public void SetMenuCamera(Matrix playerWorld)
        {
            FollowedPosition = Vector3.Transform(playerWorld.Forward * AxisDistanceToTarget * zoomVal + UpCamera * AxisDistanceToTarget, cameraAngle * Matrix.CreateTranslation(playerWorld.Translation));
            OffsetPosition = Vector3.Transform(playerWorld.Backward * AxisDistanceToTarget * zoomVal + UpCamera * AxisDistanceToTarget, cameraAngle * Matrix.CreateTranslation(playerWorld.Translation));
        }

    }
}
