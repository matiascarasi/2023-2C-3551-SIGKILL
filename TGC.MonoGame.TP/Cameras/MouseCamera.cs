using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using TGC.MonoGame.TP.Content.Actors;

namespace TGC.MonoGame.TP
{
    /// <summary>
    /// Una camara que sigue objetos
    /// </summary>
    class MouseCamera
    {
        private const float AxisDistanceToTarget = 1800f;
        private const float UpVectorDistance = 500f;
        private const float AngleFollowSpeed = .10f;
        private const float AngleThreshold = 0.85f;

        private float zoomVal = 1f;
        private float BackVectorInterpolator { get; set; } = 0f;
        private float LeftRightRotation { get; set; } = 0f;
        private float UpDownRotation { get; set; } = 0f;
        private Vector3 PastBackwardVector { get; set; } = Vector3.Backward;
        private Vector3 CurrentBackwarVector { get; set; } = Vector3.Backward;


        private Quaternion LeftRightQuat { get; set; }
        private Quaternion UpDownQuat { get; set; }
        private Vector3 UpCamera { get; set; } = Vector3.Up / 3;

        private Matrix cameraAngle { get; set; } = Matrix.Identity;

        private MouseState mouse { get; set; }

        public Matrix Projection { get; private set; }

        public Matrix View { get; private set; }

        private Vector3 FollowedPosition { get; set; }

        private Vector3 OffsetPosition { get; set; }

        private Vector2 screenCenterCoordinates { get; set; }
        private float mouseSensibility { get; set; } = 10f;

        private ModelBone cannonBone { get; set; }
        private ModelBone turretBone { get; set; }
        private Matrix turretTransform { get; set; }
        private Matrix cannonTransform { get; set; }

        /// <summary>
        /// Crea una FollowCamera que sigue a una matriz de mundo
        /// </summary>
        /// <param name="aspectRatio"></param>
        public MouseCamera(GraphicsDevice GraphicsDevice)
        {

            var aspectRatio = GraphicsDevice.Viewport.AspectRatio;
            screenCenterCoordinates = new Vector2((GraphicsDevice.Viewport.Width / 2), (GraphicsDevice.Viewport.Height / 2));

            Projection = Matrix.CreatePerspectiveFieldOfView(MathF.PI / 3.5f, aspectRatio, 0.1f, 100000f);
        }


        public void SetModelVariables(Model model, String turretBoneName, String CannonBoneName)
        {

            turretBone = model.Bones[turretBoneName];
            cannonBone = model.Bones[CannonBoneName];

            turretTransform = turretBone.Transform;
            cannonTransform = cannonBone.Transform;
        }

        /// <summary>
        /// Actualiza la Camara usando una matriz de mundo actualizada para seguirla
        /// </summary>
        /// <param name="gameTime">The Game Time to calculate framerate-independent movement</param>
        /// <param name="followedWorld">The World matrix to follow</param>
        public void Update(GameTime gameTime, Matrix playerWorld)
        {

            var elapsedTime = Convert.ToSingle(gameTime.ElapsedGameTime.TotalSeconds);

            var mouseDifference = new Vector2(0f, 0f);

            mouse = Mouse.GetState();

            //if((float) mouse.ScrollWheelValue / 1000f + 1f - zoomVal != 0f)
            //{
            //    zoomVal = Math.Min(Math.Max((float) mouse.ScrollWheelValue / 1000f, .5f), 2f);
            //}


            mouseDifference.X = mouse.X - screenCenterCoordinates.X;
            mouseDifference.Y = mouse.Y - screenCenterCoordinates.Y;

            UpDownRotation = Math.Min(Math.Max(UpDownRotation - mouseDifference.Y * AngleFollowSpeed * elapsedTime, -0.75f), 0.25f);
            LeftRightRotation -= mouseDifference.X * AngleFollowSpeed * elapsedTime;

            LeftRightQuat = Quaternion.CreateFromAxisAngle(playerWorld.Up, LeftRightRotation);
            UpDownQuat = Quaternion.CreateFromAxisAngle(playerWorld.Right, UpDownRotation);
            Mouse.SetPosition((int)screenCenterCoordinates.X, (int)screenCenterCoordinates.Y);

            cameraAngle = Matrix.CreateFromQuaternion(UpDownQuat) * Matrix.CreateFromQuaternion(LeftRightQuat);

            if (UpDownRotation > -0.25f) cannonBone.Transform = Matrix.CreateRotationX(-UpDownRotation) * cannonTransform;
            turretBone.Transform = Matrix.CreateRotationY(LeftRightRotation) * turretTransform;

            FollowedPosition = Vector3.Transform(playerWorld.Forward * AxisDistanceToTarget * zoomVal + UpCamera * AxisDistanceToTarget, cameraAngle * Matrix.CreateTranslation(playerWorld.Translation));

            //var offsetedPosition = Vector3.Transform(CurrentTargetPosition, cameraAngle);
            OffsetPosition = Vector3.Transform(playerWorld.Backward * AxisDistanceToTarget * zoomVal + UpCamera * AxisDistanceToTarget, cameraAngle * Matrix.CreateTranslation(playerWorld.Translation));


            var forward = (playerWorld.Translation - OffsetPosition);
            forward.Normalize();
            var right = Vector3.Cross(forward, Vector3.Up);
            var cameraCorrectUp = Vector3.Cross(right, forward);

            View = Matrix.CreateLookAt(OffsetPosition, FollowedPosition, cameraCorrectUp);

        }
    }
}
