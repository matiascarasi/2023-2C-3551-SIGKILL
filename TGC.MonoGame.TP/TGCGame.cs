﻿using System;
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
            // Hace que el mouse sea visible.
            IsMouseVisible = true;
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

        
        private Camera Camera { get; set; }


        protected override void Initialize()
        {

            IsMouseVisible = true;
            View = Matrix.CreateLookAt(new Vector3(0f, 2500f, 5000f), Vector3.Zero, Vector3.Up);
            Projection =
                Matrix.CreatePerspectiveFieldOfView(MathHelper.PiOver4, GraphicsDevice.Viewport.AspectRatio, 1, 200000);

            Terrain = new Terrain("Textures/heightmaps/hills-heightmap", "Textures/heightmaps/hills");

            Player = new GameObject(
                new TankGraphicsComponent(Content, PlayerDefaults.TankName),
                new PlayerInputComponent(PlayerDefaults.DriveSpeed, PlayerDefaults.RotationSpeed),
                PlayerDefaults.Position,
                PlayerDefaults.YAxisRotation,
                PlayerDefaults.Scale
            );
            
            Box = new GameObject(
                new MiscellaneousGraphicsComponent(Content, "Rock", "Rock07-Base"),
                new PlayerInputComponent(0f, 0f),
                new Vector3(0f, 300f, 0f),
                0f,
                1f
            );


            Camera = new SimpleCamera(GraphicsDevice.Viewport.AspectRatio, new Vector3(-400f, 1000f, 2000f), 400, 1.0f, 1,
                int.MaxValue);

            base.Initialize();
        }


        protected override void LoadContent()
        {
            // Aca es donde deberiamos cargar todos los contenido necesarios antes de iniciar el juego.
            SpriteBatch = new SpriteBatch(GraphicsDevice);

            Terrain.LoadContent(Content, GraphicsDevice);
            
            Player.LoadContent();
            
            Box.LoadContent();
            
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
            Camera.Update(gameTime);

            Player.Update(gameTime);
            
            base.Update(gameTime);
        }


        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Blue);
            Terrain.Draw(GraphicsDevice, Camera.View, Camera.Projection);

            Box.Draw(gameTime, Camera.View, Camera.Projection);
            
            Player.Draw(gameTime, Camera.View, Camera.Projection);

        }

        
        protected override void UnloadContent()
        {
            Content.Unload();
            Terrain.UnloadContent();

            base.UnloadContent();
        }
    }
}