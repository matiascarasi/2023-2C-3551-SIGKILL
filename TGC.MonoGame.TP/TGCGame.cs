using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualBasic.CompilerServices;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using TGC.MonoGame.TP.Cameras;
using TGC.MonoGame.TP.Components;
using TGC.MonoGame.TP.Content.Actors;
using TGC.MonoGame.TP.Controllers;
using TGC.MonoGame.TP.Defaults;
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
        public TGCGame()
        {
            // Maneja la configuracion y la administracion del dispositivo grafico.
            Graphics = new GraphicsDeviceManager(this);

            Graphics.GraphicsProfile = GraphicsProfile.HiDef;

            // Para que el juego sea pantalla completa se puede usar Graphics IsFullScreen.
            // Carpeta raiz donde va a estar toda la Media.
            Content.RootDirectory = "Content";
            IsMouseVisible = false;
        }
        private GraphicsDeviceManager Graphics { get; }
        private SpriteBatch SpriteBatch { get; set; }
        private Matrix View { get; set; }
        private Matrix Projection { get; set; }
        private GameObject Player { get; set; }
        private GameObject Box { get; set; }
        private Terrain Terrain;


        public const string ContentFolderEffects = "Effects/";

        private Effect Effect { get; set; }

        private float Rotation { get; set; }
        private Matrix World { get; set; }

        private Model Model { get; set; }

        private MouseCamera MouseCamera { get; set; }



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

            Terrain = new Terrain(Content, GraphicsDevice, "Textures/heightmaps/hills-heightmap", "Textures/heightmaps/hills", 20.0f, 8.0f);
            Player = new GameObject(
                new TankGraphicsComponent(Content, PlayerDefaults.TankName, "Models/TankWars/Panzer/PzVl_Tiger_I_0", "Models/TankWars/Panzer/PzVI_Tiger_I_track_0"),
                new PlayerInputComponent(PlayerDefaults.DriveSpeed, PlayerDefaults.RotationSpeed),
                new Vector3(0f, Terrain.Height(0f, 0f), 0f),
                PlayerDefaults.YAxisRotation,
                PlayerDefaults.Scale
            );

            MouseCamera = new MouseCamera(GraphicsDevice);


            Random random = new Random();

            for (int i = 0; i < 20; i++)
            {

                float randomObjectX = (float)random.NextDouble() * 20000f - 10000f;
                float randomObjectZ = (float)random.NextDouble() * 20000f - 10000f;

                GameObject obj = new GameObject(
                    new MiscellaneousGraphicsComponent(Content, "Rock", "Rock07-Base"),
                    new PlayerInputComponent(0f, 0f),
                    new Vector3(randomObjectX, Terrain.Height(randomObjectX, randomObjectZ), randomObjectZ),
                    0f,
                    1f
                );
                objects.Add(obj);
            }

            base.Initialize();
        }


        protected override void LoadContent()
        {
            // SpriteBatch = new SpriteBatch(GraphicsDevice);


            Terrain.LoadContent(Content, GraphicsDevice);
            Player.LoadContent();
            MouseCamera.SetModelVariables(Player.Model, "Turret", "Cannon");
            //
            // Box.LoadContent();


            foreach (var obj in objects)
            {
                obj.LoadContent();
            }

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

            Player.Update(gameTime);
            MouseCamera.Update(gameTime, Player.World);

            base.Update(gameTime);
        }


        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Blue);
            Terrain.Draw(GraphicsDevice, MouseCamera.View, MouseCamera.Projection);

            Player.Draw(gameTime, MouseCamera.View, MouseCamera.Projection);


            foreach (var obj in objects)
            {
                obj.Draw(gameTime, MouseCamera.View, MouseCamera.Projection);
            }

        }


        protected override void UnloadContent()
        {
            Content.Unload();
            Terrain.UnloadContent();

            base.UnloadContent();
        }
    }
}