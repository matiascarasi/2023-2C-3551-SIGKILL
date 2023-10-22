using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualBasic.CompilerServices;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using TGC.MonoGame.TP.Collisions;
using TGC.MonoGame.TP.Content.Actors;
using TGC.MonoGame.TP.Controllers;
using TGC.MonoGame.TP.Defaults;
using TGC.MonoGame.TP.Graphics;
using TGC.MonoGame.TP.HUD;
using TGC.MonoGame.TP.Menu;

namespace TGC.MonoGame.TP
{
    /// <summary>
    ///     Esta es la clase principal del juego.
    ///     Inicialmente puede ser renombrado o copiado para hacer mas ejemplos chicos, en el caso de copiar para que se
    ///     ejecute el nuevo ejemplo deben cambiar la clase que ejecuta Program <see cref="Program.Main()" /> linea 10.
    /// </summary>
    public class TGCGame : Game
    {

        /// <summary>
        ///     Constructor del juego.
        /// </summary>
        /// 

        public TGCGame()
        {
            // Maneja la configuracion y la administracion del dispositivo grafico.
            Graphics = new GraphicsDeviceManager(this);

            Graphics.GraphicsProfile = GraphicsProfile.HiDef;

            // Para que el juego sea pantalla completa se puede usar Graphics IsFullScreen.
            // Carpeta raiz donde va a estar toda la Media.
            Content.RootDirectory = "Content";
            IsMouseVisible = true;

            Gizmos = new Gizmos.Gizmos();
        }
        public Gizmos.Gizmos Gizmos { get; }
        private GraphicsDeviceManager Graphics { get; }
        private GraphicsDevice _device { get; }
        private MenuComponent Menu { get; set; }
        private HUDComponent HUD { get; set; }
        private SpriteBatch SpriteBatch { get; set; }
        private Matrix View { get; set; }
        private Matrix Projection { get; set; }
        private GameObject Player { get; set; }
        private Terrain Terrain;
        public bool IsMenuActive { get; set; }


        public const string ContentFolderEffects = "Effects/";

        private MouseCamera MouseCamera { get; set; }

        private Song song { get; set; }

        List<GameObject> objects = new List<GameObject>();


        protected override void Initialize()
        {
            View = Matrix.CreateLookAt(new Vector3(0f, 2500f, 5000f), Vector3.Zero, Vector3.Up);
            Projection =
                Matrix.CreatePerspectiveFieldOfView(MathHelper.PiOver4, GraphicsDevice.Viewport.AspectRatio, 1, 200000);

            //Pantalla Completa
            Graphics.PreferredBackBufferWidth = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width - 100;
            Graphics.PreferredBackBufferHeight = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height - 100;
            Graphics.ApplyChanges();

            //song = Content.Load<Song>("Audio/mainSong");
            //MediaPlayer.Volume -= .7f;
            //MediaPlayer.IsRepeating = true;
            //MediaPlayer.Play(song);


            IsMenuActive = true;
            
            //Cambio textura de cursor
            Mouse.SetCursor(MouseCursor.FromTexture2D(Content.Load<Texture2D>("Textures/Menu/cursor"), 0, 0));

            Menu = new MenuComponent(this);
            HUD = new HUDComponent(PlayerDefaults.TankName, PlayerDefaults.Health);
            Terrain = new Terrain(Content, GraphicsDevice, "Textures/heightmaps/hills-heightmap", "Textures/heightmaps/hills", 20.0f, 8.0f);
            MouseCamera = new MouseCamera(GraphicsDevice);

            Random random = new Random();

            for (int i = 0; i < 20; i++)
            {

                float randomObjectX = (float)random.NextDouble() * 20000f - 10000f;
                float randomObjectZ = (float)random.NextDouble() * 20000f - 10000f;
                GameObject obj = new GameObject(
                    new RockGraphicsComponent(Content),
                    new PlayerInputComponent(0f, 0f, Terrain),
                    new Vector3(randomObjectX, Terrain.Height(randomObjectX, randomObjectZ), randomObjectZ),
                    0f,
                    1f,
                    10f
                );
                objects.Add(obj);
            }


            Player = new GameObject(
                new PanzerGraphicsComponent(Content),
                new PlayerInputComponent(PlayerDefaults.DriveSpeed, PlayerDefaults.RotationSpeed, Terrain),
                 new Vector3(0f, Terrain.Height(0f, 0f), 0f),
                PlayerDefaults.YAxisRotation,
                PlayerDefaults.Scale,
                PlayerDefaults.Health
            ); ;


            base.Initialize();
        }


        protected override void LoadContent()
        {
            SpriteBatch = new SpriteBatch(GraphicsDevice);
            Menu.LoadContent(Content, GraphicsDevice);
            HUD.LoadContent(Content);   
            Terrain.LoadContent(Content, GraphicsDevice);

            for (int i = 0; i < objects.Count; i++)
            {
                objects[i].LoadContent(objects[i]);
            }
            Player.LoadContent(Player);
            MouseCamera.SetModelVariables(Player.GraphicsComponent.Model, "Turret", "Cannon");

            Gizmos.LoadContent(GraphicsDevice, Content);

            base.LoadContent();
        }


        protected override void Update(GameTime gameTime)
        {
            // Aca deberiamos poner toda la logica de actualizacion del juego.

            // Capturar Input teclado
            if (Keyboard.GetState().IsKeyDown(Keys.Escape))
            {
                //Salgo del juego.
                Exit();
            }

            Menu.Update();
            HUD.Update(Player.Health);
            Player.Update(gameTime, MouseCamera, IsMenuActive);
            MouseCamera.Update(gameTime, Player.World, IsMenuActive);

            DetectCollisions();

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);
            Terrain.Draw(GraphicsDevice, MouseCamera.View, MouseCamera.Projection);


            foreach (var obj in objects)
            {
                obj.Draw(gameTime, MouseCamera.View, MouseCamera.Projection);
            }

            Player.Draw(gameTime, MouseCamera.View, MouseCamera.Projection);

            DrawGizmos(gameTime);

            if (IsMenuActive)
            {
                SpriteBatch.Begin();
                Menu.Draw(SpriteBatch);
                SpriteBatch.End();
            }
            else
            {
                SpriteBatch.Begin();
                HUD.Draw(SpriteBatch);
                SpriteBatch.End();
            }

        }
        protected override void UnloadContent()
        {
            Content.Unload();
            Terrain.UnloadContent();

            base.UnloadContent();
        }

        private void DetectCollisions()
        {
            var collision = false;
            for (int i = 0; i < objects.Count; i++)
            {
                if (objects[i].CollidesWith(Player))
                {
                    collision = true;
                }
            }
            var color = collision ? Color.Red : Color.Yellow;
            Gizmos.SetColor(color);

            Gizmos.UpdateViewProjection(MouseCamera.View, MouseCamera.Projection);
        }

        private void DrawGizmos(GameTime gameTime)
        {
            foreach (var obj in objects)
            {
                obj.DrawBoundingVolume(Gizmos);
            }
            Player.DrawBoundingVolume(Gizmos);
            Gizmos.Draw();
        }
    }
}